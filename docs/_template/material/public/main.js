export default {
    iconLinks: [
        {
            icon: 'github',
            href: 'https://github.com/gehongyan/QQBot.Net',
            title: 'GitHub'
        },
        {
            icon: 'box-seam-fill',
            href: 'https://www.nuget.org/packages/QQBot.Net/',
            title: 'NuGet'
        },
        {
            icon: 'chat',
            href: 'http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=QiRvU4EFUavyNKQLKyfymezkG9H46cY6&authKey=OnAwAOWySUTds7YJUhaiS%2Bpr%2FWYLKSIPAPzdnhsM4RgAgWRQKZywjc6RSEAnDfNM&noverify=0&group_code=849595128',
            title: 'QQGroup'
        }
    ],
    start: () =>
    {
        let target = document.getElementById("toc");

        if (!target) return;

        let config = { attributes: false, childList: true, subtree: true };
        let observer = new MutationObserver((list) =>
        {
            for (const mutation of list)
            {
                if (mutation.type === "childList" && mutation.target == target)
                {
                    let scrollValue = localStorage.getItem("tocScroll");

                    // Add event to store scroll pos.
                    let tocDiv = target.getElementsByClassName("flex-fill")[0];

                    tocDiv.addEventListener("scroll", (event) =>
                    {
                        if (event.target.scrollTop >= 0)
                        {
                            localStorage.setItem("tocScroll", event.target.scrollTop);
                        }
                    });

                    if (scrollValue && scrollValue >= 0)
                    {
                        tocDiv.scroll(0, scrollValue);
                    }

                    observer.disconnect();
                    break;
                }
            }
        });

        observer.observe(target, config);
    }
}
