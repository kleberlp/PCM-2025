
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);
const basePath = window.location.pathname.split("/").slice(0, 2).join("/");

$(document).ready(function () {


    function format(data) {

        var info = '';

        $.ajax({
            type: "POST",
            url: "LoadMovimentacaoEnxovalDetalhe",
            async: false,
            data: {
                "empresa": data.codigoEmpresa,
                "unidade": data.codigoUnidade,
                "dataInicio": $('#dataInicio').val(),
                "dataTermino": $('#dataTermino').val(),
                "enxoval": data.codigoEnxoval
            },
            dataType: "json",
            success: function (response) {

                if (response.length) {

                    info = info + '<table class="table table-bordered table-vcenter table-striped table-sm js-dataTable-full" id="tbDetails"> ';
                    info = info + '<thead> ';
                    info = info + '<tr class="table-secondary"> ';
                    info = info + '<th class="font-size-sm">' + messages.data + '</th> ';
                    info = info + '<th class="text-center font-size-sm">' + messages.tipoMovimentacao + '</th> ';
                    info = info + '<th class="text-center font-size-sm">' + messages.quantidade + '</th> ';
                    info = info + '<th class="text-center font-size-sm">' + messages.usuario + '</th> ';
                    info = info + '<th class="text-center font-size-sm">' + messages.local + '</th> ';
                    info = info + '<th class="text-center font-size-sm">' + messages.saldo + '</th> ';
                    info = info + '</tr> ';
                    info = info + '</thead> ';
                    info = info + '<tbody>';

                    for (var i = 0; i < response.length; i++) {

                        info = info + '<tr class="bg-white"> ';
                        info = info + '<td class="font-size-sm">' + response[i].data + '</td> ';
                        info = info + '<td class="text-center font-size-sm">' + response[i].tipoMovimentacao + '</td> ';
                        info = info + '<td class="text-center font-size-sm">' + response[i].quantidade + '</td> ';
                        info = info + '<td class="text-center font-size-sm">' + response[i].usuario + '</td> ';
                        info = info + '<td class="text-center font-size-sm">' + response[i].local + '</td> ';
                        info = info + '<td class="text-center font-size-sm">' + response[i].saldo + '</td> ';
                        info = info + '</tr> ';
                    }

                    info = info + '</tbody> ';

                    info = info + '</table> ';

                }

                return info;

            },
            error: function (data) {
                Codebase.layout("header_loader_off");
            }
        });

        return info;

    }

    var table = $('#tableMain').DataTable({
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
            url: `${basePath}/LoadMovimentacaoEnxoval`,
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.empresa = $('#empresa').val(),
                d.unidade = ($('#unidade').val() == "") ? -1 : $('#unidade').val(),
                d.dataInicio = $('#dataInicio').val(),
                d.dataTermino = $('#dataTermino').val(),
                d.enxoval = ($('#enxoval').val() == "") ? -1 : $('#enxoval').val()
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
                columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            }
        ],
        columns: [
            {
                class: "details-control",
                orderable: false,
                data: null,
                defaultContent: "",
                render: function () {
                    return '<i class="fa fa-plus-square" aria-hidden="true"></i>';
                },
                width: "15px"
            },
            { data: "enxoval" },
            { data: "quantidade" },
            { data: "quantidadeHotel" },
            { data: "saidaLavanderia" },
            { data: "entradaLavanderia" },
            { data: "saldoLavanderia" },
            { data: "perdasDeclaradas" },
            { data: "aquisicoes" },
            { data: "evasoes" },
            { data: "percentualEvasao" }            
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        order: [[1, 'asc']],        
        columnDefs: [
            { className: 'text-center', targets: [0, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { width: '40px', targets: [0] },
            { width: '100px', targets: [2, 3, 4, 5, 6, 7, 8, 9, 10] }
        ],

    });

    // Reload DataTable
    $('#btnSearch').click(function () {
        table.ajax.reload(null, false);
    });

    // Add event listener for opening and closing details
    $('#tableMain tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var tdi = tr.find("i.fa");
        var row = table.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
            tdi.first().removeClass('fa-minus-square');
            tdi.first().addClass('fa-plus-square');
            tr.removeClass('bg-warning-light font-italic font-b');
            // tr.find('svg').attr('data-icon', 'plus-square');    // FontAwesome 5
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
            tdi.first().removeClass('fa-plus-square');
            tdi.first().addClass('fa-minus-square');
            tr.addClass('bg-warning-light font-italic font-b');
            // tr.find('svg').attr('data-icon', 'minus-circle'); // FontAwesome 5
        }
    });

    table.on("user-select", function (e, dt, type, cell, originalEvent) {
        if ($(cell.node()).hasClass("details-control")) {
            e.preventDefault();
        }
    });

});
