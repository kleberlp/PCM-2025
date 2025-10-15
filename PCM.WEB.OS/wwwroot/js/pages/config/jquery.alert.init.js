

$(document).ready(function () {

    var table;

    $(document).ready(function () {

        table = $('#tableMain').DataTable({
            searching: true,
            lengthChange: false,
            pageLength: 15,
            processing: false,
            serverSide: false,
            ajax: {
                url: "loadAlert",
                type: "POST",
                datatype: "json",
                data: function (d) {
                    d.branch = $('#branch').val(),
                    d.description = $('#description').val()
                },
                dataSrc: ""
            },
            columns: [
                { data: "description" },
                { data: "active", orderable: false },
                {
                    orderable: false,
                    data: null,
                    defaultContent: "<div class='btn-group'><button type='button' class='btn btn-sm btn-outline-secondary waves-light waves-effect dropdown-toggle' id='btnDelete' data-bstoggle='dropdown' aria-expanded='false'><i class='fas fa-trash-alt'></i></button><button id='btnEdit' name='btnEdit' type='button' class='btn btn-sm btn-outline-secondary waves-light waves-effect dropdown-toggle' data-bstoggle='dropdown' aria-expanded='false'><i class='fas fa-pencil-alt'></i></button></div> "
                }
            ],
            dom: 'Bfrtip',
            //buttons: ['copy', 'excel', 'pdf', 'colvis'],
            language: {
                "emptyTable": "@Html.Raw(Resources.emptyTable)",
                "info": "",
                "infoEmpty": "",
                "infoFiltered": "",
            },
            order: [[0, 'asc']],
            columnDefs: [
                { className: 'text-center', targets: [1, 2] },
                { width: '100px', targets: [1] },
                { width: '40px', targets: [2] },
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

    });

    function editRegister(data) {
        window.location = 'profileEdit?profileId=' + data.profileId;
    }

    function deleteRegister(data) {

        swal.fire({
            title: "@Html.Raw(Resources.msgQuestionDelete)",
            text: "@Html.Raw(Resources.msgNotPossibleReverse)",
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: '@Html.Raw(Resources.yes)',
            cancelButtonText: '@Html.Raw(Resources.no)',
            reverseButtons: true
        }).then(function (result) {
            if (result.value) {
                jQuery.ajax({
                    method: "POST",
                    url: "deleteProfile",
                    async: false,
                    data: {
                        "username": data.username
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
    
} );
