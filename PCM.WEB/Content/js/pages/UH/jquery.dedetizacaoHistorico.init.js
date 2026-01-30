
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

    $("#unidade").change(function () {

        if ($("#unidade").val() == "") {

            $("#apartamento").empty();

        } else {

            var request = { "unidade": $("#unidade").val() };

            $.post("LoadApartamento", request, function (obj) {
                $("#apartamento").empty();
                option = $("<option>", { "value": "" }).text("");
                $("#apartamento").append(option);
                $.each(obj, function (indice, result) {
                    option = $("<option>", { "value": result.codigo }).text(result.descricao);
                    $("#apartamento").append(option);
                });
            }, "json");

        }

    });

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
            url: `${basePath}/LoadDedetizacaoHistorico`,
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.empresa = $('#empresa').val(),
                d.unidade = ($('#unidade').val() == "") ? -1 : $('#unidade').val(),
                d.dataInicio = $('#dataInicio').val(),
                d.dataTermino = $('#dataTermino').val(),
                d.apartamento = ($('#apartamento').val() == "") ? -1 : $('#apartamento').val()
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
            { data: "unidade" },
            { data: "data" },
            { data: "apartamento" },
            { data: "colaborador" },
            { data: "observacao" },
            {
                orderable: false,
                data: null,
                defaultContent: "<div class='btn-group'> " +
                    "<button type='button' class='btn btn-sm btn-secondary' title='" + messages.clickToDelete + "' id='btnDelete' name='btnDelete'><i class='fa fa-times'></i></button> " +
                    "</div> "
            }
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        order: [[0, 'asc']],        
        columnDefs: [
            { className: 'text-center', targets: [1, 2, 5] },
            { width: '40px', targets: [5] },
            { width: '150px', targets: [1, 2] },
            {
                createdCell: function (td, cellData, rowData, row, col) {

                    if (col == 5 && messages.diretoExcluir == false) {
                        $(td).find('#btnDelete').hide();
                    }

                }, targets: [5]
            }
        ],

    });

    // Reload DataTable
    $('#btnSearch').click(function () {
        table.ajax.reload(null, false);
    });

    $('#tableMain tbody').on('click', 'button', function () {

        var data = table.row($(this).closest('tr')).data();

        if (this.id == "btnDelete") {
            deleteRegister(data);
        }

    });

    function deleteRegister(data) {

        swal.fire({
            title: messages.msgQuestionDelete,
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
                    url: `${basePath}/DeleteDedetizacao`,
                    async: false,
                    data: {
                        "codigoEmpresa": data.codigoEmpresa,
                        "codigoUnidade": data.codigoUnidade,
                        "codigoUHAtividade": data.codigoUHAtividade,
                        "codigo": data.codigo,
                        "usuario": $("#usuario").val()
                    },
                    dataType: "json",
                    success: function (response) {

                        if (response.success) {
                            Swal.fire({
                                title: response.message,
                                icon: "success",
                                showDenyButton: false,
                                showCancelButton: false,
                            })
                            table.ajax.reload(null, false);

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
