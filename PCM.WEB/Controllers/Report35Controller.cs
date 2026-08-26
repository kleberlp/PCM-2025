using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PCM.WEB.MODELS;

namespace PCM.WEB.Controllers
{
    public class Report35Controller : Controller
    {
        // Connection string local/dev configurada em Web.config (DefaultConnection -> PCM_dev).
        private const string ConnectionStringName = "DefaultConnection";

        public async Task<ActionResult> Index(short codigoEmpresa, int codigoUnidade, long codigoOrdemServico)
        {
            var linhas = await CarregarLinhasAsync(codigoEmpresa, codigoUnidade, codigoOrdemServico);
            var apontamentos = await CarregarApontamentosAsync(codigoEmpresa, codigoUnidade, codigoOrdemServico);

            var primeira = linhas.FirstOrDefault();

            var model = new Report35ViewModel
            {
                CodigoEmpresa = codigoEmpresa,
                CodigoUnidade = codigoUnidade,
                CodigoOrdemServico = codigoOrdemServico,
                TipoOrdemServico = primeira?.TipoOrdemServico,
                Unidade = primeira?.Unidade,
                Data = primeira?.Data,
                Servico = primeira?.Programada,
                Equipamento = primeira?.Equipamento,
                Categoria = primeira?.Categoria,
                TipoServico = primeira?.TipoServico,
                Solucao = primeira?.DescricaoSolucao,
                // TODO: placeholder fixo para o mockup. No projeto final, resolver o logo do cliente
                // dinamicamente a partir de codigoEmpresa (essa convenção de armazenamento ainda não existe no projeto).
                ClienteLogoUrl = "~/Content/img/report35/logo-cliente-placeholder.png",
                EmitidoPor = Session["nome"] as string,
                Apontamentos = apontamentos,
                Grupos = linhas
                    .GroupBy(l => l.GrupoChecklist)
                    .Select(g => new Report35GrupoViewModel
                    {
                        Nome = g.Key,
                        SubGrupos = g
                            .GroupBy(l => l.SubGrupoChecklist)
                            .Select(sg => new Report35SubGrupoViewModel
                            {
                                Nome = sg.Key,
                                Itens = sg
                                    .OrderBy(l => l.Checklist)
                                    .Select(l => new Report35ItemViewModel
                                    {
                                        Codigo = l.CodigoItemChecklist,
                                        CodigoTipoItemChecklist = l.CodigoTipoItemChecklist,
                                        Checklist = l.Checklist,
                                        Descricao = l.Descricao,
                                        Resultado = l.Resultado,
                                        Observacao = l.Observacao,
                                        Peso = l.Peso
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToList()
            };

            // Fotos: uma consulta por item de checklist, filtrando pelo código real do item
            // (evita ter que casar por texto — sp_report_000000035_picture aceita @codigo_item_checklist).
            foreach (var item in model.TodosItens)
            {
                item.Fotos = await CarregarFotosAsync(codigoEmpresa, codigoUnidade, codigoOrdemServico, item.Codigo, model.TipoOrdemServico);
            }

            return View(model);
        }

        // GET: /Report35/Imagem
        // Serve o arquivo de foto anexada pelo app/API durante a execução do checklist.
        // Em produção o IIS roda na mesma máquina de C:\SIM\PCM\SITE\IMAGE\..., então o arquivo existe.
        // Em ambiente local de desenvolvimento (sem o compartilhamento de imagens) devolve um placeholder.
        public async Task<ActionResult> Imagem(short codigoEmpresa, int codigoUnidade, long codigoOrdemServico, int codigoItemChecklist, string tipo)
        {
            var fotos = await CarregarFotosAsync(codigoEmpresa, codigoUnidade, codigoOrdemServico, codigoItemChecklist, tipo);
            var caminho = fotos.FirstOrDefault()?.CaminhoArquivo;

            if (!string.IsNullOrWhiteSpace(caminho) && System.IO.File.Exists(caminho))
            {
                var extensao = Path.GetExtension(caminho).ToLowerInvariant();
                var contentType = extensao == ".jpg" || extensao == ".jpeg" ? "image/jpeg" : "image/png";
                return File(System.IO.File.ReadAllBytes(caminho), contentType);
            }

            return PlaceholderIndisponivel();
        }

        private FileContentResult PlaceholderIndisponivel()
        {
            const string svg = "<svg xmlns='http://www.w3.org/2000/svg' width='480' height='320'>" +
                "<rect width='100%' height='100%' fill='#eef0f3'/>" +
                "<text x='50%' y='50%' text-anchor='middle' fill='#8a8f98' font-family='Segoe UI, Arial' font-size='16'>Pré-visualização indisponível neste ambiente</text>" +
                "</svg>";
            return File(System.Text.Encoding.UTF8.GetBytes(svg), "image/svg+xml");
        }

        private static async Task<List<LinhaRelatorio>> CarregarLinhasAsync(short codigoEmpresa, int codigoUnidade, long codigoOrdemServico)
        {
            var linhas = new List<LinhaRelatorio>();
            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("sp_report_000000035", conn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 120 })
            {
                cmd.Parameters.Add("@codigo_empresa", SqlDbType.SmallInt).Value = codigoEmpresa;
                cmd.Parameters.Add("@codigo_unidade", SqlDbType.Int).Value = codigoUnidade;
                cmd.Parameters.Add("@codigo_pcm_programada_ordem_servico", SqlDbType.BigInt).Value = codigoOrdemServico;

                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        linhas.Add(new LinhaRelatorio
                        {
                            TipoOrdemServico = reader["tipo_ordem_servico"] as string,
                            Equipamento = reader["equipamento"] as string,
                            Programada = reader["programada"] as string,
                            Categoria = reader["categoria"] as string,
                            TipoServico = reader["tipo_servico"] as string,
                            DescricaoSolucao = reader["descricao_solucao"] as string,
                            Data = reader["data"] as string,
                            CodigoTipoItemChecklist = Convert.ToInt16(reader["codigo_tipo_item_checklist"]),
                            CodigoItemChecklist = Convert.ToInt32(reader["codigo_item_checklist"]),
                            Unidade = reader["unidade"] as string,
                            GrupoChecklist = reader["grupo_checklist"] as string,
                            SubGrupoChecklist = reader["sub_grupo_checklist"] as string,
                            Checklist = reader["codigo_checklist"]?.ToString(),
                            Descricao = reader["descricao"] as string,
                            Resultado = reader["resultado"] as string,
                            Observacao = reader["observacao"] as string,
                            Peso = reader["peso"] as decimal?
                        });
                    }
                }
            }

            return linhas;
        }

        private static async Task<List<Report35ApontamentoViewModel>> CarregarApontamentosAsync(short codigoEmpresa, int codigoUnidade, long codigoOrdemServico)
        {
            var apontamentos = new List<Report35ApontamentoViewModel>();
            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("sp_report_000000035_apontamento", conn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 60 })
            {
                cmd.Parameters.Add("@codigo_empresa", SqlDbType.SmallInt).Value = codigoEmpresa;
                cmd.Parameters.Add("@codigo_unidade", SqlDbType.Int).Value = codigoUnidade;
                cmd.Parameters.Add("@codigo_pcm_programada_ordem_servico", SqlDbType.BigInt).Value = codigoOrdemServico;

                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        apontamentos.Add(new Report35ApontamentoViewModel
                        {
                            Executor = reader["executor"] as string,
                            DataInicio = reader["data_hora_inicio"] as DateTime?,
                            DataTermino = reader["data_hora_termino"] as DateTime?,
                            TempoGastoMinutos = reader["tempo_gasto"] as int?,
                            DescricaoSolucao = reader["descricao_solucao"] as string
                        });
                    }
                }
            }

            return apontamentos;
        }

