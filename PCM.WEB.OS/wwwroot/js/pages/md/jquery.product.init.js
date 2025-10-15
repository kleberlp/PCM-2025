const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

$(document).ready(function () {

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
        autoWidth: true,
        pageLength: 15,
        processing: true,
        scrollX: true,
        scrollCollapse: true,
        ajax: {
            url: "loadProduct",
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.sku = $('#sku').val(),
                d.description = $('#description').val(),
                d.familyId = $('#familyId').val() == "" ? -1 : $('#familyId').val(),
                d.uomId = $('#uomId').val() == "" ? -1 : $('#uomId').val(),
                d.locationTypeId = $('#locationTypeId').val() == "" ? -1 : $('#locationTypeId').val(),
                d.classificationId = $('#classificationId').val() == "" ? -1 : $('#classificationId').val()
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
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
            }
        ],
        columns: [
            { data: "sku" },
            { data: "description" },
            { data: "ean" },
            { data: "family" },
            { data: "uom" },
            { data: "lensPacking" },
            { data: "locationType" },
            { data: "addressWh" },
            { data: "classification" },
            { data: "active" },
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
            { className: 'text-center', targets: [0, 2, 4, 5, 6, 7, 8, 9, 10] },
            { width: '150px', targets: [0, 2] },
            { width: '120px', targets: [4, 5, 6, 7, 8, 9] },
            { width: '20px', targets: [10] },
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

    function editRegister(data) {
        window.location = 'productEdit?productId=' + data.productId;
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
                    url: "deleteProduct",
                    async: false,
                    data: {
                        "productId": data.productId
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
