// using System.Globalization;
//
// namespace QQBot.Commands;
//
// /// <summary>
// ///     表示一个用于解析实现了 <see cref="QQBot.IRole"/> 的对象的类型读取器。
// /// </summary>
// /// <typeparam name="T"> 要解析为的角色类型。 </typeparam>
// public class RoleTypeReader<T> : TypeReader
//     where T : class, IRole
// {
//     /// <inheritdoc />
//     public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
//     {
//         if (context.Guild != null)
//         {
//             var results = new Dictionary<ulong, TypeReaderValue>();
//             var roles = context.Guild.Roles;
//
//             //By Mention (1.0)
//             if (MentionUtils.TryParseRole(input, out var id))
//                 AddResult(results, context.Guild.GetRole(id) as T, 1.00f);
//
//             //By Id (0.9)
//             if (ulong.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out id))
//                 AddResult(results, context.Guild.GetRole(id) as T, 0.90f);
//
//             //By Name (0.7-0.8)
//             foreach (var role in roles.Where(x => string.Equals(input, x.Name, StringComparison.OrdinalIgnoreCase)))
//                 AddResult(results, role as T, role.Name == input ? 0.80f : 0.70f);
//
//             if (results.Count > 0)
//                 return Task.FromResult(TypeReaderResult.FromSuccess(results.Values.ToReadOnlyCollection()));
//         }
//         return Task.FromResult(TypeReaderResult.FromError(CommandError.ObjectNotFound, "Role not found."));
//     }
//
//     private void AddResult(Dictionary<ulong, TypeReaderValue> results, T? role, float score)
//     {
//         if (role != null && !results.ContainsKey(role.Id))
//             results.Add(role.Id, new TypeReaderValue(role, score));
//     }
// }