        private static async Task<List<Report35FotoViewModel>> CarregarFotosAsync(short codigoEmpresa, int codigoUnidade, long codigoOrdemServico, int codigoItemChecklist, string tipo)
        {
            var fotos = new List<Report35FotoViewModel>();
            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("sp_report_000000035_picture", conn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 60 })
            {
                cmd.Parameters.Add("@codigo_empresa", SqlDbType.SmallInt).Value = codigoEmpresa;
                cmd.Parameters.Add("@codigo_unidade", SqlDbType.Int).Value = codigoUnidade;
                cmd.Parameters.Add("@codigo", SqlDbType.BigInt).Value = codigoOrdemServico;
                cmd.Parameters.Add("@codigo_item_checklist", SqlDbType.Int).Value = codigoItemChecklist;
                cmd.Parameters.Add("@tipo", SqlDbType.VarChar, 20).Value = (object)tipo ?? DBNull.Value;

                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        fotos.Add(new Report35FotoViewModel
                        {
                            CaminhoArquivo = reader["foto"] as string,
                            Observacao = reader["observacao"] as string
                        });
                    }
                }
            }

            return fotos;
        }

        private class LinhaRelatorio
        {
            public string TipoOrdemServico { get; set; }
            public string Equipamento { get; set; }
            public string Programada { get; set; }
            public string Categoria { get; set; }
            public string TipoServico { get; set; }
            public string DescricaoSolucao { get; set; }
            public string Data { get; set; }
            public short CodigoTipoItemChecklist { get; set; }
            public int CodigoItemChecklist { get; set; }
            public string Unidade { get; set; }
            public string GrupoChecklist { get; set; }
            public string SubGrupoChecklist { get; set; }
            public string Checklist { get; set; }
            public string Descricao { get; set; }
            public string Resultado { get; set; }
            public string Observacao { get; set; }
            public decimal? Peso { get; set; }
        }
    }
}
