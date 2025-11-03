
$(function () {

    $.ajaxSetup({ async: false });
    Codebase.helpers(['datepicker', 'maxlength', 'select2']);

    var unidade = $("#unidade");
    var tipoGovernanca = $("#tipoGovernanca");
    var data = $("#data");
    var camareira = $("#camareira");
    var bloco = $("#bloco");
    var andar = $("#andar");

    unidade.change(function () {

        if (unidade.val() == "") {
            bloco.empty();
            andar.empty();
            camareira.empty();
            frontOfficeStatus.empty();
            roomStatus.empty();
        }
        else {
            var request = { "unidade": (unidade.val() == "") ? "-1" : unidade.val() };

            $.post("LoadComboCamareira", request, function (obj) {
                camareira.empty();
                option = $("<option>", { "value": "" }).text("");
                camareira.append(option);
                $.each(obj, function (indice, result) {
                    option = $("<option>", { "value": result.codigo }).text(result.descricao);
                    camareira.append(option);
                });
            }, "json");

            $.post("LoadComboBloco", request, function (obj) {
                bloco.empty();
                option = $("<option>", { "value": "" }).text("");
                bloco.append(option);
                $.each(obj, function (indice, result) {
                    option = $("<option>", { "value": result.codigo }).text(result.descricao);
                    bloco.append(option);
                });
            }, "json");

            $.post("LoadComboAndar", request, function (obj) {
                andar.empty();
                option = $("<option>", { "value": "" }).text("");
                andar.append(option);
                $.each(obj, function (indice, result) {
                    option = $("<option>", { "value": result.codigo }).text(result.descricao);
                    andar.append(option);
                });
            }, "json");

        }

    });

    loadPlanejamento();

    // Reload DataTable
    $('#filtrar').click(function () {
        table.ajax.reload(null, false);
    });

    table.columns.adjust().draw();

});

function loadPlanejamento() {

    // Init simple DataTable, for more examples you can check out https://www.datatables.net/
    table = $("#tb_main").DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        searching: true,
        fixedColumns: {
            start: 0,
            end: 1
        },
        order: [[1, 'desc']],
        info: false,
        scrollCollapse: false,
        scrollY: '50vh',
        paging: false,
        fixedHeader: true,
        destroy: true,
        paging: false,
        lengthChange: false,
        autoWidth: false,
        ajax: {
            url: "LoadPlanejamentoHistorico",
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.unidade = $("#unidade").val() == "" ? "-1" : $("#unidade").val(),
                d.tipoGovernanca = $("#tipoGovernanca").val() == "" ? "-1" : $("#tipoGovernanca").val(),
                d.dataInicio = $("#dataInicio").val(),
                d.dataTermino = $("#dataTermino").val(),
                d.camareira = $("#camareira").val() == "" ? "-1" : $("#camareira").val(),
                d.bloco = $("#bloco").val(),
                d.andar = $("#andar").val(),
                d.apartamento = $("#apartamento").val()
            },
            dataSrc: ""
        },
        dom: 'Bfrtip',
        buttons: [
            { extend: 'copy', text: 'Copiar' },
            {
                extend: 'print',
                text: 'Imprimir'
            },
            { extend: 'excel' },
            { extend: 'pdf', orientation: 'portrait' },
        ],
        columns: [
            { data: "data" },
            { data: "tipoGovernanca" },
            { data: "apartamento" },
            { data: "tipoApartamento" },
            { data: "bloco" },
            { data: "andar" },
            { data: "quantidadeCama" },
            { data: "camareira" },
            { data: "executado" },
            { data: "quantidadeNC" },
            { data: "vistoriado" }
        ],
        columnDefs: [
            { className: 'text-center', targets: [0, 1, 2, 3, 4, 5, 6, 8, 9, 10] },
            {
                createdCell: function (td, cellData, rowData, row, col) {

                    if (col == 0 && rowData.executado != "SIM") {
                        $(td).addClass("bg-warning-light");
                    } else {
                        $(td).addClass("bg-success-light");
                    }
                }, targets: [0]
            },
        ],
        language: {
            emptyTable: "Nenhum registro encontrado",
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
    });

}

// Add "Select All" checkbox in the header
var table = $('#tb_main').DataTable();
var selectAllCheckbox = $("<input type='checkbox' class='css-control-sm css-control-success css-checkbox py-0 px-0' value='-1'>").on('change', function () {
    var checkboxes = table.column(0).nodes().to$().find('input[type="checkbox"]');
    checkboxes.prop('checked', $(this).is(':checked'));
});
