// ==========================================================================
// Tudo em Dia — Execução do checklist (apontamento)
// - calendário (datepicker) e relógio (clockpicker) nas datas/horas
// - itens por tipo (Sim/Não, Sim/Não/N.A., Numérico, Texto, Data, Hora)
// - "Não" sugere abrir O.S. e nova vistoria (ambos opcionais)
// - validação dos campos obrigatórios
// - foto por item (modal), quando o item permite
// ==========================================================================
(function ($) {
    "use strict";

    var $modal = null;

    $(function () {

        // máscaras
        if ($.fn.mask) {
            $("#data_inicio, #data_termino, .js-datepicker").mask("99/99/9999");
            $("#hora_inicio, #hora_termino, .js-hora").mask("99:99");
        }

        // calendário (tema Codebase) nos campos .js-datepicker
        if (window.Codebase && Codebase.helpers) {
            Codebase.helpers(['datepicker', 'select2']);
        }

        // relógio para horas
        if ($.fn.clockpicker) {
            $('.hora_inicio, .hora_termino, .hora_item').clockpicker();
        }

        // "Não" sugere reinspeção + abertura de O.S. (o usuário pode desmarcar)
        $("#formApontamento").on("change", ".chk-radio", function () {
            var $row = $(this).closest(".chk-item");
            var isNao = this.checked && this.value === "NÃO";
            $row.toggleClass("is-nao", isNao);
            if (isNao) {
                $row.find(".chk-nova input[type='checkbox']").prop("checked", true);
                $row.find(".chk-os input[type='checkbox']").prop("checked", true);
            }
        });

        // validação no envio
        $("#formApontamento").on("submit", function (e) {
            if (!validarApontamento(this)) {
                e.preventDefault();
            }
        });

        // ---------- FOTOS ----------
        $modal = $("#modalFoto");

        $("#btnSalvarFoto").on("click", function () {
            enviarFotos();
        });

        // exclusão de foto (delegado)
        $("#fotoGaleria").on("click", ".pcm-thumb-del", function () {
            excluirFoto($(this).data("codigo"));
        });
    });

    // ---------------------------------------------------------------
    // Validação
    // ---------------------------------------------------------------
    function validarApontamento(form) {

        var msgs = [];

        if (!$.trim($("#funcionario_responsavel", form).val())) {
            msgs.push("• Informe o responsável.");
        }
        if (!validaData($("#data_inicio").val())) { msgs.push("• Data de início inválida."); }
        if (!validaData($("#data_termino").val())) { msgs.push("• Data de término inválida."); }
        if (!validaHora($("#hora_inicio").val())) { msgs.push("• Hora de início inválida."); }
        if (!validaHora($("#hora_termino").val())) { msgs.push("• Hora de término inválida."); }

        var itensPendentes = 0;
        var numeroInvalido = 0;

        $(form).find(".chk-item").each(function () {

            var $row = $(this);
            var tipo = parseInt($row.find("input[name$='.codigo_tipo_item_checklist']").val() || "8", 10);

            if (tipo === 1 || tipo === 8) {
                if ($row.find(".chk-radio:checked").length === 0) itensPendentes++;
            } else {
                var $resp = $row.find("input[name$='.resposta']");
                var val = $.trim($resp.val() || "");
                if (val === "") {
                    itensPendentes++;
                } else if (tipo === 2) {
                    var num = parseFloat(val.replace(",", "."));
                    var min = parseFloat($resp.data("min"));
                    var max = parseFloat($resp.data("max"));
                    if (isNaN(num)) numeroInvalido++;
                    else if (!isNaN(min) && !isNaN(max) && (min !== 0 || max !== 0) && (num < min || num > max)) numeroInvalido++;
                }
            }
        });

        if (itensPendentes > 0) msgs.push("• Responda todos os itens do checklist (" + itensPendentes + " pendente(s)).");
        if (numeroInvalido > 0) msgs.push("• Existem valores numéricos fora do intervalo permitido.");

        if (msgs.length > 0) {
            var texto = msgs.join("\n");
            if (window.Swal) {
                Swal.fire({ title: "Atenção", html: texto.replace(/\n/g, "<br>"), icon: "warning" });
            } else {
                alert(texto);
            }
            return false;
        }
        return true;
    }

    function validaData(v) {
        return /^([0-2][0-9]|3[01])\/(0[1-9]|1[0-2])\/\d{4}$/.test($.trim(v || ""));
    }
    function validaHora(v) {
        return /^([01][0-9]|2[0-3]):[0-5][0-9]$/.test($.trim(v || ""));
    }

    // ---------------------------------------------------------------
    // Fotos
    // ---------------------------------------------------------------
    window.abrirFoto = function (codigoItem) {
        $("#foto_item").val(codigoItem);
        $("#fotoGaleria").html("<i class='fa fa-spinner fa-spin'></i>");
        if ($("#fotoInput").length) $("#fotoInput").val("");
        carregarFotos();
        $("#modalFoto").modal("show");
    };

    function fotoParams() {
        return {
            codigo_apontamento: $("#codigo_apontamento").val(),
            codigo_unidade: $("#codigo_unidade").val(),
            codigo_item_checklist: $("#foto_item").val()
        };
    }

    function carregarFotos() {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: $modal.data("url-load"),
            type: "POST",
            data: $.extend({}, fotoParams(), { __RequestVerificationToken: token }),
            success: function (list) { renderFotos(list || []); },
            error: function () { $("#fotoGaleria").html("<div class='text-danger'>Erro ao carregar fotos.</div>"); }
        });
    }

    function renderFotos(list) {
        var readonly = $modal.data("readonly") === 1 || $modal.data("readonly") === "1";
        if (!list.length) {
            $("#fotoGaleria").html("<div class='text-muted'>Sem fotos.</div>");
            marcarBotaoFoto(false);
            return;
        }
        var html = "";
        list.forEach(function (f) {
            html += "<div class='pcm-thumb'>" +
                        "<img src='" + f.filename + "' alt='foto'>" +
                        (readonly ? "" : "<button type='button' class='pcm-thumb-del' data-codigo='" + f.codigo + "'><i class='fa fa-times'></i></button>") +
                    "</div>";
        });
        $("#fotoGaleria").html(html);
        marcarBotaoFoto(true);
    }

    function marcarBotaoFoto(tem) {
        $(".chk-foto-btn[data-item='" + $("#foto_item").val() + "']").toggleClass("has-foto", tem);
    }

    function enviarFotos() {
        var input = document.getElementById("fotoInput");
        if (!input || input.files.length === 0) {
            if (window.Swal) { Swal.fire({ title: "Atenção", text: "Selecione ao menos uma foto.", icon: "warning" }); }
            return;
        }
        var formdata = new FormData();
        for (var i = 0; i < input.files.length; i++) {
            formdata.append(input.files[i].name, input.files[i]);
        }
        var p = fotoParams();
        var url = $modal.data("url-upload") + "?codigo_apontamento=" + p.codigo_apontamento +
                  "&codigo_unidade=" + p.codigo_unidade + "&codigo_item_checklist=" + p.codigo_item_checklist;
        var token = $('input[name="__RequestVerificationToken"]').val();

        var xhr = new XMLHttpRequest();
        xhr.open("POST", url);
        xhr.setRequestHeader("RequestVerificationToken", token);
        xhr.onload = function () {
            input.value = "";
            carregarFotos();
        };
        xhr.send(formdata);
    }

    function excluirFoto(codigo) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: $modal.data("url-delete"),
            type: "POST",
            data: $.extend({}, fotoParams(), { codigo: codigo, __RequestVerificationToken: token }),
            success: function () { carregarFotos(); }
        });
    }

})(jQuery);
