import {codeToHtml} from 'https://esm.sh/shiki@3.0.0'

window.highlightAll = async () => {

    const codes = document.querySelectorAll(".markdown pre > code");

    for (const code of codes) {

        const pre = code.parentElement;

        let language = "js";
        for (const className of code.classList) {
            if (className.startsWith("language-")) {
                language = className.slice("language-".length);
                break;
            }
        }

        pre.outerHTML = (await codeToHtml(code.innerHTML
                .replaceAll("&lt;", "<")
                .replaceAll("&gt;", ">"),
            {
                "lang": language,
                "theme": "github-dark-default"
            })).replace('<code>', "<code class='language'>");

    }
    
}