// ==========================================================================
// Tudo em Dia — Execução do checklist (apontamento)
// - "Não" marca automaticamente a reinspeção da linha
// - máscara de data
// - exige todos os itens respondidos antes de concluir
// ==========================================================================
(function ($) {
    "use strict";

    $(function () {

        // máscara de data (plugin maskedinput)
        if ($.fn.mask) {
            $(".js-datepicker").mask("99/99/9999");
        }

        // ao escolher "Não", marca a reinspeção da própria linha
        $("#formApontamento").on("change", ".chk-radio", function () {
            var $row = $(this).closest(".chk-item");
            if (this.checked && this.value === "NÃO") {
                $row.find('input[type="checkbox"]').prop("checked", true);
            }
        });

        // validação: todos os itens respondidos
        $("#formApontamento").on("submit", function (e) {
            var pendente = false;
            $(this).find(".chk-item").each(function () {
                if ($(this).find(".chk-radio:checked").length === 0) {
                    pendente = true;
                }
            });
            if (pendente) {
                e.preventDefault();
                if (window.swal) {
                    swal({ title: "Atenção", text: "Responda todos os itens do checklist antes de concluir.", type: "warning" });
                } else {
                    alert("Responda todos os itens do checklist antes de concluir.");
                }
            }
        });

    });

})(jQuery);
