using System.Collections;
using System.Collections.Immutable;
using System.Reflection;

namespace QQBot.Commands;

internal sealed class NamedArgumentTypeReader<T> : TypeReader
    where T : class, new()
{
    private static readonly IReadOnlyDictionary<string, PropertyInfo> _tProps = typeof(T).GetTypeInfo()
        .DeclaredProperties
        .Where(p => p.SetMethod != null && p.SetMethod.IsPublic && !p.SetMethod.IsStatic)
        .ToImmutableDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

    private readonly CommandService _commands;

    public NamedArgumentTypeReader(CommandService commands)
    {
        _commands = commands;
    }

    public override async Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
    {
        T result = new();
        ReadState state = ReadState.LookingForParameter;
        int beginRead = 0, currentRead = 0;

        while (state != ReadState.End)
        {
            try
            {
                PropertyInfo? prop = Read(out string arg);
                object? propVal = await ReadArgumentAsync(prop, arg).ConfigureAwait(false);
                if (prop?.SetMethod is not null && propVal != null)
                    prop.SetMethod.Invoke(result, [propVal]);
                else
                {
                    return TypeReaderResult.FromError(CommandError.ParseFailed,
                        $"Could not parse the argument for the parameter '{prop?.Name}' as type '{prop?.PropertyType}'.");
                }
            }
            catch (Exception ex)
            {
                return TypeReaderResult.FromError(ex);
            }
        }

        return TypeReaderResult.FromSuccess(result);

        PropertyInfo? Read(out string arg)
        {
            string? currentParam = null;
            char match = '\0';

            for (; currentRead < input.Length; currentRead++)
            {
                char currentChar = input[currentRead];
                switch (state)
                {
                    case ReadState.LookingForParameter:
                        if (char.IsWhiteSpace(currentChar))
                            continue;
                        beginRead = currentRead;
                        state = ReadState.InParameter;
                        break;
                    case ReadState.InParameter:
                        if (currentChar != ':')
                            continue;
                        currentParam = input.Substring(beginRead, currentRead - beginRead);
                        state = ReadState.LookingForArgument;
                        break;
                    case ReadState.LookingForArgument:
                        if (char.IsWhiteSpace(currentChar))
                            continue;
                        beginRead = currentRead;
                        state = QuotationAliasUtils.DefaultAliasMap.TryGetValue(currentChar, out match)
                            ? ReadState.InQuotedArgument
                            : ReadState.InArgument;
                        break;
                    case ReadState.InArgument:
                        if (!char.IsWhiteSpace(currentChar))
                            continue;
                        return GetPropAndValue(out arg);
                    case ReadState.InQuotedArgument:
                        if (currentChar != match)
                            continue;
                        return GetPropAndValue(out arg);
                }
            }

            if (currentParam == null)
                throw new InvalidOperationException("No parameter name was read.");

            return GetPropAndValue(out arg);

            PropertyInfo? GetPropAndValue(out string argv)
            {
                bool quoted = state == ReadState.InQuotedArgument;
                state = currentRead == (quoted ? input.Length - 1 : input.Length)
                    ? ReadState.End
                    : ReadState.LookingForParameter;

                if (quoted)
                {
                    argv = input.Substring(beginRead + 1, currentRead - beginRead - 1).Trim();
                    currentRead++;
                }
                else
                    argv = input.Substring(beginRead, currentRead - beginRead);

                if (currentParam == null) return null;
                return _tProps[currentParam];
            }
        }

        async Task<object?> ReadArgumentAsync(PropertyInfo? prop, string arg)
        {
            if (prop is null) return null;
            Type elemType = prop.PropertyType;
            bool isCollection = false;
            if (elemType.GetTypeInfo().IsGenericType && elemType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                elemType = prop.PropertyType.GenericTypeArguments[0];
                isCollection = true;
            }

            OverrideTypeReaderAttribute? overridden = prop.GetCustomAttribute<OverrideTypeReaderAttribute>();
            TypeReader? reader = overridden != null
                ? ModuleClassBuilder.GetTypeReader(_commands, elemType, overridden.TypeReader, services)
                : _commands.GetDefaultTypeReader(elemType)
                ?? _commands.GetTypeReaders(elemType)?.FirstOrDefault().Value;

            if (reader != null)
            {
                if (isCollection)
                {
                    MethodInfo method = _readMultipleMethod.MakeGenericMethod(elemType);
                    Task<IEnumerable>? task = (Task<IEnumerable>?)method.Invoke(null, [reader, context, arg.Split(','), services]);
                    if (task is not null)
                        return await task.ConfigureAwait(false);
                }

                return await ReadSingle(reader, context, arg, services).ConfigureAwait(false);
            }

            return null;
        }
    }

    private static async Task<object?> ReadSingle(TypeReader reader, ICommandContext context, string arg, IServiceProvider services)
    {
        TypeReaderResult readResult = await reader.ReadAsync(context, arg, services).ConfigureAwait(false);
        return readResult.IsSuccess ? readResult.BestMatch : null;
    }

    private static async Task<IEnumerable> ReadMultiple<TObj>(TypeReader reader, ICommandContext context, IEnumerable<string> args,
        IServiceProvider services)
    {
        List<TObj> objs = [];
        foreach (string arg in args)
        {
            object? read = await ReadSingle(reader, context, arg.Trim(), services).ConfigureAwait(false);
            if (read != null)
                objs.Add((TObj)read);
        }

        return objs.ToImmutableArray();
    }

    private static readonly MethodInfo _readMultipleMethod = typeof(NamedArgumentTypeReader<T>)
        .GetTypeInfo()
        .DeclaredMethods
        .Single(m => m.IsPrivate && m.IsStatic && m.Name == nameof(ReadMultiple));

    private enum ReadState
    {
        LookingForParameter,
        InParameter,
        LookingForArgument,
        InArgument,
        InQuotedArgument,
        End
    }
}
