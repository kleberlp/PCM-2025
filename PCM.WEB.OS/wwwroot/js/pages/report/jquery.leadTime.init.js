
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

$(document).ready(function () {

    var table = $('#tableMain').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        fixedColumns: {
            start: 0,
            end: 1
        },
        searching: true,
        lengthChange: false,
        pageLength: 15,
        processing: true,
        scrollX: true,
        scrollCollapse: true,
        serverSide: false,
        ajax: {
            url: "loadLeadTime",
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.branch = $('#branch').val(),
                d.carrier = ($('#carrier').val() == "") ? -1 : $("#carrier").val(),
                d.deliveryType = ($('#deliveryType').val() == "") ? -1 : $("#deliveryType").val(),
                d.origem = $('#origem').val(),
                d.destine = $('#origem').val()
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
                columns: [0, 1, 2, 3, 4, 5, 6, 7]
            }
        ],
        columns: [
            { data: "branch"},
            { data: "carrier" },
            { data: "origem"},
            { data: "destine" },
            { data: "deliveryType" },
            { data: "leadTimeMin"},
            { data: "leadTimeMax"},
            { data: "distanceKm"}
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        order: [[1, 'asc']],
        columnDefs: [
            { className: 'text-center', targets: [0, 4, 5, 6, 7] },
            { width: '150px', targets: [0, 5, 6, 7] }
        ],
    });

    // Reload DataTable
    $('#btnSearch').click(function () {
        table.ajax.reload(null, false);
    });

});
