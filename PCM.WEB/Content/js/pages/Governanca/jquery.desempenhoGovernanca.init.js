
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

var table = null;

$(document).ready(function () {

    table = $('#tbMain').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        searching: false,
        fixedColumns: {
            start: 0,
            end: 1
        },
        lengthChange: false,
        pageLength: 15,
        processing: true,
        scrollCollapse: false,
        serverSide: false,
        ajax: {
            url: messages.urlLoadDesempenhoGovernanca,
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.unidade = ($('#unidade').val() == "") ? -1 : $('#unidade').val(),
                    d.data = $('#data').val()
            },
            dataSrc: ""
        },
        dom: "<'row'<'col-sm-12 col-md-6'B><'col-sm-12 col-md-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        buttons: [
            {
                extend: 'copy',
                text: "<i class='fa fa-copy text-primary'></i>",
                titleAttr: messages.clickToCopy,
                className: 'btn btn-sm btn-secondary'
            },
            {
                extend: 'excel',
                text: "<i class='fa fa-file-excel-o text-success'></i>",
                titleAttr: messages.clickToExcel,
                className: 'btn btn-sm btn-secondary'
            },
            {
                extend: 'pdf',
                text: "<i class='fa fa-file-pdf-o text-danger'></i>",
                titleAttr: messages.clickToPdf,
                className: 'btn btn-sm btn-secondary'
            },
            {
                extend: 'colvis',
                text: "<i class='fa fa-columns text-black'></i>",
                titleAttr: messages.clickToConfig,
                className: 'btn btn-sm btn-outline-light waves-light waves-effect dropdown-toggle tippy-btn mb-2',
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
            }
        ],
        columns: [
            { data: "unidade" },
            { data: "quantidadeCamareiras" },
            { data: "quantidadeSupervisores" },
            { data: "uhsArrumadas" },
            { data: "uhsPermanencia" },
            { data: "uhsSaida" },
            { data: "uhsVistoriadas" },
            { data: "percentualVistoria" },
            { data: "quantidadeOSManutencao" },
            { data: "quantidadeNC" },
            { data: "quantidadeRetrabalho" },
            { data: "quantidadeAlteracaoStatus" }
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        order: [[0, 'asc']],
        columnDefs: [
            { className: 'text-center', targets: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11] },
            { width: '150px', targets: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11] },
        ],

    });

    $("#unidade").change(function () {
        table.ajax.reload(null, false);
    });

    $("#data").change(function () {
        table.ajax.reload(null, false);
    });

});
