
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);
var table;

$(document).ready(function () {

    // Basic
    $('input[name="planningDate"]').daterangepicker({
        "alwaysShowCalendars": true,
    });
    $('.open_picker').show();


    // Select2
    $(".select2").select2({
        width: '100%'
    });

    $('#modActionOccurrence').on('shown.bs.modal', function () {
        $('#tableAction').DataTable().columns.adjust().draw();
    });

    function format(data) {

        var table_info = '';

        var dataSerialized = JSON.stringify(data).replace(/"/g, '&quot;');

        jQuery.ajax({
            type: "POST",
            url: "loadGroupPlanningDelivery",
            async: false,
            data: {
                "planningId": data.planningId
            },
            dataType: "json",
            success: function (response) {

                table_info = '<div class="bg-light"><table class="table table-striped table-bordered dt-responsive nowrap bg-white"  style="border-collapse: collapse; border-spacing: 0; width: 100%;"> ' +
                    '<thead> ' +
                    '<tr class="bg-light" rowSpan="2">' +
                    '<th class="text-center">' + messages.deliveryNumber + '</th>' +
                    '<th class="text-center">' + messages.deliveryCreateDate + '</th>' +
                    '<th class="text-center">' + messages.deliveryDate + '</th>' +
                    '<th>' + messages.shipToParty + '</th>' +
                    '<th>' + messages.soldToParty + '</th>' +
                    '<th class="text-center" style:"width:20px"></th>' +
                    '</tr>' +
                    '</thead> ' +
                    '<tbody>';

                for (var i = 0; i < response.length; i++) {

                    table_info += '<tr class="bg-white"> ';
                    table_info += '<td class="text-center">' + response[i].deliveryNumber + '</td>';
                    table_info += '<td class="text-center">' + response[i].deliveryCreateDate + '</td>';
                    table_info += '<td class="text-center">' + response[i].deliveryDate + '</td>';
                    table_info += '<td>' + response[i].shipToParty + '</td>';
                    table_info += '<td>' + response[i].soldToParty + '</td>';
                    table_info += '<td style="width:20px" class="text-center"><div class="btn-group">' +
                        '<button type="button" class="btn btn-sm btn-outline-secondary waves-light waves-effect dropdown-toggle tippy-btn" ' +
                        'title="' + messages.clickToDelete + '" data-tippy-arrow="true" data-tippy-arrowTransform="scale(0.75)" data-tippy-animation="fade" ' +
                        'onClick=\'deleteDelivery(' + JSON.stringify(response[i].planningId) + ',' + JSON.stringify(response[i].deliveryNumber) + ');\' ' +
                        'data-bstoggle="dropdown" aria-expanded="false">' +
                        '<i class="fas fa-trash-alt text-danger"></i></button></div></td>';
                    table_info += '</tr>';
                    
                }

                table_info += '</tbody> ' +
                    '</table></div>';

            }
        });

        return table_info;

    }

    table = $('#tableMain').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        searching: true,
        fixedColumns: {
            start: 1,
            end: 1
        },
        autoWidth: true,
        pageLength: 12,
        processing: true,
        scrollX: true,
        scrollCollapse: true,
        ajax: {
            url: "loadGroupPlanning",
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.branch = $('#branch').val(),
                d.groupCode = $('#groupCode').val(),
                d.startDate = $('#planningDateStart').val(),
                d.endDate = $('#planningDateEnd').val(),
                d.route = ($('#route').val() == "") ? -1 : $("#route").val(),
                d.status = ($('#status').val() == "") ? -1 : $("#status").val()
            },
            dataSrc: ""
        },
        dom: "<'row'<'col-sm-12 col-md-6'B><'col-sm-12 col-md-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        buttons: [
            {
                extend: 'copy',
                text: "<i class='fas fa-copy text-primary'></i>",
                titleAttr: messages.clickToCopy,
                className: 'btn btn-sm btn-outline-light waves-light waves-effect dropdown-toggle tippy-btn mb-2'
            },
            {
                extend: 'excel',
                text: "<i class='fas fa-file-excel text-success'></i>",
                titleAttr: messages.clickToExcel,
                className: 'btn btn-sm btn-outline-light waves-light waves-effect dropdown-toggle tippy-btn mb-2'
            },
            {
                extend: 'pdf',
                text: "<i class='fas fa-file-pdf text-danger'></i>",
                titleAttr: messages.clickToPdf,
                className: 'btn btn-sm btn-outline-light waves-light waves-effect dropdown-toggle tippy-btn mb-2'
            },
            {
                extend: 'colvis',
                text: "<i class='fas fa-columns text-black'></i>",
                titleAttr: messages.clickToConfig,
                className: 'btn btn-sm btn-outline-light waves-light waves-effect dropdown-toggle tippy-btn mb-2',
                columns: [1, 2, 3, 4, 5]
            }
        ],
        columns: [

            {
                class: "details-control",
                orderable: false,
                data: null,
                defaultContent: ""
            },
            { data: "groupCode" },
            { data: "shipment" },
            { data: "createDate" },
            { data: "route" },
            { data: "statusDescription" },
            {
                orderable: false,
                data: null,
                defaultContent: "<div class='btn-group'> " +
                    "<button type='button' class='btn btn-sm btn-outline-secondary waves-light waves-effect dropdown-toggle tippy-btn' title='" + messages.clickToConfirmPlanning + "' data-tippy-arrow='true' data-tippy-arrowTransform='scale(0.75)' data-tippy-animation='fade' id='btnConfirm' data-bstoggle='dropdown' aria-expanded='false'><i class='far fa-calendar-check'></i></button> " +
                    "<button type='button' class='btn btn-sm btn-outline-secondary waves-light waves-effect dropdown-toggle tippy-btn' title='" + messages.clickToEdit + "' data-tippy-arrow='true' data-tippy-arrowTransform='scale(0.75)' data-tippy-animation='fade' id='btnEdit' name='btnEdit' data-bstoggle='dropdown' aria-expanded='false'><i class='fas fa-pencil-alt'></i></button></div> "
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
            { className: 'text-center', targets: [0, 1, 2, 3, 6] },
            { width: '20px', targets: [0, 6] },
            { width: '150px', targets: [1, 2, 3] },
            {
                createdCell: function (td, cellData, rowData, row, col) {

                    if (col == 5) {
                        $(td).addClass(rowData.cssClass);
                    }

                    }, targets: [5]
            }
        ],
        createdRow: function (row, data, dataIndex) {
            if (data.status > 1) {
                $(row).find('#btnConfirm').hide();
                $(row).find('#btnEdit').hide();
            }
        }
    });

    $('#tableMain tbody').on('click', 'button', function () {

        var data = table.row($(this).closest('tr')).data();

        if (this.id == "btnEdit") {
            editGroup(data);
        } else if (this.id == "btnConfirm") {
            confirmPlanning(data);
        } 

    });

    $('#tableMain tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var tdi = tr.find("i.fa");
        var row = table.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
            tdi.first().removeClass('fa-minus');
            tdi.first().addClass('fa-plus');
        }
        else {
            // Open this row
            tr.addClass('show bg-white');
            tdi.first().removeClass('fa-plus');
            tdi.first().addClass('fa-minus');
            row.child(format(row.data())).show();
        }
    });

    table.on("user-select", function (e, dt, type, cell, originalEvent) {
        if ($(cell.node()).hasClass("details-control")) {
            e.preventDefault();
        }
    });

    $('#btnSearch').click(function () {
        table.ajax.reload(null, false);
    });

    $('#btnSave').click(function () {
        $("#formEdit").submit();
    });

});

