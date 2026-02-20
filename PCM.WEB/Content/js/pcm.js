/*
 *  Document   : codebase.js
 *  Author     : pixelcave
 *  Description: Codebase - UI Framework Custom Functionality
 *
 */

'use strict';
function loadGridMain(table, data, endpoint, editAction = false, deleteAction = false, warningAction = false) {

    localStorage.clear();

    if (table) {
        table.destroy();
        table = null;
    }

    $.ajax({
        url: endpoint,
        type: "POST",
        data: data,
        success: function (response) {

            var data = response.data || [];
            var columnsResponse = response.columns || [];

            var groupDefsAll = (response.groupBy || [])
                .slice()
                .sort(function (a, b) { return (a.Level || 0) - (b.Level || 0); });

            // remove datatable antigo + DOM
            if ($.fn.DataTable.isDataTable('#tableDynamic')) {
                $('#tableDynamic').DataTable().destroy();
                $('#tableDynamic').empty();
            }

            // --- helpers ---
            function normalize(v) {
                if (v === null || v === undefined) return "";
                return v.toString().trim().replace(/\s+/g, " ").toUpperCase();
            }

            // níveis efetivos: só considera nível se existir algum valor preenchido no dataset
            var groupDefs = groupDefsAll.filter(function (g) {
                return data.some(function (row) {
                    return normalize(row[g.Column]) !== "";
                });
            });

            var groupColumns = groupDefs.map(function (g) { return g.Column; });

            // frozen
            var frozenCount = 0;

            // columns
            var dynamicColumns = columnsResponse.map(function (col) {
                if (col.Frozen) frozenCount++;

                var isGrouped = groupColumns.indexOf(col.Data) >= 0;

                return {
                    data: col.Data,
                    title: col.Title,
                    visible: isGrouped ? false : !!col.Visible, // grouped sempre invisível
                    width: col.Width || null,
                    orderable: col.Orderable !== false,
                    className: col.Align ? "text-" + col.Align : "",
                    defaultContent: ""
                };
            });

            if (editAction === true || deleteAction === true) {

                dynamicColumns.push({
                    orderable: false,
                    data: null,
                    width: "40px",
                    className: "text-center",
                    defaultContent:
                        "<div class='btn-group'>" +
                            (editAction === true) ? "<button type='button' class='btn btn-sm btn-secondary btn-edit' title='" + messages.clickToEdit + "'><i class='fa fa-pencil'></i></button>" : "" +
                                (deleteAction === true) ? "<button type='button' class='btn btn-sm btn-secondary btn-delete' title='" + messages.clickToDelete + "'><i class='fa fa-times'></i></button>" : "" +
                        "</div>"
                });

            }

            if (warningAction === true) {

                dynamicColumns.push({
                    orderable: false,
                    data: null,
                    width: "40px",
                    className: "text-center",
                    defaultContent:
                        "<div class='btn-group'>" +
                        "<button type='button' class='btn btn-sm btn-outline-secondary btn-warning-view' " +
                        "title='Clique para visualizar o alerta'>" +
                        "<i class='fa fa-warning text-warning'></i>" +
                        "</button>" +
                        "</div>"
                });

            }

            // estado por checklist
            currentStateKey = "dt_group_state:" + window.location.pathname + ":tipo=" + ($("#tipoChecklist").val() || "");

            function readState() {
                try { return JSON.parse(localStorage.getItem(currentStateKey) || "{}"); }
                catch (e) { return {}; }
            }

            function writeState(obj) {
                localStorage.setItem(currentStateKey, JSON.stringify(obj));
            }

            function buildKey(level, path) {
                return "L" + level + "|" + path.slice(0, level + 1).join("||");
            }

            function getPath(row) {
                // NÃO remove níveis (não usa filter)
                // Regra: se nível N estiver vazio, a hierarquia para ali.
                var path = groupColumns.map(function (col) {
                    return normalize(row[col]);
                });
                return path;
            }

            function countDirect(rowsData, level, parentPath) {

                // Último nível de agrupamento -> contar linhas (perguntas) dentro do grupo
                if (level >= groupColumns.length - 1) {
                    var count = 0;

                    for (var i = 0; i < rowsData.length; i++) {
                        var p = getPath(rowsData[i]);

                        // precisa bater o prefixo do parentPath
                        var ok = true;
                        for (var j = 0; j < parentPath.length; j++) {
                            if (p[j] !== parentPath[j]) { ok = false; break; }
                        }

                        if (ok) count++;
                    }

                    return count;
                }

                // Níveis intermediários -> contar filhos diretos distintos (próximo nível)
                var nextLevel = level + 1;
                var set = {};

                for (var i = 0; i < rowsData.length; i++) {
                    var p = getPath(rowsData[i]);

                    // prefixo
                    var ok = true;
                    for (var j = 0; j < parentPath.length; j++) {
                        if (p[j] !== parentPath[j]) { ok = false; break; }
                    }
                    if (!ok) continue;

                    // conta o valor do nível filho (se existir)
                    if (p.length > nextLevel) {
                        set[p[nextLevel]] = true;
                    }
                }

                return Object.keys(set).length;
            }


            function applyCollapse(api, animate) {
                var state = readState();

                api.rows({ page: 'current' }).every(function () {
                    var row = this.data();
                    var node = this.node();

                    var path = getPath(row);

                    var hidden = false;

                    for (var level = 0; level < path.length; level++) {
                        var key = buildKey(level, path.slice(0, level + 1));
                        if (state[key] === true) { hidden = true; break; }
                    }

                    if (hidden) {
                        if (animate) $(node).stop(true, true).fadeOut(120);
                        else node.style.display = "none";
                    } else {
                        if (animate) $(node).stop(true, true).fadeIn(120);
                        else node.style.display = "";
                    }
                });
            }

            function renderGroups(api) {

                $('#tableDynamic tbody tr.dt-group-row').remove();
                if (!groupColumns.length) return;

                var state = readState();
                var rowsData = api.rows({ page: 'current' }).data().toArray();
                var rowsNodes = api.rows({ page: 'current' }).nodes();

                var lastPath = [];

                for (var i = 0; i < rowsData.length; i++) {

                    var row = rowsData[i];
                    var node = rowsNodes[i];

                    var path = getPath(row);

                    for (var level = 0; level < path.length; level++) {

                        var currentValue = path[level];

                        // compara nível por nível
                        if (lastPath[level] !== currentValue) {

                            // reset níveis abaixo (essencial)
                            lastPath = lastPath.slice(0, level);
                            lastPath[level] = currentValue;

                            var def = groupDefs[level] || {};
                            var parentPath = path.slice(0, level + 1);
                            var key = buildKey(level, parentPath);

                            var collapsed = state[key] === true;

                            var icon = def.Collapsible
                                ? (collapsed ? "<i class='fa fa-chevron-right'></i>" : "<i class='fa fa-chevron-down'></i>")
                                : "<i class='fa fa-angle-right'></i>";

                            var indent = level * 18;

                            var countText = "";
                            if (def.ShowCount) {
                                var qtd = countDirect(rowsData, level, parentPath);
                                countText = " <span class='text-muted'>(" + qtd + ")</span>";
                            }

                            var css = def.CssClass || "";

                            var $tr = $("<tr/>")
                                .addClass("dt-group-row")
                                .addClass("level-" + level)
                                .attr("data-key", key)
                                .attr("data-level", level)
                                .attr("data-collapsible", def.Collapsible ? "1" : "0");

                            var $td = $("<td/>")
                                .attr("colspan", dynamicColumns.length)
                                .css("padding-left", (12 + indent) + "px")
                                .html(
                                    "<span class='dt-group-icon' style='display:inline-block;width:18px;'>" + icon + "</span>" +
                                    "<strong>" + currentValue + "</strong>" + countText
                                );

                            $tr.append($td);
                            $(node).before($tr);
                        }
                    }
                }

                // toggle collapse
                $('#tableDynamic tbody tr.dt-group-row').off('click').on('click', function () {

                    var collapsible = $(this).data("collapsible") == 1;
                    if (!collapsible) return;

                    var key = $(this).data("key");
                    var st = readState();
                    st[key] = !st[key];
                    writeState(st);

                    // atualiza ícone sem redesenhar tudo
                    var isCollapsed = st[key] === true;
                    $(this).find(".dt-group-icon").html(isCollapsed ? "<i class='fa fa-chevron-right'></i>" : "<i class='fa fa-chevron-down'></i>");

                    applyCollapse(api, true);
                });
            }

            // DataTable config
            currentConfig = {
                data: data,
                columns: dynamicColumns,
                searching: false,
                paging: false,
                processing: false,
                ordering: false,            // importante: DOM previsível
                autoWidth: false,
                scrollX: frozenCount > 0,   // se tiver frozen, precisa scrollX
                deferRender: true,
                fixedColumns: frozenCount > 0 ? { leftColumns: frozenCount } : false,
                drawCallback: function () {
                    var api = this.api();
                    renderGroups(api);         // SEM bloqueio _groupRendered
                    applyCollapse(api, false); // idempotente

                    if (warningAction === true) {

                        api.rows().every(function () {

                            var rowData = this.data();
                            var node = this.node();
                            var $btn = $(node).find('.btn-warning-view');

                            var hasError =
                                rowData.errorData &&
                                Array.isArray(rowData.errorData) &&
                                rowData.errorData.length > 0;

                            if (!hasError) {
                                $btn.hide();
                            } else {
                                $(node).addClass("text-danger");
                            }

                        });
                    }
                }
            };

            table = $('#tableDynamic').DataTable(currentConfig);

            $('#tableDynamic tbody')
                .off('click', '.btn-delete')
                .on('click', '.btn-delete', function () {
                    var rowData = table.row($(this).closest('tr')).data();
                    console.log("Excluir:", rowData);
                });

            $('#tableDynamic tbody')
                .off('click', '.btn-edit')
                .on('click', '.btn-edit', function () {
                    var rowData = table.row($(this).closest('tr')).data();
                    console.log("Editar:", rowData);
                });

            $('#tableDynamic tbody')
                .off('click', '.btn-warning-view')
                .on('click', '.btn-warning-view', function () {
                    var rowData = table.row($(this).closest('tr')).data();
                    showWarning(rowData);
                });
        }
    });
}

function showWarning(data) {

    if ($.fn.DataTable.isDataTable('#tableWarning')) {
        $('#tableWarning').DataTable().clear().destroy();
    }

    $('#tableWarning tbody').empty();

    $('#tableWarning').DataTable({
        lengthChange: false,
        paging: false,
        searching: false,
        scrollX: false,
        autoWidth: false,
        data: data.errorData || [],
        columns: [
            { data: "message", title: "Mensagem" }
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: ""
        }
    });

    $("#modWarningInfo").modal("show");
}

function gridHasErrors() {

    if (!$.fn.DataTable.isDataTable('#tableDynamic'))
        return false;

    var table = $('#tableDynamic').DataTable();
    var data = table.rows().data().toArray();

    for (var i = 0; i < data.length; i++) {

        if (data[i].errorData &&
            Array.isArray(data[i].errorData) &&
            data[i].errorData.length > 0) {

            return true;
        }
    }

    return false;
}

