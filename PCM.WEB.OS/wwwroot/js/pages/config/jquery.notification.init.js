
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

$(document).ready(function () {

    function format(data) {

        var table_info = '';

        jQuery.ajax({
            type: "POST",
            url: "loadNotificationAlert",
            async: false,
            data: {
                "notificationId": data.notificationId
            },
            dataType: "json",
            success: function (response) {

                table_info = '<table class="table table-bordered nowrap display"><thead> ' +
                    '<tr class="bg-light">' +
                    '<th class="col-6">' + messages.alertType + '</th>' +
                    '<th class="text-center col-2">' + messages.deliveryType + '</th>' +
                    '<th class="text-center col-2">' + messages.recurring + '</th>' +
                    '<th class="text-center col-2">' + messages.frequency + '</th>' +
                    '</tr>' +
                    '</thead> ' +
                    '<tbody>';

                for (var i = 0; i < response.length; i++) {

                    table_info += '<tr class="bg-white"> ';
                    table_info += '<td>' + response[i].alertType + '</td>';
                    table_info += '<td class="text-center">' + response[i].deliveryType + '</td>';
                    table_info += '<td class="text-center">' + response[i].recurring + '</td>';
                    table_info += '<td class="text-center">' + response[i].frequency + '</td>';
                    table_info += '</tr>';

                }

                table_info += '</tbody> ' +
                    '</table>';

            }
        });

        return table_info;

    }

    var table = $('#tableMain').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        searching: true,
        fixedColumns: {
            start: 0,
            end: 1
        },
        lengthChange: false,
        pageLength: 15,
        processing: true,
        scrollX: true,
        scrollCollapse: true,
        serverSide: false,
        ajax: {
            url: "loadNotification",
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.branch = $('#branch').val()
                d.description = $('#description').val()
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
                columns: [0, 1]
            }
        ],
        columns: [
            {
                class: "details-control",
                orderable: false,
                data: null,
                defaultContent: ""
            },
            { data: "description" },
            { data: "active", orderable: false },
            {
                orderable: false,
                data: null,
                defaultContent: "<div class='btn-group'> " +
                    "<button type='button' class='btn btn-sm btn-outline-secondary waves-light waves-effect dropdown-toggle tippy-btn' title='" + messages.clickToDelete + "' data-tippy-arrow='true' data-tippy-arrowTransform='scale(0.75)' data-tippy-animation='fade' id='btnDelete' data-bstoggle='dropdown' aria-expanded='false'><i class='fas fa-trash-alt'></i></button> " +
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
            { className: 'text-center', targets: [0, 2, 3] },
            { width: '100px', targets: [2] },
            { width: '40px', targets: [3] },
            { width: '20px', targets: [0] },
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
        } if (this.id == "btnEdit") {
            editRegister(data);
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
            // tr.find('svg').attr('data-icon', 'plus-square');    // FontAwesome 5
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
            tdi.first().removeClass('fa-plus-square');
            tdi.first().addClass('fa-minus-square');
            // tr.find('svg').attr('data-icon', 'minus-circle'); // FontAwesome 5
        }
    });

    table.on("user-select", function (e, dt, type, cell, originalEvent) {
        if ($(cell.node()).hasClass("details-control")) {
            e.preventDefault();
        }
    });

    function editRegister(data) {
        window.location = 'notificationEdit?notificationId=' + data.notificationId;
    }

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
                    url: "deleteNotification",
                    async: false,
                    data: {
                        "notificationId": data.notificationId
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
