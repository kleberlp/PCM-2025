
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);
const basePath = window.location.pathname.split("/").slice(0, 2).join("/");

$(document).ready(function () {

    $('.js-datepicker').datepicker({
        format: 'dd/mm/yyyy',
        weekStart: 1,
        autoclose: true,
        todayHighlight: true
    });

    function format(data) {

        var info = '';

        $.ajax({
            type: "POST",
            url: "LoadGovernancaInventarioEnxovalDetalhe",
            async: false,
            data: {
                "codigoInventarioGovernanca": data.codigo
            },
            dataType: "json",
            success: function (response) {

                if (response.length) {

                    info = info + '<table class="table table-bordered table-vcenter table-striped table-sm js-dataTable-full" id="tbDetails"> ';
                    info = info + '<thead> ';
                    info = info + '<tr class="table-secondary"> ';
                    info = info + '<th class="font-size-sm">' + messages.enxoval + '</th> ';
                    info = info + '<th class="text-center font-size-sm">' + messages.quantidadeRouparia + '</th> ';
                    info = info + '<th class="text-center font-size-sm">' + messages.quantidadeUso + '</th> ';
                    info = info + '<th class="text-center font-size-sm">' + messages.quantidadeLavanderia + '</th> ';
                    info = info + '<th class="text-center font-size-sm">' + messages.quantidade + '</th> ';
                    info = info + '<th class="text-center font-size-sm">' + messages.divergencia + '</th> ';
                    info = info + '</tr> ';
                    info = info + '</thead> ';
                    info = info + '<tbody>';

                    for (var i = 0; i < response.length; i++) {

                        info = info + '<tr class="bg-white"> ';
                        info = info + '<td class="font-size-sm">' + response[i].enxoval + '</td> ';
                        info = info + '<td class="text-center font-size-sm" style="width:100px">' + response[i].quantidadeInventario + '</td> ';
                        info = info + '<td class="text-center font-size-sm" style="width:100px">' + response[i].quantidadeUso + '</td> ';
                        info = info + '<td class="text-center font-size-sm" style="width:100px">' + response[i].quantidadeLavanderia + '</td> ';
                        info = info + '<td class="text-center font-size-sm" style="width:100px">' + response[i].quantidadeContabil + '</td> ';
                        if (response[i].divergencia != 0) {
                            info = info + '<td class="text-center bg-danger text-white font-size-sm" style="width:100px">' + response[i].divergencia + '</td> ';
                        } else {
                            info = info + '<td class="text-center font-size-sm" style="width:100px">' + response[i].divergencia + '</td> ';
                        }
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
            url: `${basePath}/LoadInventarioEnxoval`,
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.empresa = $('#empresa').val(),
                d.unidade = ($('#unidade').val() == "") ? -1 : $('#unidade').val(),
                d.dataInicio = $('#dataInicio').val(),
                d.dataTermino = $('#dataTermino').val(),
                d.status = ($('#status').val() == "") ? -1 : $('#status').val()
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
                columns: [1, 2, 3, 4]
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
            { data: "unidade" },
            { data: "data" },
            { data: "contador" },
            { data: "acuracidade" },
            { data: "statusDescricao" },
            {
                orderable: false,
                data: null,
                defaultContent: "<div class='btn-group'> " +
                    "<button type='button' class='btn btn-sm btn-secondary bg-success' title='" + messages.clickToApproval + "' id='btnApproval' name='btnApproval'><i class='fa fa-check'></i></button> " +
                    "<button type='button' class='btn btn-sm btn-secondary bg-danger' title='" + messages.clickToReproval + "' id='btnReproval' name='btnReproval'><i class='fa fa-times'></i></button> " +
                    "</div> "
            }
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        order: [[1, 'asc']],        
        columnDefs: [
            { className: 'text-center', targets: [0, 2, 4, 5, 6] },
            { width: '40px', targets: [0, 6] },
            { width: '150px', targets: [2, 4] },
            {
                createdCell: function (td, cellData, rowData, row, col) {

                    if (col == 6 && rowData.status != '1') {
                        $(td).find('#btnApproval').hide();
                        $(td).find('#btnReproval').hide();
                    }

                }, targets: [6]
            }
        ],

    });

    // Reload DataTable
    $('#btnSearch').click(function () {
        table.ajax.reload(null, false);
    });

    $('#tableMain tbody').on('click', 'button', function () {

        var data = table.row($(this).closest('tr')).data();

        if (this.id == "btnApproval") {
            approvalRegister(data, 2);
        }
        else if (this.id == "btnReproval") {
            approvalRegister(data, 9);
        }

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


    function editRegister(data) {
        window.location = `${basePath}/inventarioEnxovalEdit?codigo=` + data.codigo;
    }

    function approvalRegister(data, status) {

        swal.fire({
            title: (status == 2) ? messages.msgQuestionApprovalInventory : messages.msgQuestionReprovalInventory,
            text: messages.msgNotPossibleReverse,
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: messages.yes,
            cancelButtonText: messages.no,
            reverseButtons: true
        }).then(function (result) {
            if (result.value) {
                jQuery.ajax({
                    method: "POST",
                    url: `${basePath}/ChangeStatusInventarioEnxoval`,
                    async: false,
                    data: {
                        "codigoInventarioGovernanca": data.codigo,
                        "status": status
                    },
                    dataType: "json",
                    success: function (response) {

                        if (response.success) {
                            Swal.fire({
                                title: response.message,
                                icon: "success",
                                showDenyButton: false,
                                showCancelButton: false,
                            }).then((result) => {
                                if (result) {
                                    table.ajax.reload(null, false);
                                }
                            });

                        } else {
                            Swal.fire({
                                title: response.message,
                                icon: "error",
                                showDenyButton: false,
                                showCancelButton: false,
                            });
                        }
                    }
                });
            }
        });
    }

});
