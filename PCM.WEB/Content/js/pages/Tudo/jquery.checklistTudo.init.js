// ==========================================================================
// Tudo em Dia — Checklist (lista/painel)
// Interações da página. Filtros de unidade/setor e status são via navegação
// (form GET + links da torre). Aqui só o toggle grade/lista.
// ==========================================================================
(function () {
    "use strict";

    document.addEventListener("DOMContentLoaded", function () {

        var btnGrid = document.getElementById("btnGrid");
        var btnList = document.getElementById("btnList");

        function setView(mode) {
            var compact = mode === "list";
            document.querySelectorAll(".local-grid").forEach(function (g) {
                g.style.gridTemplateColumns = compact ? "1fr" : "";
            });
            if (btnGrid) btnGrid.classList.toggle("active", !compact);
            if (btnList) btnList.classList.toggle("active", compact);
            try { localStorage.setItem("tudo_view", mode); } catch (e) { }
        }

        if (btnGrid) btnGrid.addEventListener("click", function () { setView("grid"); });
        if (btnList) btnList.addEventListener("click", function () { setView("list"); });

        var saved = "grid";
        try { saved = localStorage.getItem("tudo_view") || "grid"; } catch (e) { }
        setView(saved);
    });

})();
