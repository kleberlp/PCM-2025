
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);
const basePath = window.location.pathname.split("/").slice(0, 2).join("/");

$(document).ready(function () {

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
            { data: "status" },
            {
                orderable: false,
                data: null,
                defaultContent: "<div class='btn-group'> " +
                    "<button type='button' class='btn btn-sm btn-secondary' title='" + messages.clickToApproval + "' id='btnApproval' name='btnApproval'><i class='fa fa-times'></i></button> " +
                    "<button type='button' class='btn btn-sm btn-secondary' title='" + messages.clickToRepproval + "' id='btnRepproval' name='btnRepproval'><i class='fa fa-times'></i></button> " +
                    "<button type='button' class='btn btn-sm btn-secondary' title='" + messages.clickToEdit + "' id='btnEdit' name='btnEdit'><i class='fa fa-pencil'></i></button></div> "
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
            { width: '150px', targets: [2, 4] }
        ],
    });

    // Reload DataTable
    $('#btnSearch').click(function () {
        table.ajax.reload(null, false);
    });

    $('#tableMain tbody').on('click', 'button', function () {

        var data = table.row($(this).closest('tr')).data();

        if (this.id == "btnApproval") {
            approvalRegister(data, 1);
        }
        else if (this.id == "btnRepproval") {
            approvalRegister(data, 2);
        }
        else if (this.id == "btnEdit") {
            editRegister(data);
        }

    });

    function editRegister(data) {
        window.location = `${basePath}/inventarioEnxovalEdit?codigo=` + data.codigo;
    }

    function approvalRegister(data, status) {

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
                    url: `${basePath}/changeStatusInventarioEnxoval`,
                    async: false,
                    data: {
                        "codigo": data.codigo,
                        "status": data.status
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