function deleteDelivery(planningId, deliveryNumber) {

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
                url: "DeleteDelivery",
                async: false,
                data: {
                    "planningId": planningId,
                    "deliveryNumber": deliveryNumber
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

function confirmPlanning(data) {

    swal.fire({
        title: messages.msgQuestionConfirmPlanning,
        text: messages.groupCode + ': ' + data.groupCode,
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: messages.yes,
        cancelButtonText: messages.no,
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            jQuery.ajax({
                method: "POST",
                url: "confirmPlanning",
                async: false,
                data: {
                    "planningId": data.planningId
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

function editGroup(data) {

    $("#planningId").val(data.planningId);
    $("#groupCodeInfo").html(data.groupCode);
    loadComboReasonCode(data.branch);
    loadComboDelivery(data.branch);

    $("#modGroupEdit").modal("show");
}

function loadComboReasonCode(branch) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: 'loadComboReasonCode',
            method: 'GET',
            data: { branch: $("#branch").val()},
            success: function (data) {
                $("#reasonCode").empty();
                $("#reasonCode").append(new Option("", -1));
                data.forEach(function (result) {
                    $("#reasonCode").append(new Option(result.description, result.id));
                });
                resolve();
                $("#reasonCode").trigger('dataLoaded');
            },
            error: function (error) {
                reject(error); // Rejeita a Promise em caso de erro
                console.error("Erro ao carregar as cidades:", error);
            }
        });
    });
}

function loadComboDelivery(branch) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: 'loadComboDelivery',
            method: 'GET',
            data: { branch: $("#branch").val() },
            success: function (data) {
                $("#deliveryNumber").empty();
                $("#deliveryNumber").append(new Option("", -1));
                data.forEach(function (result) {
                    $("#deliveryNumber").append(new Option(result.description, result.id));
                });
                resolve();
                $("#deliveryNumber").trigger('dataLoaded');
            },
            error: function (error) {
                reject(error); // Rejeita a Promise em caso de erro
                console.error("Erro ao carregar as cidades:", error);
            }
        });
    });
}

$('#formEdit').validate({
    ignore: [],
    errorClass: 'invalid-feedback animated fadeInDown',
    errorElement: 'div',
    onkeyup: false,
    onclick: false,
    onfocusout: false,
    errorPlacement: function (error, e) {
        jQuery(e).parents('.accordion-body > div > div').append(error);
    },
    highlight: function (e) {
        jQuery(e).closest('.modal-body > div > div').removeClass('is-invalid').addClass('is-invalid');
    },
    success: function (e) {
        jQuery(e).closest('.accordion-body > div > div').removeClass('is-invalid');
        jQuery(e).remove();
    },
    rules: {
        'deliveryNumber': {
            required: true
        },
        'reasonCode': {
            required: true
        }
    },
    messages: {
        'deliveryNumber': {
            required: messages.requiredField
        },
        'reasonCode': {
            required: messages.requiredField
        }
    },
    submitHandler: function (form) {   

        $.ajax({
            method: "POST",
            url: "addDelivery",
            data: {
                "planningId": $("#planningId").val(),
                "deliveryNumber": $("#deliveryNumber").val(),
                "reasonCode": $("#reasonCode").val(),
            },
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: response.message,
                        icon: "success",
                        showDenyButton: false,
                        showCancelButton: false,
                    }).then(function (result) {
                        if (result) {
                            table.ajax.reload(null, false);
                            $("#modGroupEdit").modal("hide");
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
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    title: 'Error',
                    text: error,
                    icon: 'error'
                });
                $("#btnModSave").prop('disabled', false);
            }
        });
    },
    invalidHandler: function (e, validation) {
        $("#btnModSave").prop('disabled', false);
    }
});

