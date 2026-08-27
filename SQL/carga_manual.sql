/* Carga do manual — gerada por migrar_manual_supabase.py em 26/08/2026 23:09 */
/* Rode DEPOIS de 2026-08-27_manual_integrado.sql, que cria as tabelas.        */

SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- Refaz a carga: sai o que veio de uma execucao anterior deste script.
DELETE FROM tb_manual_item WHERE codigo_manual IN (SELECT codigo FROM tb_manual WHERE usuario = N'supabase');
UPDATE tb_manual SET codigo_manual_processo = NULL WHERE usuario = N'supabase';
DELETE FROM tb_manual WHERE usuario = N'supabase';

DECLARE @codigo int;
DECLARE @trilha TABLE (chave varchar(200) PRIMARY KEY, codigo int);

-- ---- Trilhas (manuais de processo) ----

-- Novidades na Plataforma [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Novidades na Plataforma', N'Fique por dentro de tudo que está mudando no PCM by SIM, mês a mês. Antes de seguir pra trilha da sua função, dê uma olhada aqui — é onde avisamos sobre novas telas, ajustes de fluxo e melhorias que chegam na plataforma, sem você precisar reler o manual inteiro pra descobrir o que mudou.', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
INSERT INTO @trilha (chave, codigo) VALUES (N'trilha:novidades', @codigo);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Sobre esta trilha', N'Fique por dentro de tudo que está mudando no PCM by SIM, mês a mês. Antes de seguir pra trilha da sua função, dê uma olhada aqui — é onde avisamos sobre novas telas, ajustes de fluxo e melhorias que chegam na plataforma, sem você precisar reler o manual inteiro pra descobrir o que mudou.', NULL, NULL, NULL, NULL, 1);

-- Comece por aqui [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Comece por aqui', N'Antes de mergulhar na trilha da sua função, comece por aqui. Todo colaborador da rede — do técnico ao diretor — passa por este módulo: como acessar o sistema com segurança, personalizar seu perfil e usar o LogBook, o diário de ocorrências que conecta toda a operação em tempo real. É rápido, é para t', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
INSERT INTO @trilha (chave, codigo) VALUES (N'trilha:universal', @codigo);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Sobre esta trilha', N'Antes de mergulhar na trilha da sua função, comece por aqui. Todo colaborador da rede — do técnico ao diretor — passa por este módulo: como acessar o sistema com segurança, personalizar seu perfil e usar o LogBook, o diário de ocorrências que conecta toda a operação em tempo real. É rápido, é para todos, e garante que você já fala a "língua" do PCM by SIM antes de avançar.', NULL, NULL, NULL, NULL, 1);

-- Supervisores e Técnicos [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Supervisores e Técnicos', N'Sua rotina em campo, sem papel e sem retrabalho. Aprenda a receber e executar Ordens de Serviço, apontar horas e materiais, e cumprir rotinas e rondas de forma estruturada — tudo pelo celular. Dominar esta trilha transforma manutenção reativa em trabalho organizado e rastreável, com cada apontamento', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
INSERT INTO @trilha (chave, codigo) VALUES (N'trilha:tecnico', @codigo);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Sobre esta trilha', N'Sua rotina em campo, sem papel e sem retrabalho. Aprenda a receber e executar Ordens de Serviço, apontar horas e materiais, e cumprir rotinas e rondas de forma estruturada — tudo pelo celular. Dominar esta trilha transforma manutenção reativa em trabalho organizado e rastreável, com cada apontamento seu contando na produtividade e no histórico técnico da unidade.', NULL, NULL, NULL, NULL, 1);

-- Governança & Camareira [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Governança & Camareira', N'O painel de comando da governança hoteleira. Aqui você aprende a monitorar o status de cada quarto em tempo real, controlar o ciclo do enxoval e acompanhar a produtividade e a qualidade da equipe de camareiras. Essas seções conectam governança, manutenção e recepção numa única visão — e são o que ga', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
INSERT INTO @trilha (chave, codigo) VALUES (N'trilha:camareira', @codigo);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Sobre esta trilha', N'O painel de comando da governança hoteleira. Aqui você aprende a monitorar o status de cada quarto em tempo real, controlar o ciclo do enxoval e acompanhar a produtividade e a qualidade da equipe de camareiras. Essas seções conectam governança, manutenção e recepção numa única visão — e são o que garante que nenhum hóspede seja alocado num apartamento com problema.', NULL, NULL, NULL, NULL, 1);

-- Gestor de PCM [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Gestor de PCM', N'A trilha da operação de PCM em si — planejamento de preventivas, aprovações, laudos, PMOC, Green Planet e indicadores de desempenho. Os cadastros básicos que alimentam tudo isso (unidades, equipamentos, equipe) agora têm trilha própria: Gestor — Cadastros. Ao dominar estas seções, você deixa de apen', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
INSERT INTO @trilha (chave, codigo) VALUES (N'trilha:gestor', @codigo);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Sobre esta trilha', N'A trilha da operação de PCM em si — planejamento de preventivas, aprovações, laudos, PMOC, Green Planet e indicadores de desempenho. Os cadastros básicos que alimentam tudo isso (unidades, equipamentos, equipe) agora têm trilha própria: Gestor — Cadastros. Ao dominar estas seções, você deixa de apenas usar o sistema e passa a comandá-lo — transformando dados operacionais em decisões de gestão.', NULL, NULL, NULL, NULL, 1);

-- Gestor — Cadastros [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Gestor — Cadastros', N'A base de tudo: os 17 cadastros que sustentam a operação inteira do PCM by SIM — unidades, setores, U.H.s, equipamentos, equipe, checklists, prioridades, fornecedores e mapa de manutenção. Sem esses cadastros feitos corretamente, nenhum outro módulo funciona direito. Separamos esta trilha da Gestor ', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
INSERT INTO @trilha (chave, codigo) VALUES (N'trilha:gestor-cadastros', @codigo);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Sobre esta trilha', N'A base de tudo: os 17 cadastros que sustentam a operação inteira do PCM by SIM — unidades, setores, U.H.s, equipamentos, equipe, checklists, prioridades, fornecedores e mapa de manutenção. Sem esses cadastros feitos corretamente, nenhum outro módulo funciona direito. Separamos esta trilha da Gestor de PCM para deixar claro o que é configuração de base e o que é operação do dia a dia.', NULL, NULL, NULL, NULL, 1);

-- Solicitante [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Solicitante', N'A porta de entrada mais simples do sistema, para qualquer colaborador. Aprenda a abrir uma requisição de serviço em segundos, sem precisar conhecer o resto da plataforma. Um problema relatado rápido e bem descrito é o primeiro passo de toda manutenção bem-sucedida.', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
INSERT INTO @trilha (chave, codigo) VALUES (N'trilha:solicitante', @codigo);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Sobre esta trilha', N'A porta de entrada mais simples do sistema, para qualquer colaborador. Aprenda a abrir uma requisição de serviço em segundos, sem precisar conhecer o resto da plataforma. Um problema relatado rápido e bem descrito é o primeiro passo de toda manutenção bem-sucedida.', NULL, NULL, NULL, NULL, 1);

-- Almoxarife [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Almoxarife', N'Controle total sobre o que entra, sai e falta no seu estoque. Aqui você aprende a gerenciar entradas e saídas de materiais, e abrir requisições e ordens de compra com rastreabilidade completa. O resultado: menos ruptura de estoque, menos gasto por urgência, e um custo de manutenção que você consegue', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
INSERT INTO @trilha (chave, codigo) VALUES (N'trilha:almoxarife', @codigo);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Sobre esta trilha', N'Controle total sobre o que entra, sai e falta no seu estoque. Aqui você aprende a gerenciar entradas e saídas de materiais, e abrir requisições e ordens de compra com rastreabilidade completa. O resultado: menos ruptura de estoque, menos gasto por urgência, e um custo de manutenção que você consegue explicar linha por linha.', NULL, NULL, NULL, NULL, 1);

-- Ativo Fixo [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Ativo Fixo', N'Controle patrimonial completo dos bens do hotel. Aprenda a cadastrar cada ativo com sua etiqueta patrimonial, registrar baixas, manutenções e transferências, e conduzir os inventários físicos periódicos que confirmam se o que está no sistema bate com o que existe de verdade nos andares. Módulo novo ', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
INSERT INTO @trilha (chave, codigo) VALUES (N'trilha:ativofixo', @codigo);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Sobre esta trilha', N'Controle patrimonial completo dos bens do hotel. Aprenda a cadastrar cada ativo com sua etiqueta patrimonial, registrar baixas, manutenções e transferências, e conduzir os inventários físicos periódicos que confirmam se o que está no sistema bate com o que existe de verdade nos andares. Módulo novo na plataforma — este conteúdo ainda está em construção conjunta com a SIM Services.', NULL, NULL, NULL, NULL, 1);

-- Administrador [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Administrador', N'As chaves do sistema estão nas suas mãos. Domine a criação de usuários, a definição de permissões e a hierarquia de perfis que decide quem vê o quê — a base de segurança e governança de toda a plataforma. Um cadastro bem-feito aqui evita retrabalho, protege dados sensíveis e garante que cada ação no', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
INSERT INTO @trilha (chave, codigo) VALUES (N'trilha:administrador', @codigo);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Sobre esta trilha', N'As chaves do sistema estão nas suas mãos. Domine a criação de usuários, a definição de permissões e a hierarquia de perfis que decide quem vê o quê — a base de segurança e governança de toda a plataforma. Um cadastro bem-feito aqui evita retrabalho, protege dados sensíveis e garante que cada ação no sistema tenha um responsável identificável.', NULL, NULL, NULL, NULL, 1);

-- Qualidade & Auditoria [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Qualidade & Auditoria', N'O ciclo de melhoria contínua da sua operação, fechado com dados. Aprenda a planejar e executar auditorias corporativas e de qualidade, manter o repositório de normas e POPs atualizado, e garantir a conformidade sanitária da área de Alimentos e Bebidas — uma das mais fiscalizadas do setor hoteleiro.', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
INSERT INTO @trilha (chave, codigo) VALUES (N'trilha:qualidade', @codigo);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Sobre esta trilha', N'O ciclo de melhoria contínua da sua operação, fechado com dados. Aprenda a planejar e executar auditorias corporativas e de qualidade, manter o repositório de normas e POPs atualizado, e garantir a conformidade sanitária da área de Alimentos e Bebidas — uma das mais fiscalizadas do setor hoteleiro.', NULL, NULL, NULL, NULL, 1);

-- Financeiro [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Financeiro', N'Onde a operação vira prestação de contas. Aprenda a planejar o orçamento por departamento, lançar despesas e gerir contratos de fornecedores — conectando cada gasto real ao que foi previsto. É a ferramenta que dá à diretoria uma visão clara e confiável da saúde financeira da manutenção.', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
INSERT INTO @trilha (chave, codigo) VALUES (N'trilha:financeiro', @codigo);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Sobre esta trilha', N'Onde a operação vira prestação de contas. Aprenda a planejar o orçamento por departamento, lançar despesas e gerir contratos de fornecedores — conectando cada gasto real ao que foi previsto. É a ferramenta que dá à diretoria uma visão clara e confiável da saúde financeira da manutenção.', NULL, NULL, NULL, NULL, 1);

-- ---- Artigos ----

-- Hierarquia de Perfis e Permissões [Administracao/PerfilHierarquia]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Administracao', N'PerfilHierarquia', N'Hierarquia de Perfis e Permissões', N'1.4 — Administrador', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:administrador') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'🎯 O que você vai conquistar', N'Esta seção explica como o sistema organiza os níveis de autoridade entre os diferentes tipos de usuário e como configurar essa estrutura. Ao dominar a hierarquia de perfis, você garante que cada colaborador veja apenas o que precisa ver, que as aprovações sigam o fluxo correto de autoridade e que dados sensíveis (financeiro, relatórios de rede) fiquem protegidos dos perfis operacionais.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Você precisa ter perfil de Administrador para reordenar a hierarquia de perfis.
> Os perfis precisam existir em Administração > Perfil antes de serem ordenados.
> Defina a hierarquia antes de criar os primeiros usuários da unidade.


O sistema PCM by SIM utiliza uma escala numérica de níveis para organizar a cadeia de autoridade. Quanto menor o número do nível, maior a autoridade do perfil:', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'🧭 Visão do Fluxo', N'| 1️⃣ Perfil CRIADO | → | 2️⃣ Permissões DEFINIDAS | → | 3️⃣ Hierarquia DEFINIDA | → | 4️⃣ Usuário VINCULADO | → | 📊 Acesso Ativo |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🚀 Passo a Passo', N'### 4.1  Acessar a tela de Hierarquia de Perfis

![Administração > Perfil — Hierarquia — lista de perfis ordenados por nível](/screenshots/admin-perfil-hierarquia-real.png)


1. No menu lateral, clique em Administração.
1. Clique em Perfil, Hierarquia.
1. A tela exibe a lista de todos os perfis cadastrados, ordenados do nível mais alto (1) ao mais baixo.


### 4.2  Reordenar um perfil na hierarquia

1. Localize o perfil que deseja reposicionar na lista.
1. Use as setas ▲ (subir) ou ▼ (descer) ao lado do nome do perfil para ajustar sua posição na hierarquia.
1. Cada clique nas setas move o perfil um nível acima ou abaixo, reajustando os demais automaticamente.
1. A alteração é salva automaticamente, não há botão Salvar nesta tela.


### 4.3  Criar um novo perfil (Administração > Perfil)

Antes de posicionar um perfil na hierarquia, ele precisa existir. Para criar:

1. Acesse Administração > Perfil (diferente de Perfil, Hierarquia).
1. Clique em Novo e preencha a Descrição do perfil (ex: ''Governança — Execução'').
1. Defina as permissões de módulo que este perfil terá acesso (quais menus aparecem na navegação).
1. Clique em Salvar, depois volte para Perfil, Hierarquia e posicione o novo perfil no nível correto.


Perfis típicos recomendados para uma operação hoteleira (nomes simplificados neste manual para ficarem compreensíveis independente do nome exato configurado em cada unidade, consulte Administração > Perfil, Hierarquia para ver a lista real de perfis já cadastrados):', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Nível | Posição na cadeia de autoridade: menor = mais alto | Sim | 1 (Admin) / 5 (Técnico) |
| Perfil | Nome do cargo ou função no sistema | Sim | Gestor de PCM |
| Permissões | Módulos e funcionalidades acessíveis | Sim | PCM, Estoque, Relatórios |
| Setas de ordenação | Reposicionam o perfil na hierarquia | --- | Clicar para subir ou descer um nível |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Defina a hierarquia em ordem crescente de restrição: Administrador no topo, Técnico na base.
> Revise a hierarquia a cada mudança organizacional para evitar acessos incorretos.
> Documente os critérios de cada nível em um POP interno para facilitar a integração de novos colaboradores.


> [!DANGER]
> Reordenar a hierarquia afeta imediatamente as aprovações em andamento, antes de mover um perfil, confirme que não há fluxos de aprovação pendentes que dependam do nível atual dele.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Um perfil não aparece na lista de hierarquia | Perfil inativo ou vinculado a outra unidade | Acesse Administração > Perfil, localize e ative o switch Ativo |
| A hierarquia não está sendo respeitada nas aprovações | Perfil do usuário em nível incorreto | Revise o posicionamento em Administração > Perfil — Hierarquia e reordene conforme necessário |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Administração > Perfil — cadastro de perfis (seção 1.3) | Todos os módulos do sistema: a hierarquia define o nível de autoridade e o que cada perfil pode ver ou aprovar | Reordenação de nível aplicada instantaneamente a todos os usuários vinculados ao perfil |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Empresa (Fornecedor/Prestador) [CadastroBasico/FornecedorIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'FornecedorIndex', N'Cadastro de Empresa (Fornecedor/Prestador)', N'1.7 — Administrador', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:administrador') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Administradores do sistema. | Menu lateral > Administração > Empresa pcmbysim.com.br/Administracao/Empresa |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de consultar e gerenciar o status das empresas (fornecedores e prestadores de serviço) cadastradas a nível administrativo do sistema, diferente do Cadastro Básico > Fornecedor (seção 2.15), que é o cadastro operacional usado em estoque e ordens de compra. Esta tela em Administração é a visão consolidada de todas as empresas parceiras da rede.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Você precisa ter perfil Administrador para acessar esta tela.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Listagem ACESSADA | → | 2️⃣ Empresa LOCALIZADA | → | 3️⃣ Ativar/Inativar | → | 📊 Reflete no Sistema |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Consultar e gerenciar empresas cadastradas

1. Acesse Administração > Empresa.
1. A tela lista todas as empresas (fornecedores e prestadores de serviço) já cadastradas no sistema, identificadas pelo Nome Fantasia.
1. Para inativar ou reativar uma empresa, clique no ícone de status ao lado do nome — o link real alterna entre Ativar/Inativar por empresa.


Empresas inativas deixam de aparecer nas seleções de fornecedor em outras telas do sistema.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Nome Fantasia | Identificação da empresa na listagem | Sim | Engenharia e Manutenção |
| Ativo | Define se a empresa aparece nas seleções de fornecedor | — | Alternado pelo ícone de status na listagem |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Inative empresas que encerraram o contrato com a rede, em vez de deixá-las na listagem, mantém a lista de seleção limpa nas demais telas do sistema.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Uma empresa não aparece mais em uma seleção de fornecedor | Empresa foi inativada | Acesse Administração > Empresa e reative pelo ícone de status |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro realizado a nível administrativo | Seleções de fornecedor/prestador em outras telas do sistema | Status Ativo reflete imediatamente nas seleções |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Cliente (Unidade Hoteleira) [Administracao/ClienteIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Administracao', N'ClienteIndex', N'Cadastro de Cliente (Unidade Hoteleira)', N'1.8 — Administrador', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:administrador') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Uso interno da equipe PCMbySIM: não é uma tela operada pelo administrador do hotel cliente. | Menu lateral > Administração > Cliente pcmbysim.com.br/Administracao/Cliente |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de cadastrar uma nova rede/cliente na plataforma antes de criar as Unidades operacionais dela (seção 2.1). É o primeiro passo da implantação de um novo cliente no PCM by SIM, sem um Cliente cadastrado, não é possível vincular uma Unidade a ele.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Esta tela é de uso interno da equipe PCMbySIM durante a implantação de um novo cliente, o administrador do próprio hotel normalmente não precisa (nem deve) acessá-la.
> Tenha em mãos os dados fiscais completos do cliente: CNPJ, Razão Social, Inscrições Estadual/Municipal e endereço.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Novo CLIENTE | → | 2️⃣ Dados FISCAIS | → | 3️⃣ Cliente SALVO | → | 📊 Disponível p/ Vínculo |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Cadastrar um Cliente

1. Acesse Administração > Cliente e clique em Novo.
1. O formulário real é bem mais completo do que só CNPJ/Nome Fantasia/UF/Município — tem os mesmos campos ricos do Cadastro de Unidades (seção 2.1): CNPJ, Nome Fantasia, Razão Social, Inscrição Estadual, Inscrição Municipal, CEP, UF, Município, Logradouro, Número, Bairro, Complemento, Telefone, Área Total (m²), Área Total Construída (m²), o switch Aponta Horas, Ativo e upload de Logo.
1. Preencha ao menos CNPJ e Nome Fantasia (obrigatórios) e o restante dos dados fiscais e de endereço que tiver disponível.
1. Mantenha Ativo habilitado e clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O cliente cadastrado fica disponível para vínculo com uma Unidade operacional (seção 2.1).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| CNPJ | Cadastro fiscal do cliente | Sim | 00.000.000/0001-00 |
| Nome Fantasia | Nome comercial do cliente | Sim | Hotel by SIM Services |
| Razão Social | Nome jurídico | Não | Hotel by SIM Ltda. |
| Inscrição Estadual / Municipal | Registros fiscais complementares | Não | 123.456.789.000 |
| CEP / Endereço | Localização completa (Logradouro, Número, Bairro, Complemento, Município, UF) | Não | 01310-100, Av. Paulista, 1000 |
| Área Total (m²) / Área Total Construída (m²) | Metragem do empreendimento | Não | 12.000 m² |
| Aponta Horas | Switch que habilita apontamento de horas para o cliente | Não | Ativado |
| Ativo | Disponibiliza o cliente para vínculo com Unidade | Sim | Sempre ativo em novo cadastro |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Preencha os dados fiscais completos já na implantação, evita retrabalho posterior quando o cliente precisar de nota fiscal ou contrato formalizado dentro da plataforma.


> [!DANGER]
> Um Cliente é o nível acima de Unidade, cadastrar a Unidade errada dentro do Cliente errado mistura dados de redes diferentes. Confirme o Cliente correto antes de criar a Unidade (seção 2.1).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Não encontro o Cliente ao tentar vincular uma nova Unidade | Cliente ainda não foi cadastrado ou está inativo | Acesse Administração > Cliente e cadastre-o (ou reative) antes de criar a Unidade |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Dados fiscais fornecidos pelo cliente na implantação | Cadastro de Unidades (seção 2.1): toda Unidade pertence a um Cliente | Cliente disponível para vínculo imediatamente após o cadastro |', NULL, NULL, NULL, NULL, 1);

-- Configuração de Pesos do Dashboard de Desempenho [Governanca/Dashboard]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Governanca', N'Dashboard', N'Configuração de Pesos do Dashboard de Desempenho', N'1.9 — Administrador', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:administrador') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Administradores do sistema. | Menu lateral > Administração > Desempenho das Unidades pcmbysim.com.br/Administracao/DesempenhoUnidades |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de configurar o peso percentual de cada indicador usado no cálculo da Nota Geral do Dashboard, Visão Geral e KPIs (seção 7.1). É uma parametrização de indicador, não um cadastro: não cria um registro novo, ajusta como os indicadores já existentes se combinam na nota final da unidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Você precisa ter perfil Administrador para acessar e alterar esta tela.
> Alterar os pesos aqui recalcula a Nota Geral de todas as unidades, avise a rede antes de mudar.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Painel ACESSADO | → | 2️⃣ Pesos AJUSTADOS | → | 3️⃣ Soma 100% CONFIRMADA | → | 4️⃣ Config. SALVA | → | 📊 Nota Geral Recalculada |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Configurar os pesos

1. Acesse Administração > Desempenho das Unidades.
1. Ajuste o peso percentual de cada indicador usado no cálculo da Nota Geral do Dashboard. Os 11 indicadores reais são: Laudo/Documentação, Preventiva, Rotina, PMOC, U.H. em Dia, No Dia (não ''Atendimento de OS''), HH Não Apontado, OS Pendente, HH Extra, Preventiva x Corretiva e Green Planet.
1. A soma de todos os pesos precisa fechar em 100%, o sistema mostra o total somado em tempo real.
1. Clique em Salvar.


> [!INFO]
> **PESOS PADRÃO CONFIRMADOS AO VIVO**
> Laudo/Documentação 20% · Preventiva 15% · Rotina 15% · PMOC 15% · U.H. em Dia 15% · No Dia 2% · HH Não Apontado 2% · OS Pendente 2% · HH Extra 2% · Preventiva x Corretiva 2% · Green Planet 10%, soma 100%.


> [!INFO]
> **RESULTADO ESPERADO**
> Os novos pesos passam a valer no próximo cálculo da Nota Geral do Dashboard, Visão Geral e KPIs (seção 7.1).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Laudo/Documentação | Peso do indicador de laudos no cálculo da Nota Geral | Sim | 20% |
| Preventiva | Peso do indicador de preventivas em dia | Sim | 15% |
| Rotina | Peso do indicador de rotinas em dia | Sim | 15% |
| PMOC | Peso do indicador de PMOC em dia | Sim | 15% |
| U.H. em Dia | Peso do indicador de conformidade de U.H. | Sim | 15% |
| No Dia | Peso do indicador de atendimento no prazo | Sim | 2% |
| HH Não Apontado | Peso do indicador de horas não apontadas | Sim | 2% |
| OS Pendente | Peso do indicador de OS em aberto | Sim | 2% |
| HH Extra | Peso do indicador de horas extras | Sim | 2% |
| Preventiva x Corretiva | Peso do indicador de proporção preventiva/corretiva | Sim | 2% |
| Green Planet | Peso do indicador de sustentabilidade | Sim | 10% |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Confira sempre se a soma dos 11 pesos fecha 100% antes de salvar.
> Evite alterar os pesos padrão sem um motivo estratégico da rede, a Nota Geral só é comparável entre unidades se o critério de cálculo for o mesmo para todas.


> [!DANGER]
> Alterar os pesos recalcula a Nota Geral de **todas** as unidades da rede, não é uma mudança isolada de uma unidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A soma dos pesos não fecha 100% | Um dos 11 campos foi digitado errado | Revise cada peso antes de salvar |
| A Nota Geral de uma unidade mudou sem explicação aparente | Os pesos globais foram alterados por um Administrador | Confira em Administração > Desempenho das Unidades se os pesos padrão foram modificados recentemente |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Os 11 indicadores operacionais da unidade (Laudo, Preventiva, Rotina, PMOC, U.H. em Dia, etc.) | Dashboard: Visão Geral e KPIs (seção 7.1): Nota Geral | Recálculo em tempo real ao salvar |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Unidades [CadastroBasico/UnidadeIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'UnidadeIndex', N'Cadastro de Unidades', N'2.1 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Administradores do sistema e gestores regionais. É o primeiro cadastro a ser feito em qualquer implantação. | Menu lateral > Cadastro Básico > Unidades pcmbysim.com.br/CadastroBasico/UnidadeIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar e configurar uma unidade de negócio no sistema, hotel, condomínio ou qualquer instalação gerenciada. A unidade é a entidade-raiz do PCM by SIM: todos os ativos, colaboradores, OS, preventivas e relatórios são filtrados por ela. Cadastrar a unidade corretamente desde o início evita retrabalho em todos os outros módulos e garante que os relatórios mostrem os dados da propriedade certa.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Tenha em mãos: CNPJ, Razão Social, Inscrição Estadual e endereço completo da unidade.
> Prepare os logotipos em dois formatos: 250×120 px (logo principal) e 500×500 px (logo secundário).
> Se a unidade usa o sistema Opera (PMS hoteleiro), tenha o código de integração disponível antes de salvar.
> Apenas perfil Administrador tem acesso a este cadastro.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Dados FISCAIS | → | 2️⃣ Endereço e CONTATO | → | 3️⃣ Config. OPERACIONAIS | → | 4️⃣ Dados de EDIFICAÇÃO | → | 5️⃣ Logos ENVIADOS | → | 📊 Unidade Salva |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1 Criar uma nova unidade

![Tela Cadastro Básico > Unidades — botão Novo e listagem de unidades existentes](/screenshots/cb-unidades-listagem.png)



1. Acesse Cadastro Básico > Unidades e clique em Novo.
1. Selecione o Tipo de Unidade, as opções reais são Casa de Campo, Casa de Praia, Casa na Montanha, Condomínio, Empresa e Não Aplicável (não existe opção literal Hotel ou Hospital; para hotéis, use Empresa ou Não Aplicável).


**Bloco A, Dados fiscais**

1. Preencha CNPJ, Razão Social, Nome Fantasia e as inscrições estadual e municipal.


**Bloco B, Localização e contato**

1. Preencha o endereço completo, no formulário real são 7 campos separados: CEP, UF, Município, Logradouro, Número, Bairro e Complemento (este último não documentado antes), e o Telefone principal.
1. Se aplicável, preencha o campo Hotel Opera com o código de integração fornecido pela equipe do PMS.


**Bloco C, Configurações operacionais**

1. Ative o switch Ativo para tornar a unidade disponível em todos os módulos operacionais.
1. Ative Aponta Horas, Qualidade se a unidade registra horas trabalhadas no módulo de QA.
1. Mais abaixo no formulário, dentro da seção Configuração, existem mais dois switches: Aponta Horas - Manutenção e um segundo Aponta Horas - Qualidade (mesmo nome repetido), é uma duplicação real da interface. Até a correção, ative os dois conforme a necessidade da unidade.


**Bloco D, Dados de edificação**

1. Preencha Quantidade de Blocos, Andares, Área Total (m²) e Área Total Construída (m²). Esses dados alimentam os cálculos de PMOC e dimensionamento de equipe.


**Bloco E, Logos**

1. Faça o upload do Logo Principal (250×120 px) e do Logo Secundário (500×500 px), dimensões reais confirmadas no formulário. Esses logos aparecem nos cabeçalhos dos relatórios impressos.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A unidade aparece na listagem e fica disponível como filtro em todos os módulos do sistema.
> Os relatórios impressos já exibirão o logo cadastrado no cabeçalho.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Tipo de Unidade | Classificação da propriedade: 6 opções reais, nenhuma chamada Hotel | Sim | Casa de Campo / Casa de Praia / Casa na Montanha / Condomínio / Empresa / Não Aplicável |
| CNPJ | Cadastro Nacional da Pessoa Jurídica | Sim | 00.000.000/0001-00 |
| Nome Fantasia | Nome comercial da unidade | Sim | Hotel PCM by SIM |
| Razão Social | Nome jurídico da empresa | Sim | PCM by SIM Services Ltda. |
| Insc. Estadual/Mun. | Registros fiscais complementares | Não | 123.456.789.000 |
| Endereço completo | 7 campos reais: CEP, UF, Município, Logradouro, Número, Bairro, Complemento | Sim | Av. Paulista, 1000, SP |
| Telefone | Contato principal da unidade | Não | (11) 4321-4321 |
| Hotel Opera | Código de integração com o PMS Opera | Não | Hotel_PCMbySIM |
| Ativo | Habilita a unidade em todos os módulos | Sim | Ativar ao salvar |
| Qtde. Blocos/Andares | Estrutura física: alimenta PMOC e dimensionamento | Não | 2 blocos / 18 andares |
| Área Total (m²) | Área do terreno total | Não | 4.500 m² |
| Área Construída (m²) | Soma de todos os pavimentos construídos | Não | 12.600 m² |
| Logo 250×120 | Logo principal para relatórios: dimensão real confirmada | Não | logo_PCMbySIM.png |
| Logo 500×500 | Logo secundário: dimensão real confirmada | Não | icon_PCMbySIM.png |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Preencha os dados de edificação (blocos, andares, área) com precisão, eles são usados nos cálculos de carga térmica do PMOC e no dimensionamento de equipes de manutenção.
> Use o Nome Fantasia exatamente como ele aparece nos sistemas integrados (Opera, financeiro) para evitar divergências em relatórios consolidados de rede.
> Faça o upload dos logos em alta resolução desde o início, substituir depois exige abrir cada relatório já salvo para verificar a qualidade.


> [!DANGER]
> Uma unidade inativa (switch Ativo desligado) desaparece de todos os filtros do sistema, OS, preventivas e usuários vinculados ficam sem acesso. Só inative uma unidade após confirmar que nenhum dado em andamento depende dela.
> O CNPJ é único no sistema, não é possível cadastrar duas unidades com o mesmo CNPJ. Para filiais do mesmo grupo, use CNPJs distintos ou consulte o suporte técnico.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A unidade não aparece no filtro de outras telas após o cadastro | O switch Ativo não foi habilitado antes de salvar | Edite a unidade, ative o switch Ativo e salve: ela aparecerá imediatamente em todos os filtros |
| O logo não aparece nos relatórios mesmo após o upload | Arquivo em formato incompatível ou resolução incorreta | Use PNG ou JPG. Logo principal: 250×120 px. Logo secundário: 500×500 px. Recomendado: fundo transparente (PNG) |
| O sistema rejeita o CNPJ informado | CNPJ já cadastrado em outra unidade ou formato incorreto | Verifique se o CNPJ já existe na listagem. Use sempre o formato com pontuação: 00.000.000/0001-00 |
| O campo Hotel Opera não é exibido no formulário | A integração com Opera não está habilitada na configuração do sistema | Contate o Administrador para verificar se o módulo de integração Opera está ativado na conta |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Administração > Empresa (configuração do grupo)Perfil do Administrador | Todos os módulos: filtro global de UnidadeCadastro Básico > Setores, U.H., EquipamentosMódulo Financeiro — rateio de despesas por unidade | Logo exibido automaticamente em todos os relatórios impressosUnidade disponível em filtros imediatamente após ativação |
| Dados de edificação (blocos, andares, área) | PMOC: carga térmica e dimensionamentoDashboard: comparativo de rede (todas as unidades)Relatórios de desempenho por unidade | Integração automática com Opera se campo preenchido |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Funções [CadastroBasico/FuncaoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'FuncaoIndex', N'Cadastro de Funções', N'2.10 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Administradores e gestores de RH. Realizado antes do cadastro de colaboradores. | Menu lateral > Cadastro Básico > Função pcmbysim.com.br/CadastroBasico/FuncaoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de criar e gerenciar o catálogo de cargos e especialidades técnicas da unidade. A Função é o cargo operacional de cada colaborador, ela define a especialidade técnica (Eletricista, Técnico de Refrigeração, Camareira) e aparece nos relatórios de escalabilidade, permitindo ao gestor filtrar a equipe por competência antes de atribuir uma OS que exige habilidade específica.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A Unidade precisa estar cadastrada: seção 2.1.
> Defina as funções em conjunto com RH e PCM, use nomes alinhados com os cargos reais da CLT e com a linguagem técnica da equipe.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Função CRIADA | → | 2️⃣ Colaborador CADASTRADO | → | 3️⃣ Função VINCULADA | → | 📊 Filtro por Especialidade |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Cadastro Básico > Função — listagem com as funções reais cadastradas](/screenshots/cadastro-funcao-listagem.png)


1. Acesse Cadastro Básico > Função e clique em Novo.
1. Selecione a Unidade à qual a função pertence.
1. No campo Descrição, insira o nome oficial do cargo ou especialidade técnica.
1. Mantenha Ativo habilitado e clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A função aparece disponível para seleção no cadastro de colaboradores e como filtro na listagem da equipe.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade à qual a função pertence | Sim | Intercity Berrini |
| Descrição | Nome do cargo ou especialidade técnica | Sim | Oficial de Manutenção |
| Ativo | Disponibiliza a função para seleção no cadastro de equipe | Sim | Sempre ativo |


> [!WARNING]
> **A tabela abaixo é uma sugestão, não a lista real**
> A lista real de funções da sua unidade pode ser mais enxuta que o exemplo abaixo (ex.: Camareira, Gerente de Manutenção, Governanta Executiva, Supervisor de Manutenção, Supervisora de Andares). Use a tabela abaixo como ponto de partida, mas confirme com sua equipe antes de criar novas funções.


Funções recomendadas por módulo em unidade hoteleira:

| Módulo | Funções típicas |
| :--- | :--- |
| Manutenção | Oficial de Manutenção, Eletricista, Técnico de Refrigeração, Encanador, Pintor, Marceneiro, Auxiliar de Manutenção, Supervisor de Manutenção, Gestor de PCM |
| Governança | Camareira, Supervisora de Governança, Governanta, Vistoriadora, Auxiliar de Rouparia, Lavadeira |
| Qualidade | Auditor de Qualidade, Coordenador de Qualidade, Inspetor |
| A&B | Chef de Cozinha, Cozinheiro, Auxiliar de Cozinha, Garçom, Nutricionista |
| Segurança | Vigilante, Supervisor de Segurança, Porteiro |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Use os nomes oficiais do CBO (Classificação Brasileira de Ocupações), facilita a integração com sistemas de RH e folha de pagamento.
> Crie uma função separada para cada especialidade técnica, mesmo que o salário seja o mesmo. ''Eletricista'' e ''Encanador'' têm competências diferentes e precisam aparecer separados nos filtros de escalonamento.


> [!DANGER]
> Inativar uma função não afeta colaboradores já vinculados a ela, mas remove a função dos filtros futuros. Verifique quantos colaboradores ativos usam uma função antes de inativá-la.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Função não aparece no cadastro do colaborador | Função inativa | Acesse Cadastro Básico > Função, localize e ative o switch Ativo |
| Colaboradores com mesma especialidade têm funções diferentes | Funções criadas em momentos distintos com nomes diferentes | Padronize: crie um único nome e inative os duplicados |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Administração > Unidades | Cadastro Básico > Colaboradores — campo Função obrigatórioAdministração > Usuários — campo Função no bloco de colaborador | Filtro por Função na listagem de colaboradoresEscalonamento de OS por especialidade técnica |
| Especialidade técnica definida | Relatórios de RH: distribuição de equipe por cargoPlanejamento de contratações por especialidade |  |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Departamentos [CadastroBasico/DepartamentoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'DepartamentoIndex', N'Cadastro de Departamentos', N'2.11 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Administradores, gestores de RH e financeiro. Realizado antes do cadastro de colaboradores. | Menu lateral > Cadastro Básico > Departamento pcmbysim.com.br/CadastroBasico/DepartamentoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de criar e gerenciar os centros de custo organizacionais da unidade. O Departamento é a dimensão administrativa que agrupa colaboradores por área funcional, Manutenção, Governança, Recepção, A&B, e que o módulo Financeiro usa para ratear despesas e gerar análises de custo por setor. Uma estrutura de departamentos bem definida transforma os relatórios financeiros de números totais em inteligência por área.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Defina os departamentos alinhados com o organograma real da unidade, use os mesmos nomes utilizados pelo RH e pelo financeiro.
> Crie os departamentos antes dos colaboradores, o campo Departamento é obrigatório no cadastro de usuários.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Departamento CRIADO | → | 2️⃣ Colaborador VINCULADO | → | 3️⃣ Gestor VINCULADO | → | 📊 Despesas por Depto. |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Cadastro Básico > Departamento — listagem com os departamentos reais cadastrados](/screenshots/cadastro-departamento-listagem.png)


1. Acesse Cadastro Básico > Departamento e clique em Novo.
1. No campo Descrição, insira o nome oficial do departamento.
1. Mantenha Ativo habilitado e clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O departamento aparece disponível para seleção em colaboradores, usuários, Planos de Ação e relatórios financeiros.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Descrição | Nome oficial do departamento / centro de custo | Sim | Manutenção |
| Ativo | Disponibiliza o departamento para vínculos e lançamentos | Sim | Sempre ativo |


> [!SUCCESS]
> **Lista real de departamentos (11 no total)**
> A&B, Administrativo, Cozinha, Estacionamento, Gerência, Governança, Manutenção, Qualidade, Recepção, Restaurante, Técnico, mesma lista usada em ''Departamento Responsável'' nos cadastros de Equipamento e Ar Condicionado. A tabela ''típicos'' abaixo é uma referência conceitual, Segurança, Compras e RH sugeridos ali não existem cadastrados hoje.


Departamentos típicos em unidades hoteleiras:

| Departamento | Principal uso no sistema | Exemplos de análise financeira |
| :--- | :--- | :--- |
| Manutenção | PCM, OS, Preventivas, Laudos | Custo de mão de obra e materiais por manutenção |
| Governança | Apontamento de camareiras, enxoval, lavanderia | Custo de lavanderia e enxoval por período |
| Recepção / Front | OS de hóspedes, integração Opera | Volume de chamados por área de hóspedes |
| Alimentos & Bebidas | Auditoria A&B, laudos sanitários, contratos | Custo de higienização e laudos obrigatórios |
| Segurança | OS de CCTV, controle de acesso | Custo de manutenção de equipamentos de segurança |
| Administração / TI | Usuários, configurações do sistema | Custo de contratos de software e infraestrutura |
| Compras | Aprovação de requisições, ordens de compra | Volume e valor de compras por período |
| RH | Gestão de colaboradores, faltas, treinamentos | Custo de mão de obra total por departamento |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Crie departamentos no mesmo nível de granularidade que você quer ver nos relatórios financeiros. Se você quer ver o custo de Manutenção separado do custo de Governança, crie dois departamentos, não um único ''Operações''.
> Alinhe os nomes com o plano de contas do financeiro, isso facilita a integração com o ERP e elimina retrabalho de reclassificação contábil.


> [!DANGER]
> Inativar um departamento impede novos vínculos mas não remove os colaboradores e despesas já associados. Antes de inativar, transfira os colaboradores ativos para outro departamento.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Despesas não aparecem no relatório do departamento | Lançamento feito com departamento incorreto ou em branco | Edite o lançamento financeiro e corrija o departamento |
| Colaborador não aparece no filtro de departamento | Colaborador cadastrado sem departamento vinculado | Edite o colaborador e vincule o departamento correto |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Organograma da unidade (estrutura organizacional) | Cadastro Básico > Colaboradores — campo DepartamentoAdministração > Usuários — campo DepartamentoPlano de Ação: atribuição por departamento | Centro de custo ativo em lançamentos financeiros imediatamente após cadastro |
| Estrutura de centros de custo | Módulo Financeiro: análise de gastos por departamentoRelatórios de BI: custo e produtividade por áreaGestores de Departamento (seção 2.12) | Filtro de departamento em OS, Preventivas e Relatórios |', NULL, NULL, NULL, NULL, 1);

-- Gestores de Departamento [CadastroBasico/DepartamentoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'DepartamentoIndex', N'Gestores de Departamento', N'2.12 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Administradores do sistema. Configurado após criar departamentos e usuários gestores. | Menu lateral > Cadastro Básico > Departamento — Gestor pcmbysim.com.br/CadastroBasico/DepartamentoGestorIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de vincular formalmente um responsável a cada departamento da unidade, definindo quem recebe notificações de OS críticas, quem aprova requisições e quem é responsável pelos indicadores de desempenho de cada área. O Gestor de Departamento é o ''dono'' operacional da área, sem esse vínculo, o sistema não sabe para quem direcionar alertas e aprovações, e as notificações simplesmente não chegam a ninguém.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Os departamentos precisam estar cadastrados: seção 2.11.
> Os usuários que serão gestores precisam estar criados: seção 1.3 (recomendação de processo, não trava técnica — o dropdown Gestor lista todos os usuários cadastrados, sem filtrar por perfil).
> Defina apenas um gestor por departamento, múltiplos gestores no mesmo departamento podem gerar notificações duplicadas.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Departamento EXISTENTE | → | 2️⃣ Usuário Gestor CRIADO | → | 3️⃣ Vínculo CRIADO | → | 📊 Notificações Ativas |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Tela Departamento — Gestor — listagem de vínculos com botão Novo](/screenshots/cadastro-departamento-gestor.png)


1. Acesse Cadastro Básico > Departamento, Gestor e clique em Novo.
1. Selecione a Unidade à qual o vínculo pertence.
1. Selecione o Departamento na lista suspensa.
1. Selecione o Gestor, usuário que assumirá a responsabilidade pelo departamento.
1. Atenção: o sistema não impede selecionar qualquer usuário, mesmo perfis operacionais como camareiras, a responsabilidade de escolher um gestor de fato é da pessoa que preenche o cadastro, não do sistema.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O gestor vinculado passa a receber notificações de OS críticas do departamento.
> Requisições de compra originadas no departamento são encaminhadas para aprovação deste gestor.
> Planos de Ação atribuídos ao departamento aparecem no painel do gestor.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade física onde o vínculo se aplica | Sim | Intercity Berrini |
| Departamento | Área cujo responsável está sendo definido | Sim | Manutenção |
| Gestor | Usuário responsável pelo departamento | Sim | João Silva (Gestor PCM) |


Mapeamento recomendado de gestores por departamento:

| Departamento | Perfil do gestor | O que o gestor recebe |
| :--- | :--- | :--- |
| Manutenção | Gestor de PCM | Alertas de OS crítica, aprovações de requisições técnicas, planos de ação de auditoria PCM |
| Governança | Governanta / Supervisora | Alertas de OS em quartos, notificações de enxoval crítico, planos de ação de qualidade |
| A&B | Gerente de A&B | Laudos sanitários próximos do vencimento, não conformidades de auditoria alimentar |
| Segurança | Supervisor de Segurança | OS de equipamentos de segurança, alertas de CCTV |
| Compras | Coordenador de Compras | Requisições aprovadas aguardando geração de OC, fornecedores com contrato vencendo |
| RH | Gerente de RH | Alertas de treinamentos vencidos, colaboradores sem função cadastrada |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Revise os vínculos de gestor a cada troca de liderança, um departamento com gestor desligado ou transferido fica sem receber notificações silenciosamente.
> Para unidades com equipe enxuta onde um gestor cuida de múltiplos departamentos, crie o vínculo em cada departamento separadamente, o mesmo usuário pode ser gestor de vários departamentos.
> Configure os gestores antes de entrar em operação plena, as primeiras OS críticas sem gestor vinculado passam sem notificação e podem criar a percepção de que o sistema ''não funciona''.


> [!DANGER]
> Se um gestor for desligado ou transferido, remova o vínculo imediatamente e cadastre o substituto. Durante o período sem gestor vinculado, alertas e aprovações ficam sem destinatário, o que pode gerar atrasos operacionais graves.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Gestor não recebe notificações de OS críticas | Vínculo gestor-departamento não criado ou e-mail incorreto | Verifique o vínculo em Cadastro Básico > Departamento — Gestor e confirme o e-mail |
| Departamento sem gestor após desligamento | Vínculo não foi atualizado com o substituto | Crie novo vínculo com o substituto imediatamente: sem gestor, nenhuma notificação é enviada |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Departamentos (seção 2.11)Administração > Usuários — perfil do gestor (seção 1.3) | OS: notificações de prioridade CríticaRequisições: fluxo de aprovação por departamentoPlano de Ação: tarefas atribuídas ao departamento | Notificações automáticas direcionadas ao gestor correto imediatamente após o vínculo |
| Perfil e hierarquia de acesso (seção 1.4) | Dashboard: indicadores filtrados por área de responsabilidadeRelatórios de desempenho: performance por departamento | Alertas de vencimento de laudos do departamento |', NULL, NULL, NULL, NULL, 1);

-- Criação de Checklists — Rotina, Preventiva e Governança [CadastroBasico/PreventivaIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'PreventivaIndex', N'Criação de Checklists — Rotina, Preventiva e Governança', N'2.13 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, Qualidade e Governança. Esta seção cobre os checklists usados por Rotina, Preventiva e Governança/U.H. em Dia. Os checklists de Auditoria e de PMOC usam telas e estrutura próprias: ver seções dedicadas a cada um. | Menu lateral > Cadastro Básico > Checklist pcmbysim.com.br/CadastroBasico/ChecklistIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de estruturar corretamente o cadastro que a SIM Services considera o motor de grande parte do sistema: é o Checklist que define as perguntas que o técnico ou a camareira respondem em campo, e um checklist mal configurado quebra silenciosamente o funcionamento de U.H. em Dia, Rotinas, Preventivas e Auditorias, sem gerar nenhum erro visível na hora do cadastro. Esta seção usa como base uma planilha real de checklist compartilhada pela SIM Services para destrinchar cada campo com precisão.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A unidade precisa estar cadastrada: seção 2.1.
> Defina os itens do checklist antes de cadastrar, recomendado revisar com o responsável técnico da área (PCM, Governança ou Qualidade).
> Tenha o modelo de importação em mãos se preferir usar o upload em planilha em vez de digitar item a item.
> O Tipo de Checklist do cabeçalho deve ser escolhido com cuidado, ele determina em qual módulo o checklist aparece para uso, e não pode ser alterado depois de criado.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Tipo DEFINIDO | → | 2️⃣ Grupos ORGANIZADOS | → | 3️⃣ Itens ADICIONADOS | → | 4️⃣ Checklist VINCULADO | → | 5️⃣ Resposta em CAMPO | → | 📊 Histórico Alimentado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Criar o cabeçalho do checklist

![Cadastro Básico > Checklist — formulário de criação com campos de tipo e descricao](/screenshots/cadastro-checklist-novo.png)


1. Acesse Cadastro Básico > Checklist e clique em Novo.
1. Selecione a Unidade.
1. Escolha o Tipo de Checklist, 9 opções reais confirmadas (Auditoria, Governança, PMOC, Preventiva, Qualidade, Rotina, Tarefa, Tudo em Dia, U.H. em Dia), mais do que as 4 sugeridas em versões antigas deste manual. Este campo não pode ser alterado após a criação.
1. Preencha a Descrição com um nome claro e padronizado (ex: ''Preventiva Mensal — Ar Condicionado Split'').
1. Clique em Salvar para criar o cabeçalho.


> [!INFO]
> **RESULTADO ESPERADO**
> O checklist é criado e aparece na listagem. Agora é necessário adicionar os itens de verificação, e é aqui que mora a parte mais importante e menos óbvia deste cadastro, detalhada nos passos a seguir.


### 4.2  Entender as duas camadas do cadastro, e a armadilha do nome ''Tipo de Checklist''

> [!DANGER]
> **''TIPO DE CHECKLIST'' TEM DOIS SIGNIFICADOS DIFERENTES NO SISTEMA**
> No cabeçalho do checklist (passo 4.1), ''Tipo de Checklist'' define o MÓDULO onde ele vai aparecer, Preventiva, Rotina, Governança etc. Quando esse Tipo é Preventiva ou Rotina, cada ITEM do checklist tem um campo próprio com o MESMO NOME ''Tipo de Checklist'' que define outra coisa completamente diferente: o FORMATO DE RESPOSTA daquele item específico (SIM/NÃO, NUMÉRICO, TEXTO...). São dois campos com o mesmo nome e finalidades totalmente diferentes dentro da mesma tela, confundir um pelo outro é a causa mais comum de checklist mal configurado. Quando o Tipo do cabeçalho é Governança, esse campo de item nem existe, ver passo 4.3.


Para checklists do Tipo Preventiva ou Rotina, cada item tem 11 campos próprios, preenchidos um por linha na planilha de importação (passo 4.6) ou diretamente na tela:


| Campo do item | O que faz | Exemplo real |
| :--- | :--- | :--- |
| Sequência Checklist | Código no formato Grupo.Item (GG.III): controla ordem de exibição e agrupamento visual | 04.003 |
| Tipo de Checklist (do item) | O FORMATO DE RESPOSTA daquele item: ver passo 4.5 | NUMÉRICO |
| Grupo | Agrupamento maior: a área ou sistema físico verificado | 04 - CÂMARAS PISO R |
| Subgrupo | Divisão dentro do Grupo: ver passo 4.4 | 02 - MEDIÇÕES |
| Item do Checklist | O texto da pergunta exibida ao técnico | CÂMARA 03 - DESCONGELAMENTO... QUAL A TEMPERATURA? [2°C A 4°C] |
| Valor Mínimo / Valor Máximo | Faixa aceitável da leitura: só relevante quando o Tipo é NUMÉRICO | 2.00 / 4.00 |
| Unidade Medida | Unidade exibida junto ao valor numérico | °C |
| Permite foto | Se o técnico pode/deve anexar foto como evidência daquele item | SIM |
| Gera Ordem de Serviço | Se SIM, o item marcado como Não Conforme abre uma OS automaticamente em PCM | NÃO |
| Auditado | Se SIM, o item entra no Relatório Dinâmico de Itens Auditáveis (seção 7.8) | SIM |


### 4.3  A estrutura mais simples do checklist de Governança

Quando o Tipo de Checklist do cabeçalho é Governança, o item usa uma estrutura bem mais enxuta, apenas 5 campos, sem o campo Tipo de resposta: todo item de Governança é sempre uma pergunta SIM/NÃO fixa, sem opção de NUMÉRICO, TEXTO, DATA ou HORA.


| Campo do item (Governança) | O que faz | Exemplo real |
| :--- | :--- | :--- |
| Sequência Checklist | Código Grupo.Item (GG.III) | 01.001 |
| Grupo | Área verificada | 01 - QUARTO |
| Subgrupo | Existe no cadastro, mas na prática costuma ficar em branco | — |
| Item do Checklist | Texto da pergunta, sempre respondida como SIM/NÃO | LIMPEZA, CONSERVAÇÃO E FUNCIONAMENTO DA PORTA DE ENTRADA |
| Permite foto | Se o técnico/camareira pode anexar foto | SIM |
| Gera Ordem de Serviço | Se SIM, item Não Conforme abre OS automaticamente | SIM |


> [!INFO]
> **É A MESMA ESTRUTURA DO U.H. EM DIA**
> Este é o cadastro por trás do checklist de conformidade técnica documentado na seção 5.1 (U.H. em Dia), cada Grupo ali corresponde a um cômodo da U.H. (ex.: ''01 - QUARTO'', ''02 - BANHEIRO''), e cada Item é uma verificação SIM/NÃO daquele cômodo.


### 4.4  Organizar itens com Grupo e Subgrupo

O Grupo é sempre um agrupamento MAIOR, a área ou sistema físico que está sendo verificado. O Subgrupo é uma divisão DENTRO desse grupo, usada para separar tipos diferentes de verificação no mesmo local, deixando as perguntas mais organizadas.


> [!INFO]
> **EXEMPLO REAL**
> Um Grupo ''Substação'' pode ter o Subgrupo ''Elétrica'' (onde ficam as perguntas sobre os quadros elétricos) e o Subgrupo ''Civil'' (onde ficam os detalhes de pisos e paredes), assim as perguntas ficam organizadas por natureza da verificação, mesmo estando todas dentro da mesma área física.


Na planilha template de Preventiva/Rotina compartilhada pela SIM Services, o padrão mais comum usa apenas 2 Subgrupos por Grupo: ''01 - OPERAÇÃO'' (para perguntas SIM/NÃO sobre funcionamento) e ''02 - MEDIÇÕES'' (para leituras NUMÉRICO com faixa aceitável), mas a estrutura permite quantos Subgrupos forem necessários, como no exemplo da Substação acima. Já na planilha de Governança, o Subgrupo existe mas não costuma ser usado (ver passo 4.3).

A Sequência Checklist usa o formato GG.III (Grupo.Item): os 2 primeiros dígitos identificam o Grupo e os 3 últimos são o número sequencial do item dentro dele, por isso 01.001, 01.002... pertencem ao Grupo 01, e 04.001, 04.002... pertencem ao Grupo 04.


### 4.5  Os formatos de resposta de cada item (Preventiva e Rotina)

| Tipo de Checklist (item) | O que o técnico vê em campo | Exemplo real da planilha |
| :--- | :--- | :--- |
| SIM / NÃO | Pergunta binária simples | EXAUSTOR (COZINHA CENTRAL) - ESTÁ FUNCIONANDO? |
| SIM / NÃO / N.A. | Binária com opção de ''não se aplica'' | LIMPEZA DE BORDAS DA PISCINA |
| NUMÉRICO | Leitura de medição, com Valor Mínimo/Máximo e Unidade de Medida | ÁGUA DA PISCINA (CLORO) [0,8 A 3 PPM]: mínimo 0,00, máximo 3,00, unidade PPM |
| TEXTO | Resposta livre em texto | QUANTOS EQUIPAMENTOS ESTÃO FUNCIONANDO |
| DATA | Campo de data | QUAL A DATA DA ÚLTIMA LIMPEZA |
| HORA | Campo de horário | QUAL FOI A HORA DA ÚLTIMA LIMPEZA |


> [!INFO]
> **IMPORTANTE SABER**
> A lista de seleção do sistema mostra mais de 6 opções, as demais são de uso interno do sistema (alimentam a funcionalidade de Discrepâncias) e não fazem parte do fluxo normal de criação de checklist. Use sempre um dos 6 formatos acima ao montar um checklist de Preventiva ou Rotina.


### 4.6  Adicionar itens, via upload de planilha (recomendado para listas longas)

1. Na tela do checklist criado, clique em Download de Modelo para baixar a planilha template, o modelo baixado muda conforme o Tipo de Checklist do cabeçalho (Preventiva/Rotina baixa o modelo de 11 colunas do passo 4.2; Governança baixa o modelo de 5 colunas do passo 4.3).
1. Preencha a planilha com os itens de verificação, uma linha por item.
1. Salve a planilha e retorne ao sistema. Clique em Upload de Arquivo e selecione o arquivo preenchido.
1. O sistema importa todos os itens de uma vez.


> [!INFO]
> **IMPORTANTE SABER**
> Exemplos de itens para checklist de Preventiva Mensal de AC Split:
> 1. Filtros, estado de conservação e necessidade de limpeza ou troca.
> 2. Evaporadora, verificar fixação, ausência de vibração e ruídos anormais.
> 3. Drenagem, verificar bandeja e dreno desobstruídos.
> 4. Condensadora, verificar grade de proteção e limpeza das aletas.
> 5. Gás refrigerante, verificar ausência de vazamentos (pressão nominal).
> 6. Tensão de operação, medir e registrar (Volts).
> 7. Corrente de operação, medir e registrar (Amperes).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade (cabeçalho) | Unidade onde o checklist será aplicado | Sim | PCM by SIM |
| Tipo de Checklist (cabeçalho) | Módulo de uso: 9 opções reais confirmadas | Sim | AUDITORIA / GOVERNANÇA / PMOC / PREVENTIVA / QUALIDADE / ROTINA / TAREFA / TUDO EM DIA / U.H. EM DIA |
| Descrição (cabeçalho) | Nome identificador do checklist | Sim | Preventiva Mensal: AC Split |
| Sequência Checklist (item) | Código Grupo.Item: ordem de exibição | Sim | 04.003 |
| Tipo de Checklist (item) | Formato de resposta do item: 6 opções relevantes. Só existe quando o cabeçalho é Preventiva ou Rotina; Governança não tem este campo (item sempre SIM/NÃO) | Preventiva/Rotina: Sim · Governança:: | SIM/NÃO, NUMÉRICO, TEXTO, DATA, HORA, SIM/NÃO/N.A. |
| Grupo (item) | Área ou sistema físico verificado | Sim | 04 - CÂMARAS PISO R |
| Subgrupo (item) | Divisão dentro do Grupo, por natureza da verificação | Sim em Preventiva/Rotina · costuma ficar vazio em Governança | 02 - MEDIÇÕES |
| Item do Checklist | Texto da pergunta | Sim | Água da Piscina (PH) [7,2 a 7,8] |
| Valor Mínimo / Máximo | Faixa aceitável: só para Tipo NUMÉRICO (Preventiva/Rotina) | Não | 0,8 / 3,0 |
| Unidade Medida | Unidade exibida junto ao valor: só para Tipo NUMÉRICO (Preventiva/Rotina) | Não | PPM |
| Permite foto | Habilita anexo de foto como evidência | Sim | SIM / NÃO |
| Gera Ordem de Serviço | Abre OS automaticamente se o item for Não Conforme | Sim | SIM / NÃO |
| Auditado | Inclui o item no Relatório Dinâmico de Itens Auditáveis (seção 7.8): só existe em Preventiva/Rotina | Preventiva/Rotina: Sim · Governança:: | SIM / NÃO |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Use no máximo 20 a 25 itens por checklist, listas maiores causam apontamento mecânico sem leitura real.
> Use o Grupo para a área física e o Subgrupo para separar a natureza da verificação dentro dela (ex.: Operação x Medições, ou Elétrica x Civil), isso organiza a tela do técnico em blocos lógicos.
> Decida deliberadamente o valor de Gera Ordem de Serviço e Auditado para cada item, não deixe no padrão sem avaliar: um item crítico marcado errado nesses dois campos não abre OS automaticamente nem aparece nas evidências de auditoria.
> Para itens críticos de segurança, coloque-os no início com instrução de parar o serviço se ''Não Conforme''.
> Revise os checklists anualmente e documente qualquer alteração com data e motivo.


> [!DANGER]
> Configurar o campo errado em qualquer um dos 11 campos do item não gera erro visível no cadastro, o checklist salva normalmente. O problema só aparece depois, silenciosamente: um Tipo de resposta errado trava o apontamento em campo, um Auditado = NÃO tira o item do Relatório Dinâmico (seção 7.8) sem aviso, e um Gera Ordem de Serviço mal configurado deixa uma Não Conformidade sem OS aberta. Revise cada item com atenção antes de publicar o checklist.
> Alterar os itens de um checklist já em uso não afeta os apontamentos já realizados, mas pode quebrar a comparação histórica entre um período e outro. Prefira criar um novo checklist versionado quando a mudança for estrutural.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O checklist não aparece para vinculação na preventiva ou rotina | O Tipo de Checklist do cabeçalho não corresponde ao módulo onde está tentando vincular | Verifique o Tipo cadastrado. Para preventivas use tipo ''Preventiva''; para rotinas use tipo ''Rotina'' |
| Um item aparece no checklist mas nunca no Relatório Dinâmico de Itens Auditáveis (seção 7.8) | O campo Auditado do item está como NÃO | Edite o item e marque Auditado = SIM |
| Uma Não Conformidade não gerou Ordem de Serviço automaticamente | O campo Gera Ordem de Serviço do item está como NÃO | Avalie se esse item deveria gerar OS e ajuste o campo |
| O técnico não consegue informar um valor numérico esperado | O item foi cadastrado com Tipo SIM/NÃO em vez de NUMÉRICO | Corrija o Tipo do item para NUMÉRICO e preencha Valor Mínimo/Máximo |
| Os itens importados não aparecem após o upload | A planilha não seguiu o formato do modelo ou foi salva em formato incompatível | Baixe novamente o modelo do sistema, preencha apenas as colunas indicadas e salve como .xlsx |
| O técnico não vê o checklist no app mobile | O checklist não foi vinculado ao plano de preventiva ou rotina | Acesse o plano em Cadastro Básico > Preventiva ou Rotina e vincule o checklist correto |
| Um checklist aparece duplicado na lista de seleção | Foi criado duas vezes com nomes ligeiramente diferentes | Inative o duplicado. Use sempre o filtro de Descrição para verificar se o checklist já existe antes de criar |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Tipo de U.H. e Tipo de AC (vinculam checklists) Cadastro Básico > Preventiva e Rotina (planos) | PCM > Preventiva — itens respondidos pelo técnico em campo PCM > Rotina — verificações de ronda Governança/U.H. em Dia: apontamento por U.H. | Checklist carregado automaticamente no app ao iniciar apontamento |
| Itens e estrutura do checklist, incluindo os campos Auditado e Gera Ordem de Serviço | Relatório Dinâmico de Itens Auditáveis (seção 7.8): itens marcados como Auditado PCM > Ordens de Serviço — abertura automática quando Gera Ordem de Serviço = SIM e o item é Não Conforme Plano de Ação: gerado automaticamente para itens Não Conformes | Foto e observação vinculadas a cada item no apontamento |', NULL, NULL, NULL, NULL, 1);

-- Prioridades e Regras de Notificação [CadastroBasico/PrioridadeIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'PrioridadeIndex', N'Prioridades e Regras de Notificação', N'2.14 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Administradores e gestores de PCM. Configurado antes da abertura das primeiras OS. | Menu lateral > Cadastro Básico > Prioridade pcmbysim.com.br/CadastroBasico/PrioridadeIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de definir os níveis de urgência das Ordens de Serviço e configurar alertas automaticos por e-mail para cada nível. A prioridade e o motor do SLA (Service Level Agreement): ela define o prazo de atendimento esperado, a cor visual da OS na listagem e quem deve ser notificado quando um chamado de alta urgência e aberto. Uma configuração de prioridades bem feita garante que os problemas criticos não passem despercebidos, independentemente de quem está de plantão.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A unidade precisa estar cadastrada: seção 2.1.
> Defina os e-mails dos gestores que receberao as notificações antes de configurar as prioridades.
> Discuta com a equipe quais níveis de urgência fazem sentido para a realidade operacional da unidade, níveis em excesso geram confusao.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Prioridade CRIADA | → | 2️⃣ OS ABERTA | → | 3️⃣ Cor na LISTAGEM | → | 4️⃣ SLA CALCULADO | → | 📊 Alerta por E-mail |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Cadastro Básico > Prioridade — listagem de prioridades com campos de notificação](/screenshots/cadastro-prioridade-listagem.png)


1. Acesse Cadastro Básico > Prioridade e clique em Novo.
1. Selecione a Unidade.
1. No campo Descrição, insira o nome do nível (ex: ''Crítica'', ''Alta'', ''Média'', ''Baixa'').
1. Marque o checkbox Envia E-mail? se este nível deve disparar notificações automaticas ao ser aberta uma OS.
1. Se ativou notificação, preencha o campo Lista E-mails com os enderecos dos gestores separados por ponto e virgula.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A prioridade aparece disponível na abertura de OS e nos planos de preventiva.
> Se configurada com e-mail, o disparo automático ocorre em ate 30 segundos após a abertura da OS.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade a qual a prioridade se aplica | Sim | Intercity Berrini |
| Descrição | Nome do nível de urgência | Sim | Crítica |
| Envia E-mail? | Ativa disparo automático de notificação ao abrir OS | Não | Marcar para Crítica e Alta |
| Lista E-mails | Destinatarios do alerta: separar por ponto e virgula | Cond. | gestor@hotel.com; diretor@hotel.com |
| Ativo | Disponibiliza a prioridade para seleção em OS | Sim | Sempre ativo |


> [!WARNING]
> **CORES E SLA NÃO SÃO CONFIGURÁVEIS, SÃO PADRÃO DO SISTEMA**
> O cadastro real de Prioridade só tem 5 campos: Unidade, Descrição, Envia e-mail?, Lista de E-mails e Ativo. Não existe nenhum campo para configurar Cor ou SLA/Prazo, e isso não é uma lacuna: o sistema usa cores e SLA padrão, iguais para todos os clientes, associados ao nome da prioridade, não é algo que cada unidade personaliza. A tabela abaixo mostra essa referência padrão do sistema, não uma sugestão editável.


Referência padrão de cores e SLA por nível de prioridade:

| Prioridade | Cor | SLA recomendado | Notificação | Quando usar |
| :--- | :--- | :--- | :--- | :--- |
| Crítica | Vermelho | Ate 1 hora | Sim: gestor + diretor | Risco de segurança, hóspede impactado agora, falha crítica de infraestrutura |
| Alta | Laranja | Ate 4 horas | Sim: gestor PCM | Equipamento principal com defeito, hóspede sem AC ou água quente |
| Média | Amarelo | Ate 24 horas | Não | Problemas que afetam operação mas não impedem uso imediato |
| Baixa | Verde | Ate 72 horas | Não | Melhorias esteticas, preventivos agendaveis, solicitações sem urgência |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Treine toda a equipe sobre qual prioridade usar em cada situacao antes de entrar em operação. A consistencia na classificação e o que garante que o SLA seja mensuravel.
> Configure no máximo 4 níveis de prioridade, acima disso a equipe fica em duvida na hora de classificar e acaba usando sempre ''Alta'' para tudo.
> Revise os e-mails de notificação a cada troca de lideranca, um gestor desligado ainda recebendo alertas criticos e uma falha de processo grave.


> [!DANGER]
> Excesso de notificações de prioridade Crítica causa fadiga de alertas, gestores comecam a ignorar os e-mails. Use Crítica apenas para situacoes que realmente exigem resposta em menos de 1 hora.
> O prazo de SLA calculado pelo Dashboard e baseado na prioridade selecionada na OS. Se a equipe usar prioridades incorretas, o SLA reportado fica distorcido e os relatorios de desempenho perdem credibilidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Os e-mails de notificação não estao chegando | Endereco incorreto no campo Lista E-mails ou problema de configuração de SMTP | Verifique se o e-mail está correto e sem espaços. Se correto, contate o suporte técnico para verificar a configuração de envio |
| A prioridade não aparece para seleção ao abrir uma OS | A prioridade está inativa | Acesse Cadastro Básico > Prioridade, localize a prioridade e ative o switch Ativo |
| O SLA no Dashboard não bate com o esperado | As OS estao sendo abertas com prioridades incorretas pela equipe | Realize uma sessao de treinamento com a equipe sobre os criterios de classificação. Revise as OS abertas na última semana e corrija as mal classificadas |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Unidades | OS: cor e prazo automático de SLA por prioridade Dashboard: cálculo de SLA e OS Atrasadas Requisicoes: nível de urgência na fila de aprovacao | Disparo de e-mail automático em menos de 30 segundos após abertura da OS crítica |
| Lista de e-mails configurados | Relatorios de BI: tempo médio de atendimento por prioridadePlanejamento de equipe: dimensionamento pelo volume de OS criticas | Cor visual da OS na listagem e no app mobile definida pela prioridade |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Fornecedores [CadastroBasico/FornecedorIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'FornecedorIndex', N'Cadastro de Fornecedores', N'2.15 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, Almoxarifado e Financeiro. | Menu lateral > Cadastro Básico > Fornecedor pcmbysim.com.br/CadastroBasico/FornecedorIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar e gerenciar a base de empresas parceiras que fornecem materiais ou prestam serviços para a unidade. O cadastro de fornecedores e a base do controle de suprimentos: ele vincula entradas de estoque a quem forneceu, permite identificar quem realizou uma manutenção externa e rastrear garantias de pecas e serviços. Um fornecedor não cadastrado não pode ser vinculado a nenhuma nota fiscal ou ordem de compra no sistema.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A unidade precisa estar cadastrada: seção 2.1.
> As Categorias de Serviço precisam estar criadas para classificar o ramo do fornecedor: seção 2.8.
> Tenha em maos: CNPJ, Razao Social, endereco e contato do fornecedor.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ CNPJ e Dados FISCAIS | → | 2️⃣ Categoria VINCULADA | → | 3️⃣ Fornecedor ATIVO | → | 4️⃣ Entrada de ESTOQUE | → | 📊 Ordem de Compra |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Formulário de cadastro de fornecedor — dados fiscais, endereco e classificação](/screenshots/cadastro-fornecedor-novo.png)


1. Acesse Cadastro Básico > Fornecedor e clique em Novo.
1. Selecione a Unidade a qual o fornecedor está vinculado.
1. Bloco A, Dados fiscais: preencha CNPJ, Inscricao Estadual/Municipal, Nome Fantasia e Razao Social.
1. Bloco B, Endereco e contato: preencha CEP, Logradouro, Número, Bairro, Complemento, Municipio, UF, E-mail e Telefone (Número e Complemento são campos reais separados, não documentados antes).
1. Bloco C, Classificação: selecione a Categoria, Serviço que define o ramo de atuacao do fornecedor.
1. Mantenha Ativo habilitado e clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O fornecedor aparece disponível para vinculacao em entradas de estoque, ordens de compra e OS externas.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade a qual o fornecedor está vinculado | Sim | PCM by SIM |
| CNPJ | Cadastro Nacional da Pessoa Juridica | Sim | 00.000.000/0001-00 |
| Nome Fantasia | Nome comercial do fornecedor | Sim | Engenharia e  Manutenção |
| Razao Social | Nome juridico da empresa | Sim | Eng e Mnt Ltda. |
| Insc. Est./Mun. | Registros fiscais complementares | Não | 123.456.789.000 |
| CEP / Endereco | Localização completa: inclui Número e Complemento como campos separados | Não | 01310-100, Av. Paulista, 1000 |
| E-mail | Contato para envio de OC e comunicacoes | Não | contato@engemnt.com.br |
| Telefone | Contato direto | Não | (11) 3000-0000 |
| Categoria: Serviço | Ramo de atuacao: facilita busca ao abrir OS externa | Sim | Refrigeracao / Elétrica / Civil |
| Ativo | Disponibiliza o fornecedor para vinculação | Sim | Sempre ativo |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Cadastre o fornecedor assim que fechar o contrato ou realizar a primeira compra, não deixe para depois. Entradas de estoque sem fornecedor vinculado perdem o histórico de origem.
> Use a Categoria de Serviço para classificar com precisão, um fornecedor de ''Refrigeracao'' deve ser diferente de ''Ar Condicionado Residencial''. A busca em OS externas depende dessa classificação.
> Mantenha o e-mail do fornecedor atualizado, o sistema usa esse campo para envio automático de Ordens de Compra quando configurado.


> [!DANGER]
> Inativar um fornecedor não remove o histórico de entradas e compras vinculadas a ele, mas impede novas vinculacoes. Inative apenas fornecedores encerrados ou com contrato cancelado definitivamente.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O fornecedor não aparece na busca ao registrar uma entrada de estoque | Fornecedor inativo ou vinculado a outra unidade | Edite o fornecedor: verifique Ativo e a Unidade vinculada |
| O sistema rejeita o CNPJ | CNPJ já cadastrado ou formato incorreto | Use sempre o formato com pontuacao: 00.000.000/0001-00. Verifique se o fornecedor já não existe na listagem |
| A Categoria de Serviço não aparece para seleção | A categoria não foi criada ou está inativa | Acesse Cadastro Básico > Categoria — Serviço (seção 2.8) e crie ou reative a categoria necessária |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Categoria de Serviço (classificação) Dados fiscais do fornecedor | Estoque > Entrada de Material — fornecedor vinculado a cada notaEstoque > Ordem de Compra — destinatario da OCOS Externas: prestador de serviço vinculado | Histórico de fornecimentos por CNPJ disponível no módulo de Estoque |
| Contratos de manutenção preventiva externa | Módulo Financeiro: controle de contratos e despesas por fornecedor Laudo e Documentação: fornecedor responsável pelo laudo técnico Histórico de Garantias por equipamento |  |', NULL, NULL, NULL, NULL, 1);

-- Produtos e Grupos de Estoque [CadastroBasico/ProdutoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'ProdutoIndex', N'Produtos e Grupos de Estoque', N'2.16 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de Almoxarifado e PCM. Realizado antes de iniciar movimentacoes de estoque. | Cadastro Básico > Produto / Grupo pcmbysim.com.br/CadastroBasico/ProdutoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de cadastrar os materiais, pecas e insumos que serao controlados no almoxarifado, e organiza-los em grupos logicos para análise financeira. O produto e a unidade basica do estoque: sem ele cadastrado, o técnico não consegue registrar os materiais usados em uma OS, e a baixa automática de estoque não acontece. Um catalogo de produtos bem estruturado e o que torna o custo de manutenção mensuravel com precisão.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A unidade precisa estar cadastrada: seção 2.1.
> Os Grupos de produto precisam existir antes dos produtos, crie-os primeiro.
> Defina a unidade de medida correta para cada produto, ela não pode ser alterada após movimentacoes.
> Tenha o código interno de cada produto para facilitar a busca rapida no almoxarifado.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Grupo CRIADO | → | 2️⃣ Produto CADASTRADO | → | 3️⃣ Ponto de Reposição DEFINIDO | → | 4️⃣ Entrada REGISTRADA | → | 5️⃣ Saída REGISTRADA | → | 📊 Saldo Atualizado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'1. Acesse Cadastro Básico > Grupo e clique em Novo.
1. Selecione a Unidade e preencha a Descrição do grupo (ex: ''Materiais Eletricos'', ''Pecas de Refrigeracao'', ''EPI'').
1. Clique em Salvar.


### 4.1 Cadastrar um Produto

![Cadastro Básico > Produto — formulário com código, grupo, unidade de medida e estoque mínimo](/screenshots/cadastro-produto-novo.png)


1. Acesse Cadastro Básico > Produto e clique em Novo.
1. Selecione a Unidade e o Grupo ao qual o produto pertence.
1. Preencha o Código Interno, identificador único para busca rapida no almoxarifado.
1. Preencha a Descrição completa do produto (ex: ''Filtro HEPA 300x300 G4 — Ar Condicionado Split'').
1. Selecione a Unidade de Medida (Unidade, Metro, Litro, Kg, Par, Caixa...).
1. Preencha o Ponto de Reposicao (Estoque Mínimo): quando o saldo atingir esse valor, o sistema gera alerta de recompra.
1. Ative Controla Lote e/ou Controla Validade se o produto exigir rastreamento por lote ou tem prazo de validade (campos reais não documentados antes, relevantes para itens de A&B ou produtos químicos).
1. Não existe campo de Preço Unitário de Referência neste formulário, apesar do que versões antigas deste manual indicavam, o preço vem da nota fiscal registrada em cada Entrada de Material (seção 4.1), recalculado como média a cada entrada.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O produto aparece disponível para seleção nas telas de Entrada, Saida e Inventario.
> A baixa de saldo ao usar o produto numa OS NÃO é automática, é sempre um passo manual em Estoque > Saída de Material, vinculando o número da OS (mesma pendência já documentada em §3.1 e §4.1).


| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade do almoxarifado | Sim | Intercity Berrini |
| Grupo | Categoria do produto para análise e relatorios | Sim | Materiais Eletricos |
| Código Interno | Identificador único para busca rapida | Sim | ELE-0042 |
| Descrição | Nome completo e especificacao do produto | Sim | Disjuntor Bipolar 20A Schneider |
| Unidade de Medida | Como o produto e contado e movimentado | Sim | Unidade / Metro / Litro / Kg |
| Ponto de Reposicao | Saldo mínimo: abaixo disso o sistema alerta | Não | 5 unidades |
| Controla Lote | Rastreamento por lote: campo real não documentado antes | Não | Ativar para produtos químicos |
| Controla Validade | Rastreamento de validade: campo real não documentado antes | Não | Ativar para itens de A&B |


Grupos de produto recomendados para almoxarifado de hotel:

| Grupo | Exemplos de produtos |
| :--- | :--- |
| Materiais Eletricos | Disjuntores, cabos, tomadas, laminas, fitas isolantes, reles |
| Pecas de Refrigeracao | Filtros de AC, gas refrigerante, capacitores, correias de condensadora |
| Materiais Hidraulicos | Tubos, conexões, velas de filtro, vedantes, torneiras |
| Ferramentas e EPI | Luvas isolantes, capacete, oculos, cinto de segurança, botas |
| Materiais de Construcao | Cimento, massa corrida, tinta, rejunte, fita crepe, lixas |
| Materiais de Limpeza | Desinfetantes, desentupidor, pano de limpeza, esponja abrasiva |
| Consumiveis de Escritorio | Pilhas, lapiseiras, cartuchos, papel, etiquetas |
| Lubrificantes e Quimicos | Oleo de motor, graxa, WD-40, fluido de freio, anticongelante |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Código Interno | Identificador único para busca rápida | Sim | ELE-0042 |
| Descrição | Nome completo e específicação do produto | Sim | Disjuntor Bipolar 20A |
| Unidade de Medida | Como o produto é contado e movimentado | Sim | Unidade / Metro / Litro |
| Ponto de Reposição | Saldo mínimo: abaixo disso o sistema alerta | Não | 5 unidades |
| Controla Lote | Rastreamento por lote: campo real não documentado antes | Não | Ativar para produtos químicos |
| Controla Validade | Rastreamento de validade: campo real não documentado antes | Não | Ativar para itens de A&B |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Use descrições específicas: ''Lâmpada LED 9W E27 Branca Fria'' é melhor que apenas ''Lâmpada''.
> A Unidade de Medida não pode ser alterada após a primeira movimentação, defina com cuidado.
> Configure o Ponto de Reposição para todos os itens críticos antes de iniciar a operação.


> [!DANGER]
> Nunca inative um produto que ainda tem saldo em estoque só para ''limpar'' a listagem, isso esconde o saldo do inventário sem baixá-lo, distorcendo a valorização do estoque.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O produto não aparece na tela de saida de estoque | Produto inativo, vinculado a outra unidade, ou saldo zerado e o filtro está ocultando | Verifique Ativo e Unidade. Remova o filtro de ''Saldo > 0'' se estiver ativo na tela de saida |
| O alerta de estoque mínimo não está disparando | Campo Ponto de Reposição está zerado ou em branco | Edite o produto e preencha o Ponto de Reposicao com o saldo mínimo desejado |
| O custo de material de uma OS não aparece em lugar nenhum | Não existe campo de Preço Unitário de Referência no produto, e o custo não volta a aparecer na própria OS hoje: o preço fica só no histórico de Entrada de Material | Filtre o histórico de Estoque > Saída de Material pelo número da OS para levantar o custo manualmente |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Grupo de Produto (agrupamento)Cadastro Básico > Fornecedor (origem das entradas) | Estoque > Entrada, Saida e Inventario OS — baixa é manual em Estoque > Saída de Material, não automática ao apontar materiais Requisição de Compra — lista itens abaixo do mínimo | Alerta de estoque mínimo gerado automaticamente |
| Preco médio (calculado a partir das Entradas) e grupo do produto | Módulo Financeiro: valorizacao do estoque Relatorios de BI: custo de material por OS e por equipamento |  |', NULL, NULL, NULL, NULL, 1);

-- Mapa de Manutenção e Itens Gerais [CadastroBasico/AtividadeIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'AtividadeIndex', N'Mapa de Manutenção e Itens Gerais', N'2.17 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM. Realizado antes de iniciar abertura de OS e preventivas. | Cadastro Básico > Mapa de Manutenção / Itens Gerais pcmbysim.com.br/CadastroBasico/AtividadeIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de criar dois catalogos complementares: o Mapa de Manutenção, que padroniza as descricoes de atividades de manutenção (evitando que cada técnico escreva o mesmo serviço de forma diferente), e os Itens Gerais, que mapeiam os objetos fisicos da unidade que não são máquinas nem equipamentos mas que precisam de manutenção, como moveis, luminárias, revestimentos e instalacoes decorativas. Juntos, eles transformam a abertura de OS em um processo padronizado e analisavel.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Os Itens Gerais precisam ser criados antes do Mapa de Manutenção.
> As Categorias de Serviço precisam estar configuradas: seção 2.8.
> Defina os itens do Mapa em conjunto com a equipe técnica.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Item Geral CRIADO | → | 2️⃣ Categoria DEFINIDA | → | 3️⃣ Atividade MAPEADA | → | 4️⃣ OS ABERTA | → | 📊 Categoria e Item |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Cadastro Básico > Mapa de Manutenção — combinacao de Categoria, Item Geral e Título da atividade](/screenshots/cadastro-mapa-manutencao-novo.png)


1. Acesse Cadastro Básico > Mapa de Manutenção e clique em Novo.
1. Selecione a Unidade e a Categoria, Serviço correspondente (ex: ''Civil'', ''Elétrica'').
1. Selecione o Item Geral relacionado a atividade (ex: ''Porta de Madeira''), hoje a lista real combina, por engano, os itens genéricos cadastrados em Itens Gerais (móveis, portas, extintores etc.) com os equipamentos com TAG do Cadastro Básico de Máquinas/Equipamentos e Ar Condicionado (ex.: ''A/C-0101 - AR CONDICIONADO'', ''BOMB-0006 - CIRCULAÇÃO BOMBA DE CALOR''). Esse comportamento é um bug confirmado (a lista deveria mostrar só os Itens Gerais) e já está reportado para correção, ver docs/bugs-sistema-pcmbysim.md.
1. No campo Título, descreva a atividade de forma clara e objetiva (ex: ''Regulagem e lubrificacao de dobradicas'').
1. Mantenha Ativo habilitado e clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A atividade aparece disponível para seleção ao abrir uma OS ou criar uma preventiva.
> O sistema preenche automaticamente a Categoria e o Item ao selecionar a atividade do mapa.


Itens Gerais:

| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade onde o item está instalado | Sim | PCM by SIM |
| Descrição | Nome do objeto físico sem motor ou equipamento | Sim | Porta de Madeira |
| Ativo | Disponibiliza o item para seleção em OS e Auditorias | Sim | Sempre ativo |


Mapa de Manutenção:

| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade onde a atividade ocorre | Sim | PCM by SIM |
| Categoria: Serv. | Especialidade tecnica da atividade | Sim | Civil / Elétrica / Hidraulica |
| Itens Gerais | Objeto que receberá a intervencao | Sim | Porta de Madeira |
| Título | Descrição padronizada da atividade | Sim | Regulagem e lubrificacao de dobradicas |


### 4.2  Cadastrar um Item OS Hóspede

1. Acesse Cadastro Básico > Item OS Hóspede e clique em Novo.
1. Preencha a Descrição do item que o hóspede pode solicitar (ex.: ''Troca de toalhas extra'', ''Travesseiro adicional'').
1. Mantenha Ativo habilitado e clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O item fica disponível na lista de seleção quando uma OS é aberta com origem ''Hóspede'' (seção 3.1).


### 4.3  Cadastrar uma Justificativa de Apontamento

1. Acesse Cadastro Básico > Justificativa, Apontamento e clique em Novo.
1. Preencha a Descrição da justificativa (ex.: ''Quarto ocupado durante a limpeza'', ''Falta de produto de limpeza'').
1. Mantenha Ativo habilitado e clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A justificativa fica disponível para a camareira selecionar ao registrar um apontamento (seção 5.1).


### 4.4  Cadastrar e consultar Treinamentos

1. Acesse Cadastro Básico > Treinamento e clique em Novo.
1. Selecione a Unidade e o Módulo relacionado (ex.: Manutenção, Governança).
1. Preencha a Descrição do treinamento e anexe o material.
1. Mantenha Ativo habilitado e clique em Salvar.


Depois de cadastrado, o colaborador acessa o treinamento pelo menu Treinamento, filtrando por Unidade e Módulo, e clica no card do treinamento desejado para abrir o material.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade onde a atividade ocorre | Sim | PCM by SIM |
| Categoria: Serviço | Especialidade técnica da atividade | Sim | Civil / Elétrica |
| Item Geral | Objeto que receberá a intervenção | Sim | Porta de Madeira |
| Título | Descrição padronizada da atividade | Sim | Regulagem e lubrificação de dobradiças |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Crie os Itens Gerais e o Mapa antes de entrar em operação, padronizar retroativamente é trabalhoso.
> Envolva os técnicos na criação do Mapa, eles conhecem os serviços mais frequentes.
> Limite o Título das atividades a 60 caracteres, descrições longas ficam cortadas nos relatórios.


> [!DANGER]
> Não crie uma nova Atividade do Mapa de Manutenção para cada variação mínima de texto, isso pulveriza o histórico de OS por descrições quase idênticas e prejudica a análise de recorrência de falhas.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Atividade não aparece ao abrir uma OS | Atividade inativa ou de outra unidade | Acesse Cadastro Básico > Mapa de Manutenção, verifique o status e a unidade |
| Técnico não encontra a atividade correta na lista | Lista muito extensa ou nomes parecidos | Revise e consolide atividades duplicadas com nomes mais específicos e padronizados |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Categoria de Serviço (seção 2.8)Cadastro Básico > Itens Gerais | OS: campo Descrição preenchido com atividade do mapaPreventiva: atividades padronizadas no plano | Selecionar atividade do mapa preenche automaticamente Categoria e Item na OS |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Checklist de PMOC — Periodicidade por Item [PMOC/PMOCIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'PMOC', N'PMOCIndex', N'Cadastro de Checklist de PMOC — Periodicidade por Item', N'2.18 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM e engenheiros responsáveis pelo PMOC. | Cadastro Básico > Checklist, com Tipo de Checklist = PMOC — mesma tela do cadastro geral de checklists (seção 2.13), mas com uma estrutura de item própria. |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de configurar o checklist técnico que alimenta o módulo PMOC (seção 3.6), incluindo o recurso mais particular desta estrutura: cada item pode ter sua própria Periodicidade, independente do ciclo geral do checklist. Isso permite, por exemplo, que a limpeza de filtro seja cobrada todo mês enquanto a aplicação de bactericida do mesmo equipamento só seja cobrada a cada 3 meses, dentro do mesmo checklist.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A unidade e os equipamentos de Ar Condicionado precisam estar cadastrados (seção 2.6).
> Esta estrutura é diferente da usada em Rotina/Preventiva/Governança (seção 2.13) e de Auditoria (seção 5.7), o PMOC troca os campos Auditado/Gera Ordem de Serviço por Periodicidade/Intervalo, específicos deste tipo de checklist.
> Esta seção usa como base uma planilha real de checklist de PMOC (equipamento Split) compartilhada pela SIM Services.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Item CRIADO | → | 2️⃣ Periodicidade DEFINIDA | → | 3️⃣ Checklist VINCULADO | → | 4️⃣ Cronograma GERADO | → | 📊 Execução pelo Técnico |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  A estrutura do item de checklist PMOC

O item de um checklist de PMOC tem 11 campos:


| Campo do item | O que faz | Exemplo real |
| :--- | :--- | :--- |
| Sequência Checklist | Código Grupo.Item (GG.III) | 01.005 |
| Tipo de Checklist (do item) | Formato de resposta: ver passo 4.3 | NUMÉRICO |
| Grupo | Tipo de equipamento verificado | 01 - SPLIT |
| Subgrupo | Existe no cadastro, mas na prática costuma ficar em branco | — |
| Item do Checklist | Texto da pergunta | MEDIR TEMPERATURA DE RETORNO - °C |
| Valor Mínimo / Valor Máximo | Faixa aceitável: só para Tipo NUMÉRICO | 23.00 / 26.00 |
| Unidade Medida | Unidade: só para Tipo NUMÉRICO | °C |
| Permite foto | Habilita anexo de foto como evidência | NÃO |
| Periodicidade | Ciclo de retorno do item: ver passo 4.2 | MÊS |
| Intervalo | Multiplicador da Periodicidade: ver passo 4.2 | 1 |


### 4.2  Periodicidade por item, o campo mais importante deste checklist

Diferente de Rotina, Preventiva, Governança e Auditoria, o checklist de PMOC permite definir de quanto em quanto tempo CADA PERGUNTA volta a ser cobrada, não é um ciclo único para o checklist inteiro.


| Periodicidade | O que significa | Exemplo real |
| :--- | :--- | :--- |
| CHECKLIST | O item NÃO tem ciclo próprio: segue o padrão geral configurado para o checklist (o mesmo conceito de Periodicidade cadastrado em Tipo de Ar Condicionado, seção 2.6) | Usar quando o item deve seguir o ritmo padrão do PMOC do equipamento |
| DIA | Item retorna a cada X dias, onde X é o Intervalo | — |
| SEMANAS | Item retorna a cada X semanas | — |
| MÊS | Item retorna a cada X meses | Intervalo 1 = mensal |


> [!INFO]
> **EXEMPLO REAL DA PLANILHA**
> No checklist de Split analisado, 8 dos 9 itens usam Periodicidade MÊS com Intervalo 1 (cobrados todo mês), mas o item ''APLICAÇÃO DE BACTERICIDA'' usa Periodicidade MÊS com Intervalo 3 (cobrado a cada 3 meses, trimestral). Mesmo equipamento, mesmo checklist, ciclos diferentes por pergunta.



### 4.3  Os formatos de resposta de cada item

| Tipo de Checklist (item) | Exemplo real da planilha |
| :--- | :--- |
| SIM / NÃO | ELIMINAR SUJEIRA, DANOS E CORROSÃO NA UNIDADE |
| NUMÉRICO | MEDIR TEMPERATURA DE RETORNO - °C |
| TEXTO | Resposta livre em texto |
| DATA | Campo de data |
| HORA | Campo de horário |
| SIM / NÃO / N.A. | Binária com opção de ''não se aplica'' |


### 4.4  Adicionar itens, via upload de planilha

1. Na tela do checklist criado com Tipo = PMOC, clique em Download de Modelo para baixar a planilha template.
1. Preencha a planilha com os itens, definindo a Periodicidade e o Intervalo de cada linha.
1. Salve a planilha e retorne ao sistema. Clique em Upload de Arquivo e selecione o arquivo preenchido.
1. O sistema importa todos os itens de uma vez.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Sequência Checklist | Código Grupo.Item | Sim | 01.005 |
| Tipo de Checklist (item) | Formato de resposta: 6 opções reais | Sim | NUMÉRICO |
| Grupo | Tipo de equipamento verificado | Sim | 01 - SPLIT |
| Item do Checklist | Texto da pergunta | Sim | MEDIR TEMPERATURA DE RETORNO - °C |
| Valor Mínimo / Máximo | Faixa aceitável: só para Tipo NUMÉRICO | Não | 23.00 / 26.00 |
| Unidade Medida | Unidade: só para Tipo NUMÉRICO | Não | °C |
| Permite foto | Habilita anexo de foto como evidência | Sim | SIM / NÃO |
| Periodicidade | Ciclo de retorno do item: CHECKLIST/DIA/SEMANAS/MÊS | Sim | MÊS |
| Intervalo | Multiplicador numérico da Periodicidade | Sim | 3 (a cada 3 meses) |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Use Periodicidade própria só quando o item realmente precisar de um ciclo diferente do padrão do equipamento (ex.: bactericida trimestral dentro de um PMOC mensal), para o restante, deixe como CHECKLIST para herdar o padrão e simplificar a manutenção do cadastro.
> Documente por que um item tem Intervalo diferente do padrão, facilita auditoria futura do próprio checklist.


> [!DANGER]
> Configurar a Periodicidade errada num item não gera erro visível no cadastro, o item pode passar a ser cobrado com uma frequência maior ou menor do que a exigida legalmente (Lei 13.589/2018, seção 3.6), sem que ninguém perceba até uma fiscalização.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Um item aparece no checklist do técnico com mais frequência do que deveria | Periodicidade/Intervalo do item configurados errados, ou item deveria estar como CHECKLIST | Edite o item e ajuste Periodicidade e Intervalo |
| O técnico não consegue informar um valor numérico esperado | O item foi cadastrado com Tipo SIM/NÃO em vez de NUMÉRICO | Corrija o Tipo do item para NUMÉRICO e preencha Valor Mínimo/Máximo |
| Os itens importados não aparecem após o upload | A planilha não seguiu o modelo de 11 colunas específico de PMOC | Baixe novamente o modelo com Tipo de Checklist = PMOC selecionado antes de exportar |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Ar Condicionado e Tipo de Ar Condicionado (seção 2.6) | PMOC: Cronograma e execução técnica (seção 3.6) | Cronograma recalculado conforme a Periodicidade de cada item |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Setores e Áreas [CadastroBasico/SetorIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'SetorIndex', N'Cadastro de Setores e Áreas', N'2.2 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM e administradores. Realizado logo após o cadastro da unidade. | Menu lateral > Cadastro Básico > Setor pcmbysim.com.br/CadastroBasico/SetorIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de definir a estrutura física da unidade em setores e sub-locais, criando a hierarquia de localização que organiza todos os ativos, OS e manutenções preventivas. Um mapa de setores bem construído faz com que o técnico saiba exatamente onde ir quando recebe uma OS, e permite ao gestor analisar quais áreas da unidade geram mais chamados de manutenção.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A unidade precisa estar cadastrada e ativa antes de criar setores: seção 2.1.
> Planeje a estrutura de setores antes de começar, uma hierarquia mal definida é difícil de corrigir depois sem impactar os cadastros vinculados.
> Tenha em mãos a planta da unidade ou um mapa de pavimentos para definir os nomes de forma padronizada e consistente.


> [!INFO]
> **IMPORTANTE SABER**
> Hierarquia recomendada de localização no PCM by SIM:
> Unidade  →  Setor  →  Local  →  U.H. (se aplicável)
> Exemplo: Intercity Berrini → 3º Andar → Corredor Norte → UH 302
> Setores são as divisões macro (andares, blocos, áreas funcionais). Locais são sub-divisões dentro de um setor (corredor, sala, salão).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Unidade SELECIONADA | → | 2️⃣ Setor CRIADO | → | 3️⃣ Locais ADICIONADOS | → | 4️⃣ Infraestrutura PREENCHIDA | → | 📊 Disponível no Sistema |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Criar um novo setor

> [!INFO]
> **Tela Cadastro Básico > Setor, formulário com campos de identificação e dados de infraestrutura**
> 


1. Acesse Cadastro Básico > Setor e clique em Novo.
1. Selecione a Unidade à qual o setor pertence.
1. No campo Descrição, use um nome padronizado e sem ambiguidade. Exemplos de boas práticas:


Use: ''1º Andar'', ''2º Andar''... em vez de ''Primeiro andar'', ''Andar 1''.

Use: ''Cozinha Industrial'', ''Restaurante'', ''Sala de Reunião A''.

Evite abreviações internas que só fazem sentido para uma pessoa.

1. Preencha o campo Observação com detalhes adicionais sobre o setor (ex: ''Inclui corredor norte e sul, exceto sala técnica'').
1. Preencha os dados de infraestrutura:


Metragem, área do setor em m² (campo real, não documentado antes).

Carga Térmica (BTU ou TR), capacidade total de climatização do setor.

Nº Pessoas Fixas, colaboradores permanentes no local.

Nº Pessoas Volantes, estimativa de circulação diária.

Descrição da Atividade, texto livre sobre o que acontece no setor (campo real, não documentado antes).

1. Mantenha o switch Ativo habilitado.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O setor aparece disponível para seleção em cadastros de equipamentos, OS e preventivas.


### 4.2  Adicionar locais dentro de um setor

![Setor cadastrado — área de Locais com botão + para adicionar sub-localizações](/screenshots/cadastro-setor-locais.png)


1. Na tela de edição do setor, role até a área Local.
1. Clique no botão + para adicionar um novo local ao setor.
1. Preencha a Descrição do local (ex: ''Sala 301'', ''Copa'', ''Casa de Máquinas'').
1. Repita o processo para todos os locais do setor e clique em Salvar.


> [!INFO]
> **IMPORTANTE SABER**
> Os locais aparecem como sub-opções quando o usuário seleciona o setor em uma OS ou preventiva, permitindo a localização precisa do problema ou ativo.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade à qual o setor pertence | Sim | Intercity Berrini |
| Descrição | Nome padronizado do setor | Sim | 3º Andar |
| Observação | Detalhes sobre abrangência ou restrições do setor | Não | Inclui corredor norte e sul |
| Metragem | Área do setor em m² | Não | 120 m² |
| Carga Térmica | Capacidade total de climatização (BTU ou TR) | Não | 48.000 BTU |
| Nº Pessoas Fixas | Colaboradores permanentes no setor | Não | 5 |
| Nº Pessoas Volantes | Estimativa de circulação diária | Não | 50 |
| Descrição da Atividade | Texto livre sobre o que acontece no setor | Não | Recepção e check-in de hóspedes |
| Ativo | Disponibiliza o setor para seleção em outros módulos | Sim | Sempre ativo |
| Local (sub) | Sub-localização dentro do setor | Não | Sala 301, Copa, Casa de Máquinas |


Exemplos de estrutura de setores por tipo de unidade:

| Tipo de unidade | Setores recomendados |
| :--- | :--- |
| Hotel | Recepção, Lobby, Restaurante, Cozinha, Pavimentos (1º ao Nº), Piscina, Academia, Área Técnica, Estacionamento, Cobertura |
| Hospital | UTI, Centro Cirúrgico, Pronto Atendimento, Internação (por ala), Farmácia, Laboratório, Zeladoria, Área Técnica |
| Corporativo | Térreo, Andares (1º ao Nº), Auditório, Salas de Reunião, Copa, TI, Estacionamento, Área Externa |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Crie os setores antes de cadastrar qualquer equipamento ou U.H., a ordem correta é: Unidade → Setores → U.H. / Equipamentos.
> Use nomes de setores consistentes com os que a equipe já usa no dia a dia. Se todo mundo chama de ''Casa de Máquinas'', cadastre exatamente assim, não crie um nome técnico que ninguém vai lembrar.
> Preencha a Carga Térmica mesmo que seja uma estimativa, o PMOC usa esse dado para calcular a necessidade de renovação de ar por setor.


> [!DANGER]
> Inativar um setor não exclui os equipamentos e OS vinculados a ele, mas o setor some dos filtros de novas OS e preventivas. Antes de inativar, verifique se há ativos ativos cadastrados naquele setor.
> Evite criar setores genéricos como ''Outros'' ou ''Geral''. Eles acumulam registros difíceis de rastrear e impossibilitam análises de onde estão concentrados os problemas de manutenção.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O setor não aparece para seleção ao abrir uma OS | O setor está inativo ou pertence a outra unidade | Verifique se o switch Ativo está habilitado e se a Unidade do setor coincide com a Unidade da OS |
| Os locais cadastrados não aparecem ao selecionar o setor numa OS | Os locais foram criados mas não salvos corretamente | Edite o setor, verifique se os locais aparecem na listagem interna e salve novamente |
| A carga térmica do setor não está sendo considerada no PMOC | O campo Carga Térmica foi deixado em branco ou com valor zero | Edite o setor e preencha a carga térmica com o valor real ou estimado em BTU/TR |
| Dois setores têm o mesmo nome: confusão na abertura de OS | Setores criados sem padronização de nomenclatura | Edite os setores para diferenciar os nomes: acrescente o bloco ou andar ao nome (ex: ''1º Andar: Bloco A'') |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Unidades (seção 2.1) | Cadastro Básico > Equipamentos e Ar CondicionadoCadastro Básico > U.H.Ordens de Serviço — campo Setor | Setor disponível imediatamente nos filtros de OS, Preventiva e Rotina |
| Dados de carga térmica e ocupação | PMOC: cálculo de renovação de ar e dimensionamentoRelatórios: volume de OS por setorDashboard: mapa de ocorrências por área | Filtro de setor em todos os relatórios de desempenho |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de U.H. (Unidades Habitacionais) [CadastroBasico/UnidadeIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'UnidadeIndex', N'Cadastro de U.H. (Unidades Habitacionais)', N'2.3 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, Governança e administradores. Essencial para operações hoteleiras e hospitalares. | Menu lateral > Cadastro Básico > U.H. pcmbysim.com.br/CadastroBasico/ApartamentoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de cadastrar e configurar cada quarto, apartamento ou suíte da unidade com suas características físicas e operacionais. O cadastro de U.H. é o que conecta a manutenção à hospitalidade: permite abrir OS para um quarto específico, rastrear o histórico de problemas por apartamento, controlar o status de limpeza em tempo real pela Governança e calcular a carga térmica do PMOC quarto a quarto.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A unidade precisa estar cadastrada: seção 2.1.
> Os setores físicos (andares, blocos) precisam estar cadastrados: seção 2.2.
> Os Tipos de U.H. precisam estar configurados para vinculação dos checklists: seção 2.4.
> Tenha em mãos a planta do hotel com a numeração exata de cada quarto, tipo e metragem.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Setor CADASTRADO | → | 2️⃣ Tipo de U.H. DEFINIDO | → | 3️⃣ U.H. CRIADA | → | 4️⃣ Características TÉCNICAS | → | 📊 U.H. Salva |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Cadastrar uma U.H.

![Formulário de cadastro de U.H. — campos de identificação, localização e características técnicas](/screenshots/cadastro-uh-novo.png)


1. Acesse Cadastro Básico > U.H. e clique em Novo.
1. Selecione a Unidade e o Setor (ex: ''5º Andar'') ao qual a U.H. pertence.
1. No campo U.H., insira o identificador único do quarto, geralmente o número (ex: 501, 502A) ou uma descrição (ex: Suíte Master).
1. Preencha Bloco e Andar para localização física precisa dentro da unidade.
1. Selecione o Tipo de U.H., o dropdown real mostra códigos curtos (ex.: STANDARD, LUX, PNE), definidos em Cadastro Básico > Tipo de U.H. (seção 2.4), não nomes completos como Luxo ou PCD por extenso. Este campo vincula os checklists de limpeza e manutenção do tipo.
1. Preencha as características técnicas:


Tipo da Cama e Nº de Camas, orienta a Governança no enxoval correto.

Descritivo, campo de texto livre real (função não totalmente confirmada, provavelmente observações adicionais sobre a U.H.).

Metragem (m²), área do quarto.

Carga Térmica (BTU), capacidade de climatização necessária para o PMOC.

Nº Pessoas Fixas e Nº Pessoas Volantes, no formulário real são 2 campos separados, não um único ''Nº Pessoas''.

Descrição da Atividade, texto livre (mesmo nome de campo visto também no cadastro de Setor).

Data Última Vistoria, campo de data real, não documentado antes.

1. Mantenha o switch Ativo habilitado. O campo Responsável (Pool de Locação/Proprietário/Uso Próprio), que aparece como coluna na listagem, não é editável neste formulário, ele é configurado pela equipe da SIM Services no ambiente de desenvolvimento, a pedido do cliente. Se sua unidade precisa alterar o Responsável de uma U.H., solicite à SIM Services em vez de procurar o campo aqui.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A U.H. aparece disponível para seleção em OS, Governança e no cronograma de preventivas.
> O status inicial da U.H. no módulo de Governança é definido automaticamente.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade hoteleira à qual o quarto pertence | Sim | Intercity Berrini |
| Setor | Agrupamento físico: geralmente o andar | Sim | 5º Andar |
| U.H. | Número ou código único do quarto | Sim | 501 |
| Bloco | Bloco da edificação (se houver mais de um) | Não | Bloco A |
| Andar | Pavimento: usado em relatórios e PMOC | Sim | 5 |
| Tipo de U.H. | Categoria comercial do quarto: vincula checklists | Sim | Standard Casal |
| Tipo da Cama | Configuração de cama: orienta o enxoval | Sim | King Size |
| Nº Camas | Quantidade de camas no quarto | Sim | 1 |
| Metragem (m²) | Área total da U.H. | Não | 28 m² |
| Carga Térmica | Capacidade de climatização necessária: alimenta PMOC | Não | 9.000 BTU |
| Nº Pessoas Fixas | Capacidade de ocupação fixa: campo real, separado de Volantes | Não | 2 |
| Nº Pessoas Volantes | Ocupação adicional/variável: campo real, separado de Fixas | Não | 1 |
| Descritivo | Texto livre: função exata não totalmente confirmada | Não | — |
| Descrição da Atividade | Texto livre sobre a U.H. | Não | — |
| Data Última Vistoria | Data da última vistoria registrada | Não | 10/08/2026 |
| Responsável | Aparece como coluna na listagem; configurado pela SIM Services no ambiente de desenvolvimento a pedido do cliente, não editável neste formulário | --- | Pool de Locação |
| Ativo | Disponibiliza a U.H. para OS, Governança e PMOC | Sim | Sempre ativo |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Cadastre as U.H.s na mesma ordem da planta, do menor para o maior número por andar. Isso facilita auditorias físicas e a distribuição de tarefas para a equipe de Governança.
> Preencha a Carga Térmica com o valor do equipamento de AC instalado no quarto, este dado é fundamental para que o PMOC seja calculado com precisão.
> Se a unidade tiver quartos para PCD (Pessoa com Deficiência), crie um Tipo de U.H. específico, esses quartos têm checklists de vistoria diferentes (rampas, barras de apoio, espaço de manobra).


> [!DANGER]
> Inativar uma U.H. a retira do módulo de Governança e bloqueia a abertura de novas OS para ela. Use esta opção apenas para quartos permanentemente fora de operação (reforma ou desativação).
> O número da U.H. deve ser exatamente igual ao número do quarto físico. Qualquer divergência gera confusão operacional, camareira no quarto 501 apontando para a UH 510, por exemplo.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A U.H. não aparece no módulo de Governança para a camareira | Switch Ativo desligado ou Tipo de U.H. não configurado com checklist de Governança | Verifique o switch Ativo e confirme que o Tipo de U.H. tem checklists de Governança vinculados (seção 2.4) |
| A carga térmica da U.H. não aparece no PMOC | Campo Carga Térmica foi deixado em branco | Edite a U.H., preencha a Carga Térmica com o valor do equipamento instalado e salve |
| Dois quartos têm o mesmo número cadastrado | Erro de digitação durante o cadastro em lote | Edite um dos quartos e corrija o identificador: o sistema permite números iguais em setores diferentes, mas é uma má prática |
| O tipo da cama não aparece na listagem de seleção | O Tipo de Cama não foi cadastrado ou está inativo | Acesse Cadastro Básico > Tipo da Cama, crie o tipo necessário e reabra o cadastro da U.H. |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Setor (localização física)Cadastro Básico > Tipo de U.H. (checklists) | Governança: status de limpeza e apontamento de camareiraOS: abertura de chamado por quarto específicoPMOC: carga térmica por U.H. | Status inicial de Governança definido automaticamenteU.H. disponível nos filtros de OS imediatamente |
| Carga térmica e tipo de cama | Relatório de inventário de quartosDashboard: indicador U.H. em Dia (Painel de Controle)Módulo Financeiro: rateio por U.H. | Checklist de limpeza carregado automaticamente no app da camareira pelo Tipo de U.H. |', NULL, NULL, NULL, NULL, 1);

-- Tipos de U.H., Cama e Ar Condicionado [CadastroBasico/TipoArCondicionadoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'TipoArCondicionadoIndex', N'Tipos de U.H., Cama e Ar Condicionado', N'2.4 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Administradores e gestores de PCM e Governança. Cadastro feito antes das U.H.s e equipamentos. | Cadastro Básico > Tipo de U.H. / Tipo da Cama / Tipo de Ar Condicionado pcmbysim.com.br/CadastroBasico/TipoApartamentoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de configurar as categorias que dão inteligência ao sistema: o Tipo de U.H. define quais checklists cada quarto usa; o Tipo de Cama orienta o enxoval correto para cada categoria; e o Tipo de Ar Condicionado define a periodicidade e o checklist técnico de manutenção de cada aparelho. Esses três cadastros são multiplicadores, você define uma vez e o sistema replica para centenas de quartos e equipamentos automaticamente.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Os checklists precisam estar criados antes de vinculá-los aos tipos: Cadastro Básico > Checklist.
> Defina os nomes dos tipos em reunião com as equipes de PCM e Governança, mudanças depois impactam todos os registros vinculados.
> Para Tipo de Ar Condicionado, tenha em mãos o manual do fabricante para definir a periodicidade correta de cada categoria.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Checklists CRIADOS | → | 2️⃣ Tipo de U.H. DEFINIDO | → | 3️⃣ Tipo de Cama DEFINIDO | → | 4️⃣ Tipo de AC DEFINIDO | → | 5️⃣ U.H. CADASTRADA | → | 📊 AC Cadastrado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Configurar Tipo de U.H.

![Cadastro Básico > Tipo de U.H. — campos de descrição e seleção de checklists](/screenshots/cadastro-tipo-uh-novo.png)


1. Acesse Cadastro Básico > Tipo de U.H. e clique em Novo.
1. No campo Descrição, insira o nome da categoria (ex: ''Standard Casal'', ''Suíte Luxo'', ''PCD'').
1. Selecione a Unidade (o Tipo de U.H. é cadastrado por unidade, não é global).
1. Vincule os checklists, o formulário real tem 7 vínculos de checklist, não 4:


Checklist, U.H. em Dia: roteiro de inspeção técnica preventiva do quarto. Este item também tem Periodicidade e Intervalo próprios (campos reais não documentados antes) para definir a frequência da inspeção.

Governança, Permanência: limpeza de quarto ocupado (hóspede mantém o quarto).

Governança, Saída: limpeza profunda após check-out.

Governança, Manutenção: vistoria técnica para liberação após reparo.

Governança, Vistoria Permanência: checklist de conferência da limpeza de permanência (campo real não documentado antes).

Governança, Vistoria Saída: checklist de conferência da limpeza de saída (campo real não documentado antes).

Governança, Vistoria Manutenção: checklist de conferência pós-manutenção (campo real não documentado antes).

1. Mantenha o switch Ativo habilitado.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O Tipo de U.H. aparece disponível para seleção no cadastro de quartos.
> O app da camareira carregará automaticamente o checklist correto ao iniciar a limpeza de um quarto deste tipo.


### 4.2  Configurar Tipo de Cama

1. Acesse Cadastro Básico > Tipo da Cama e clique em Novo.
1. Preencha a Descrição com o nome da configuração (ex: ''King Size'', ''Casal Padrão'', ''Twin, 2 Solteiros'').
1. Selecione a Unidade (o Tipo de Cama também é cadastrado por unidade).
1. Mantenha Ativo habilitado e clique em Salvar.


### 4.3  Configurar Tipo de Ar Condicionado

![Cadastro Básico > Tipo de Ar Condicionado — campos de periodicidade e checklist técnico](/screenshots/cadastro-tipo-ac-novo.png)


1. Acesse Cadastro Básico > Tipo de Ar Condicionado e clique em Novo.
1. No campo Tipo, insira o nome técnico da categoria (ex: ''Split Hi-Wall'', ''Fan Coil'', ''Chiller'').
1. Preencha a Descrição com detalhes adicionais sobre a aplicação do equipamento.
1. Defina a Periodicidade: no formulário real é um par de campos, uma unidade (Dia/Mês/Semanas) mais um número em Intervalo, não um texto livre como Mensal. Ex.: Mês + 1 = mensal.
1. Selecione o Checklist técnico que o técnico deverá preencher durante a manutenção deste tipo de AC.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O Tipo de AC aparece disponível no cadastro de equipamentos de climatização.
> O PMOC gerará automaticamente o cronograma de manutenção com a periodicidade configurada para cada aparelho deste tipo.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'Tipo de U.H.:

| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade à qual o Tipo de U.H. pertence: cadastro não é global | Sim | HOTEL BY SIM SERVICES |
| Descrição | Nome da categoria do quarto | Sim | Standard Casal |
| Checklist U.H. em Dia | Roteiro de inspeção técnica preventiva | Sim | Checklist Técnico Standard |
| Periodicidade / Intervalo (U.H. em Dia) | Frequência da inspeção: número + unidade (Dia/Mês/Semanas), campo real não documentado antes | Não | Mês(1) |
| Gov.: Permanência | Checklist para limpeza de quarto ocupado | Sim | Limpeza Permanência Padrão |
| Gov.: Saída | Checklist para limpeza profunda pós check-out | Sim | Limpeza Saída Padrão |
| Gov.: Manutenção | Checklist de vistoria pós-manutenção | Não | Vistoria Técnica |
| Gov.: Vistoria Permanência | Checklist de conferência da limpeza de permanência: campo real não documentado antes | Não | Vistoria Permanência Padrão |
| Gov.: Vistoria Saída | Checklist de conferência da limpeza de saída: campo real não documentado antes | Não | Vistoria Saída Padrão |
| Gov.: Vistoria Manutenção | Checklist de conferência pós-manutenção: campo real não documentado antes | Não | Vistoria Manutenção Padrão |
| Ativo | Disponibiliza o tipo para seleção: campo real não documentado antes | Sim | Sempre ativo |


Tipo de Ar Condicionado:

| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Tipo | Nome técnico da categoria de equipamento | Sim | Split Hi-Wall |
| Descrição | Detalhes sobre aplicação | Não | Uso em quartos e áreas comuns |
| Periodicidade | Par de campos reais: unidade (Dia/Mês/Semanas) + Intervalo (número): não um texto livre | Sim | Mês + 1 |
| Checklist | Roteiro técnico vinculado ao tipo de equipamento | Sim | Checklist PMOC Split |
| Ativo | Disponibiliza o tipo para seleção: campo real não documentado antes | Sim | Sempre ativo |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Crie tipos de U.H. específicos para quartos PCD, fumantes e conectados, cada um tem itens de vistoria únicos que não existem nos quartos padrão.
> Para Tipo de AC, siga as periodicidades mínimas da Lei 13.589/2018: mensal para filtros de Split e Fan Coil; trimestral para revisão completa. Definir periodicidades mais longas que o legal cria risco de autuação.
> Sempre que criar um novo Tipo, revise se o checklist vinculado está completo e aprovado, um checklist incompleto se multiplica por todos os equipamentos ou quartos daquele tipo.


> [!DANGER]
> Alterar o checklist vinculado a um Tipo de U.H. ou Tipo de AC afeta todos os registros futuros daquele tipo. Apontamentos já realizados mantêm o checklist original, mas novos apontamentos usarão o checklist atualizado. Documente qualquer mudança de checklist.
> Não crie tipos genéricos como ''Padrão'' para todos os equipamentos de AC, cada tecnologia (Split, Fan Coil, Chiller) tem requisitos técnicos completamente diferentes e exige checklists e periodicidades distintas.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O checklist correto não está sendo carregado no app da camareira | O Tipo de U.H. do quarto não tem o checklist de Governança vinculado | Edite o Tipo de U.H., verifique os campos de checklist de Governança e vincule o checklist correto |
| Um equipamento de AC não está gerando preventivas no PMOC | O Tipo de AC não tem Periodicidade ou Checklist definidos | Edite o Tipo de AC, defina a Periodicidade e vincule um Checklist técnico |
| O Tipo de U.H. não aparece para seleção no cadastro de quartos | O Tipo está inativo | Acesse Cadastro Básico > Tipo de U.H., localize o tipo e ative o switch Ativo |
| A periodicidade do Tipo de AC está gerando manutenções com frequência errada | O campo Periodicidade foi preenchido com valor ou unidade incorretos | Edite o Tipo de AC e corrija a Periodicidade: verifique se está em meses ou dias conforme o padrão do sistema |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Checklist (roteiros de verificação)Cadastro Básico > Setor | Cadastro Básico > U.H. — Tipo de U.H. vinculadoCadastro Básico > Ar Condicionado — Tipo de AC vinculadoPMOC: periodicidade e checklist por tipo | Checklist carregado automaticamente no app ao iniciar limpeza ou manutençãoCronograma PMOC gerado com periodicidade configurada |
| Periodicidade do Tipo de AC | Governança: checklist correto por categoria de quartoRelatório de U.H. em Dia: conformidade por tipoHistórico de manutenção por tipo de equipamento | Alteração de checklist vinculado impacta todos os registros futuros do tipo |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Máquinas e Equipamentos [CadastroBasico/EquipamentoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'EquipamentoIndex', N'Cadastro de Máquinas e Equipamentos', N'2.5 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM e administradores. Realizado após o cadastro de setores e famílias. | Menu lateral > Cadastro Básico > Máquinas / Equipamentos pcmbysim.com.br/CadastroBasico/EquipamentoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar todos os ativos físicos da unidade, motores, bombas, geradores, quadros elétricos, elevadores e qualquer equipamento que receba manutenção. O cadastro de equipamentos é a base do prontuário técnico: cada OS, preventiva e intervenção realizada fica vinculada ao ativo, construindo um histórico que fundamenta decisões de Capex (substituição) versus Opex (manutenção continuada). Um equipamento não cadastrado é um equipamento invisível para a gestão.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Os setores precisam estar cadastrados: seção 2.2.
> A Família do equipamento precisa existir: seção 2.7 (Família de Equipamentos).
> Tenha em mãos a plaqueta de identificação do equipamento: fabricante, modelo, número de série e ano.
> Defina o padrão de TAG antes de começar, uma vez criado e vinculado a OS e preventivas, mudar o código é trabalhoso.


> [!INFO]
> **IMPORTANTE SABER**
> O que é a TAG?
> TAG é o código de identificação único gravado fisicamente no equipamento (etiqueta ou plaqueta metálica). Ela é a ''matrícula'' do ativo no sistema.
> Padrão recomendado: SIGLA-SETOR-NÚMERO. Exemplos: GER-TEC-01 (Gerador 01 da Área Técnica), BOM-POC-02 (Bomba 02 do Poço).
> Use sempre letras maiúsculas, sem espaços ou caracteres especiais.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ TAG DEFINIDA | → | 2️⃣ Família SELECIONADA | → | 3️⃣ Localização DEFINIDA | → | 4️⃣ Dados TÉCNICOS | → | 5️⃣ Documentação ANEXADA | → | 📊 Ativo no Sistema |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Cadastrar um novo equipamento

![Formulário de cadastro de equipamento — blocos de identificação, localização e dados técnicos](/screenshots/cadastro-equipamento-novo.png)


1. Acesse Cadastro Básico > Máquinas / Equipamentos e clique em Novo.


**Bloco A, Identificação e localização**

1. Selecione a Unidade e a Família de Equipamento (categoria macro do ativo).
1. Preencha a TAG seguindo o padrão definido para a unidade.
1. No campo Descrição, insira o nome completo do equipamento (ex: ''Gerador a Diesel 250 kVA'').
1. Selecione o Departamento Responsável pela manutenção deste ativo.
1. Preencha Setor e Local / U.H. com a localização física exata do equipamento.


**Bloco B, Dados técnicos do fabricante**

1. Preencha Fabricante, Modelo, Nº de Fabricação (série) e Ano de Fabricação.
1. Informe os dados de contato do fabricante nos campos Endereço e Contato do Fabricante, úteis para acionar suporte técnico especializado.


**Bloco C, Documentação técnica e conformidade NR-12**

O formulário real tem 8 campos nesta seção, não 4, 4 deles são de conformidade legal com a NR-12 (segurança no trabalho em máquinas e equipamentos), ausentes de versões antigas deste manual:


Características: específicações técnicas (potência, tensão, capacidade, consumo).

Laudo / Documentação: não é um campo de texto livre, é uma lista de seleção dos laudos já cadastrados no sistema (Cadastro Básico > Laudo). Só é possível vincular um laudo que já exista cadastrado, não digitar uma referência livre.

Descrição da Operação: procedimento resumido de uso e operação (nome real do campo, versões antigas deste manual chamavam de Instruções de Operação).

Instruções para utilização segura da Máquina / Equipamento: EPIs necessários e riscos associados ao manuseio.

Procedimentos a serem adotados em situações de emergência, campo real de conformidade NR-12, não documentado antes.

Treinamento do Operador, campo real de conformidade NR-12, não documentado antes.

Condições de segurança do equipamento (NR12 - Alínea ''f''), campo real de conformidade NR-12, não documentado antes.

Indicação conclusiva quanto às condições de segurança da máquina (NR12 - Alínea ''g''), campo real de conformidade NR-12, não documentado antes.

1. Use o campo Arquivo para anexar fotos ou documentos do equipamento, é um campo de upload nativo do formulário, não é preciso anexar por fora.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O equipamento aparece na listagem com sua TAG e está disponível para vinculação em OS, Preventivas e Laudos.
> Se o equipamento for de climatização, cadastre-o também em Ar Condicionado (seção 2.6) para que entre no PMOC.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade física onde o equipamento está instalado | Sim | Intercity Berrini |
| Família de Equip. | Categoria macro do ativo | Sim | Geradores / Bombas / Quadros Elétricos |
| TAG | Código único do equipamento: padrão: SIGLA-SETOR-Nº | Sim | GER-TEC-01 |
| Descrição | Nome completo do equipamento | Sim | Gerador a Diesel 250 kVA |
| Depto. Responsável | Departamento que gerencia a manutenção | Sim | Manutenção |
| Setor | Área física do equipamento | Sim | Área Técnica |
| Local / U.H. | Ponto exato de instalação | Não | Casa de Máquinas B1 |
| Fabricante | Empresa que fabricou o equipamento | Não | Stemac |
| Modelo | Referência comercial conforme plaqueta | Não | GTA 250 GS |
| Nº Fabricação | Número de série único do fabricante | Não | STE2019-00412 |
| Ano de Fabricação | Ano de produção do equipamento | Não | 2019 |
| Características | Específicações técnicas completas | Não | 250 kVA, 380V trifásico, diesel |
| Laudo / Doc. | Seleção de um laudo já cadastrado: não é texto livre | Não | Laudo NR-13: validade 12/2025 |
| Descrição da Operação | Procedimento resumido de uso e operação | Não | Ligar em modo AUTO antes de corte |
| Instruções p/ utilização segura | EPIs e riscos no manuseio | Não | EPI: luvas isolantes, protetor auricular |
| Procedimentos de emergência | Ações em caso de emergência com o equipamento: NR-12 | Não | Desligar disjuntor geral e acionar brigada |
| Treinamento do Operador | Treinamento exigido para operar o equipamento: NR-12 | Não | Curso NR-12 de 8h |
| Condições de segurança (NR12-f) | Avaliação das condições de segurança do equipamento: NR-12 | Não | Conforme |
| Indicação conclusiva (NR12-g) | Parecer final sobre a segurança da máquina: NR-12 | Não | Apto para operação |
| Arquivo | Upload nativo de foto/documento do equipamento | Não | foto_gerador.jpg |


Exemplos de famílias de equipamentos mais comuns em hotéis:

| Família | Equipamentos típicos |
| :--- | :--- |
| Geração de Energia | Geradores a diesel, No-breaks (UPS), Quadros de transferência |
| Hidráulica | Bombas de recalque, Pressurizadores, Caixas d''água, Cisternas |
| Climatização | Chillers, Fan Coils, Splits, Torres de resfriamento (cadastrar também em AC) |
| Elevadores | Elevadores sociais, monta-cargas, plataformas de acessibilidade |
| Prevenção a Incêndio | Bombas de incêndio, Sprinklers, Extintores, Detectores de fumaça |
| Elétrica | Quadros de distribuição, Transformadores, Banco de capacitores |
| Gases e Combustíveis | Tanques de GLP/GN, Medidores, Reguladores de pressão |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Instale fisicamente a etiqueta TAG no equipamento antes ou no mesmo dia do cadastro no sistema, a correspondência física-digital é fundamental para que o técnico encontre o ativo correto em campo.
> Preencha o campo ''Para utilização segura'' para todos os equipamentos com risco elétrico, de pressão ou de alta temperatura. Isso serve como briefing de segurança para técnicos novos na equipe.
> Use o campo Características para registrar dados que o técnico vai precisar em campo: tensão de operação, tipo de óleo, capacidade de filtro. Quanto mais completo, menos deslocamentos desnecessários.
> Fotografie o equipamento e a plaqueta de identificação e anexe no campo Arquivo do próprio cadastro, facilita a identificação remota e serve como evidência do estado no momento do cadastro.


> [!DANGER]
> Nunca reutilize uma TAG de um equipamento desativado para um novo equipamento. O histórico de OS e preventivas do TAG antigo passaria a aparecer no novo equipamento, comprometendo o prontuário técnico.
> Equipamentos de climatização precisam ser cadastrados em DOIS lugares: aqui (para OS e preventivas gerais) e em Cadastro Básico > Ar Condicionado (para o PMOC). Cadastrar apenas em um dos locais deixa o ativo sem cobertura em um dos módulos.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O equipamento não aparece para seleção ao abrir uma OS | TAG ou Descrição incorretos, equipamento inativo ou vinculado a outra unidade | Verifique na listagem se o equipamento está ativo e se a Unidade coincide com a da OS |
| Dois equipamentos aparecem com a mesma TAG | Erro de digitação no cadastro ou reaproveitamento indevido de TAG | Edite o equipamento mais recente e corrija a TAG para uma nova: não delete o antigo para preservar o histórico |
| A Família de Equipamento desejada não aparece na lista | A família não foi cadastrada ou está inativa | Acesse seção 2.7 (Família de Equipamentos), crie a família necessária e retorne ao cadastro |
| O histórico de manutenção de um equipamento está incompleto | Algumas OS foram abertas sem vincular o equipamento: apenas com setor e descrição | Para OS futuras, sempre selecione o equipamento pelo campo TAG. OS antigas sem vínculo não podem ser retroativamente vinculadas |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Família de Equipamentos (agrupamento)Cadastro Básico > Setor (localização) | OS: campo Máquina/Equipamento (prontuário do ativo)Preventiva: equipamentos disponíveis para planosLaudo: laudos técnicos vinculados ao ativo | Histórico acumulado de OS, preventivas e laudos por TAGDecisão de Capex vs Opex baseada no custo histórico |
| Fabricante e dados técnicos | PMOC: se também cadastrado em Ar CondicionadoMódulo Financeiro: custo de manutenção por ativoRelatório de BI: ranking de equipamentos mais problemáticos | TAG disponível imediatamente em OS e preventivas após cadastro |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Sistemas de Ar Condicionado [CadastroBasico/ArCondicionadoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'ArCondicionadoIndex', N'Cadastro de Sistemas de Ar Condicionado', N'2.6 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, técnicos de climatização e engenheiros responsáveis pelo PMOC. | Menu lateral > Cadastro Básico > Ar Condicionado pcmbysim.com.br/CadastroBasico/ArCondicionadoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar todos os equipamentos de climatização da unidade de forma que eles entrem automaticamente no PMOC e no cronograma de manutenção preventiva. O Ar Condicionado tem um módulo dedicado, separado dos equipamentos gerais, porque cada aparelho tem obrigações legais específicas (Lei 13.589/2018) e periodicidades de manutenção que o sistema precisa gerenciar individualmente. Um aparelho não cadastrado aqui não existe para o PMOC, criando uma lacuna legal grave.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Os setores precisam estar cadastrados: seção 2.2.
> Os Tipos de Ar Condicionado precisam estar configurados com periodicidade e checklist: seção 2.4.
> Tenha em mãos a plaqueta de cada aparelho: fabricante, modelo, número de série, BTU/TR e tipo de gás refrigerante.
> Defina o padrão de TAG para climatização antes de começar (ex: AC-SETOR-Nº).


> [!INFO]
> **IMPORTANTE SABER**
> Por que o Ar Condicionado tem cadastro separado dos Equipamentos Gerais?
> Porque o PMOC puxa automaticamente apenas os ativos do módulo de Ar Condicionado, não os de Máquinas/Equipamentos. Se um Split for cadastrado apenas como Equipamento Geral, ele não aparecerá no inventário legal do PMOC.
> Regra prática: todo aparelho de climatização deve ser cadastrado nos DOIS módulos: em Máquinas/Equipamentos (para OS e preventivas gerais) e em Ar Condicionado (para o PMOC).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ TAG DEFINIDA | → | 2️⃣ Tipo SELECIONADO | → | 3️⃣ Localização DEFINIDA | → | 4️⃣ Potência REGISTRADA | → | 5️⃣ Fabricante/Série INFORMADOS | → | 📊 Entra no PMOC |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Cadastrar um aparelho de ar condicionado

![Formulário de cadastro de AC — identificação, localização e específicações técnicas](/screenshots/cadastro-ac-novo.png)


1. Acesse Cadastro Básico > Ar Condicionado e clique em Novo.


**Bloco A, Identificação e específicação**

1. Selecione a Unidade e o Tipo de Ar Condicionado, o dropdown real tem 24 opções, incluindo além dos aparelhos em si (Split Hi-Wall, Fan Coil, Chiller etc.) itens de infraestrutura de PMOC (Quadros Elétricos, Medições PMOC, Qualidade do Ar, Rede de Dutos, Tomada de Ar Exterior, Dispositivos de Controle Automático), o módulo cobre mais que só aparelhos de climatização.
1. Preencha a TAG seguindo o padrão da unidade (ex: AC-302-01 para o 1º AC do quarto 302).
1. No campo Descrição, insira o nome identificador (ex: ''AC Split — Quarto 302'').
1. Preencha a Potência, o campo real oferece 6 unidades (BTU, CV, Kcal, kW, m³/h, TR), não só BTU ou TR. Este valor alimenta o cálculo de carga térmica do PMOC.


**Bloco B, Localização detalhada**

1. Selecione o Setor, o Local / U.H. e o Andar onde o aparelho está instalado.
1. Selecione o Departamento Responsável (campo real não documentado antes, mesma lista de departamentos usada em outras seções, ex.: Manutenção, Governança).


**Bloco C, Dados do fabricante e ciclo de vida**

1. Preencha Fabricante, Modelo, Nº de Fabricação e Ano de Fabricação.
1. No campo Próxima Manutenção, informe a data prevista para a próxima intervenção técnica, o sistema gera alertas com base nesta data.
1. Mantenha o switch Ativo habilitado.


**Bloco D, Detalhamento técnico**

1. No campo Características, registre: voltagem, tipo de gás refrigerante (R-410A, R-22, R-32), capacidade de filtro e ciclos de limpeza recomendados.
1. No campo Instruções de Operação, descreva o procedimento para operação segura do equipamento.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O aparelho entra automaticamente no inventário do PMOC.
> O cronograma de manutenção preventiva é gerado com base no Tipo de AC e sua periodicidade configurada.
> Alertas de ''Próxima Manutenção'' aparecem no calendário de PCM.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade física onde o aparelho está instalado | Sim | Intercity Berrini |
| Tipo de AC | Categoria técnica: define periodicidade e checklist. 24 opções reais, incluindo itens de infraestrutura PMOC além de aparelhos | Sim | Split Hi-Wall |
| TAG | Código único do aparelho | Sim | AC-302-01 |
| Descrição | Nome identificador do aparelho | Sim | AC Split: Quarto 302 |
| Potência | Capacidade de refrigeração: 6 unidades reais (BTU/CV/Kcal/kW/m³/h/TR), não só BTU/TR | Sim | 9.000 BTU |
| Departamento Responsável | Departamento que gerencia a manutenção do aparelho: campo real não documentado antes | Não | Manutenção |
| Setor | Área física de instalação | Sim | 5º Andar |
| Local / U.H. | Ponto exato ou quarto onde está instalado | Sim | UH 302 |
| Andar | Pavimento de instalação: usado no PMOC por Andar | Sim | 3 |
| Fabricante / Modelo | Marca e referência comercial | Não | LG / S09EQ5 |
| Nº Fabricação | Número de série do aparelho | Não | LG2021-00934 |
| Ano de Fabricação | Ano de produção | Não | 2021 |
| Próxima Manutenção | Data da próxima intervenção: gera alerta no PCM | Não | 15/06/2025 |
| Ativo | Inclui o aparelho no PMOC e nos filtros do sistema | Sim | Sempre ativo |
| Características | Voltagem, gás refrigerante, tipo de filtro | Não | 220V, R-410A, filtro G4 |


Padrões de TAG recomendados para climatização:

| Tipo de equipamento | Padrão de TAG | Exemplo |
| :--- | :--- | :--- |
| Split em U.H. | AC-[Nº UH]-[Sequencial] | AC-302-01 (1º Split do quarto 302) |
| Split em área comum | AC-[SIGLA SETOR]-[Sequencial] | AC-REST-01 (1º Split do Restaurante) |
| Fan Coil central | FC-[ANDAR]-[Sequencial] | FC-05-03 (3º Fan Coil do 5º Andar) |
| Chiller | CH-[SEQUENCIAL] | CH-01 (1º Chiller da unidade) |
| ACJ (janela) | ACJ-[SETOR]-[SEQUENCIAL] | ACJ-ADM-02 (2º ACJ do Administrativo) |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Cadastre os aparelhos por andar, de cima para baixo e da esquerda para a direita seguindo a planta. Isso facilita auditorias físicas e a lógica da equipe de manutenção durante as rondas.
> Fotografe a plaqueta de cada aparelho antes de cadastrar. Número de série e modelo na plaqueta muitas vezes diferem do que está na nota fiscal.
> Use o campo Próxima Manutenção para aparelhos que já têm histórico fora do sistema, isso evita que o PMOC gere ordens de serviço imediatas para aparelhos recém-manutenidos.


> [!DANGER]
> Inativar um aparelho de AC o remove imediatamente do inventário do PMOC. Faça isso apenas quando o equipamento for definitivamente desinstalado, para manutenções temporárias, use o campo Próxima Manutenção para reagendar.
> O tipo de gás refrigerante é uma informação crítica de segurança. Registre sempre no campo Características, técnicos sem essa informação podem causar acidentes ao realizar manutenção preventiva.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Um aparelho não aparece no inventário do PMOC | O aparelho foi cadastrado apenas em Máquinas/Equipamentos, não em Ar Condicionado | Cadastre o aparelho também em Cadastro Básico > Ar Condicionado — o PMOC puxa apenas deste módulo |
| O cronograma do PMOC não está gerando manutenções para um aparelho | O Tipo de AC não tem Periodicidade configurada, ou o switch Ativo está desligado | Verifique o Tipo de AC em Cadastro Básico > Tipo de Ar Condicionado e confirme Periodicidade e Checklist |
| O campo Potência está vazio mas o aparelho aparece no PMOC | O PMOC inclui o aparelho mesmo sem potência: mas os cálculos de carga térmica ficam incompletos | Edite o aparelho e preencha a Potência em BTU ou TR para que o relatório do PMOC seja completo |
| Dois aparelhos têm a mesma TAG | Erro no cadastro: o sistema permite TAGs duplicadas em aparelhos de AC | Edite um dos aparelhos e corrija a TAG para garantir unicidade no inventário |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Tipo de AC (periodicidade e checklist)Cadastro Básico > Setor (localização) | PMOC: inventário automático de todos os ACs ativosPMOC por Andar: visão setorizadaOS: chamados vinculados à TAG do aparelho | Inclusão automática no PMOC ao ser cadastrado e ativadoCronograma gerado com base na periodicidade do Tipo de AC |
| Potência (BTU/TR) e carga térmica | Preventiva: planos de manutenção por tipo de equipamentoHistórico: prontuário técnico do aparelhoGreen Planet: consumo energético do sistema de climatização | Alerta de Próxima Manutenção no calendário de PCM |', NULL, NULL, NULL, NULL, 1);

-- Família de Equipamentos [CadastroBasico/FamiliaEquipamentoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'FamiliaEquipamentoIndex', N'Família de Equipamentos', N'2.7 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Administradores e gestores de PCM. Cadastro realizado antes dos equipamentos. | Menu lateral > Cadastro Básico > Família — Equipamento pcmbysim.com.br/CadastroBasico/FamiliaEquipamentoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de criar e gerenciar as categorias que agrupam equipamentos similares, como ''Bombas Hidráulicas'', ''Geradores'' ou ''Quadros Elétricos''. A Família de Equipamentos é o nível de classificação acima do equipamento individual: ela permite vincular normas técnicas e periodicidades de manutenção que se aplicam a um grupo inteiro de ativos, e gera relatórios de custo e ocorrência por categoria em vez de por equipamento individual.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A Unidade precisa estar cadastrada: seção 2.1.
> Planeje as famílias antes de cadastrar qualquer equipamento, alterar a família de um equipamento já com histórico exige cuidado.
> Use nomes técnicos reconhecidos pela equipe de manutenção, evite criar famílias muito genéricas ou muito específicas.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Família CRIADA | → | 2️⃣ Equipamento CADASTRADO | → | 3️⃣ Família VINCULADA | → | 📊 Relatórios por Família |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Cadastro Básico > Família - Equipamento — listagem com botão Novo](/screenshots/cadastro-familia-equipamento-listagem.png)


1. Acesse Cadastro Básico > Família, Equipamento e clique em Novo.
1. Selecione a Unidade à qual a família pertence.
1. No campo Descrição, insira o nome técnico da família (ex: ''Bombas de Recalque'', ''Sistemas de Climatização'', ''Elevadores'').
1. Mantenha Ativo habilitado e clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A família aparece disponível para seleção no cadastro de Máquinas/Equipamentos.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade à qual a família pertence | Sim | Intercity Berrini |
| Descrição | Nome técnico da categoria de equipamentos | Sim | Bombas de Recalque |
| Ativo | Disponibiliza a família para uso no cadastro de ativos | Sim | Sempre ativo |


> [!WARNING]
> **A tabela abaixo é uma sugestão, não a lista real**
> A lista real de famílias da sua unidade pode ser bem diferente do exemplo abaixo (algumas unidades usam famílias mais orientadas a A&B/cozinha, como Aquecedores, Bombas, Coifas, Estufas, Filtros, Fornos e Fogões, Refrigeração, Televisores, Utensílios). Use a tabela abaixo como ponto de partida, mas confirme com sua equipe as famílias que já existem antes de criar novas.


Famílias recomendadas para unidade hoteleira:

| Família | Equipamentos que agrupa |
| :--- | :--- |
| Geração de Energia | Geradores, No-breaks, Quadros de transferência automática |
| Hidráulica | Bombas de recalque, Pressurizadores, Caixas d''água, Cisternas, Vasos de pressão |
| Climatização | Chillers, Fan Coils, Torres de resfriamento, Self Contained |
| Elevadores e Transporte | Elevadores sociais, Monta-cargas, Plataformas PCD |
| Prevenção a Incêndio | Bombas de incêndio, Sprinklers, Extintores, Detectores, CCTV |
| Elétrica | Quadros de distribuição, Transformadores, SPDA, Banco de capacitores |
| Gases e Combustíveis | Tanques de GLP, Medidores de gás, Reguladores de pressão |
| Lazer e Esportes | Bombas de piscina, Filtros, Aquecedores, Equipamentos de academia |
| Refrigeração Comercial | Câmaras frias, Expositores frigoríficos, Ice makers |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Crie famílias no nível de granularidade que faz sentido para os seus relatórios. Se você nunca vai querer ver um relatório separado de ''Extintores'' vs ''Sprinklers'', coloque ambos na família ''Prevenção a Incêndio''.
> Uma família por norma técnica é uma boa prática: NR-13 (vasos de pressão), NR-10 (elétrica), NR-11 (transporte), facilita a vinculação de laudos e periodicidades legais.


> [!DANGER]
> Inativar uma família não inativa os equipamentos vinculados a ela, mas remove a família dos filtros de relatório. Equipamentos órfãos de família ativa podem não aparecer em análises de custo por categoria.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A família não aparece no cadastro de equipamentos | Família inativa ou vinculada a outra unidade | Edite a família, verifique a Unidade e ative o switch Ativo |
| Equipamento não aparece no relatório de custo por família | Equipamento cadastrado sem família vinculada | Edite o equipamento em Cadastro Básico > Equipamentos e vincule a família correta |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Administração > Unidades | Cadastro Básico > Máquinas/Equipamentos — seleção obrigatóriaRelatórios de custo e OS por categoria de ativo | Filtro de Família disponível em relatórios de BI e Dashboard |
| Normas técnicas vinculadas (NR-13, NR-10, etc.) | Planejamento de preventivas em massa para toda a famíliaDecisões de Capex por categoria |  |', NULL, NULL, NULL, NULL, 1);

-- Categoria de Serviço [CadastroBasico/CategoriaIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'CategoriaIndex', N'Categoria de Serviço', N'2.8 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Administradores e gestores de PCM. Cadastro realizado antes da abertura das primeiras OS. | Menu lateral > Cadastro Básico > Categoria — Serviço pcmbysim.com.br/CadastroBasico/CategoriaIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de criar e gerenciar as categorias que classificam as atividades de manutenção por especialidade técnica, como ''Elétrica'', ''Hidráulica'', ''Refrigeração'' ou ''Civil''. A Categoria de Serviço é o que transforma uma fila de OS em dados gerenciais: ela permite ao gestor analisar quanto da equipe está dedicada a cada especialidade, qual área da unidade demanda mais recursos e onde o orçamento está sendo consumido.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A Unidade precisa estar cadastrada: seção 2.1.
> Defina as categorias em conjunto com o gestor de PCM antes de cadastrar as primeiras OS, mudar a categoria de uma OS já registrada não é retroativo nos relatórios.
> Use os mesmos nomes usados pelo mercado e pela equipe técnica, nomes próprios da empresa dificultam o entendimento de novos colaboradores.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Categoria CRIADA | → | 2️⃣ OS ABERTA | → | 3️⃣ SLA MEDIDO | → | 📊 Custo por Categoria |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Cadastro Básico > Categoria - Serviço — listagem com as categorias reais cadastradas](/screenshots/cadastro-categoria-servico-listagem.png)


1. Acesse Cadastro Básico > Categoria, Serviço e clique em Novo.
1. Selecione a Unidade à qual a categoria pertence.
1. No campo Descrição, insira o nome da especialidade técnica (ex: ''Elétrica'', ''Hidráulica'', ''Refrigeração'').
1. Mantenha Ativo habilitado e clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A categoria aparece disponível para seleção ao abrir uma OS, preventiva ou plano de manutenção.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade à qual a categoria pertence | Sim | Intercity Berrini |
| Descrição | Nome da especialidade técnica | Sim | Elétrica |
| Ativo | Disponibiliza a categoria para seleção em OS e Preventivas | Sim | Sempre ativo |


> [!SUCCESS]
> **Lista real de categorias (19 no total)**
> Administração, Automação, Civil, Climatização, Elétrica, Eletrônica, Hidráulica, Jardinagem, Laudos, Manutenção, Marcenaria, Mecânica, Pintura, Preventiva, Proteção, Refrigeração, Rotina, Segurança, Serralheria. A tabela ''recomendada'' abaixo é só uma referência conceitual de agrupamento, não corresponde exatamente aos nomes já cadastrados (por exemplo, ''Limpeza Técnica'' não existe como categoria real).


Categorias de serviço recomendadas para unidade hoteleira:

| Categoria | Tipos de serviço que agrupa | Exemplos de OS |
| :--- | :--- | :--- |
| Elétrica | Instalações elétricas, iluminação, tomadas, quadros | Troca de lâmpada, curto no QD-03, instalação de tomada USB |
| Hidráulica | Água fria, água quente, esgoto, pluvial | Vazamento em quarto, entupimento de ralo, troca de sifão |
| Refrigeração | Sistemas de climatização, câmaras frias | AC sem gelar, limpeza de filtro, carga de gás |
| Civil | Alvenaria, pintura, revestimentos, esquadrias | Pintura de parede, troca de azulejo, reparo em porta |
| Marcenaria | Móveis, armários, portas de madeira | Reparo em armário, troca de dobradiça, restauro de mesa |
| Serralheria | Grades, portões, estruturas metálicas | Solda em portão, troca de fechadura, regulagem de grade |
| Jardinagem | Áreas verdes, piscinas e vasos | Poda de árvore, limpeza de piscina, plantio |
| Equipamentos | Máquinas, aparelhos e ativos em geral | Revisão de gerador, manutenção de elevador |
| Limpeza Técnica | Limpeza especializada: coifas, dutos, caixas | Limpeza de coifa, higienização de caixa d''água |
| Segurança | CCTV, alarmes, controle de acesso | Câmera sem imagem, fechadura com defeito |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Crie entre 6 e 12 categorias, menos que isso os relatórios são genéricos demais; mais que isso a equipe fica confusa na hora de classificar a OS.
> Treine toda a equipe sobre qual categoria usar em cada situação, a consistência na classificação é o que garante que os relatórios de BI façam sentido ao longo do tempo.
> Revise as categorias a cada 6 meses. Categorias com zero OS no período podem ser inativadas; categorias com alto volume podem ser divididas para análise mais granular.


> [!DANGER]
> Uma vez que as categorias entram nos relatórios históricos, evite renomeá-las, use nomes definitivos desde o início. Renomear quebra a continuidade dos gráficos de tendência.
> Não crie categorias redundantes como ''Elétrica'' e ''Eletricidade'', isso divide o volume de OS e distorce os relatórios de custo por especialidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Categoria não aparece ao abrir uma OS | Categoria inativa ou de outra unidade | Acesse Cadastro Básico > Categoria — Serviço, verifique Unidade e ative |
| Duas categorias com nomes similares causam confusão | Nomenclatura não padronizada na criação | Inative o duplicado e alinhe o nome correto com a equipe |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Administração > Unidades | OS: campo Categoria (classificação da demanda)Preventiva: organização por especialidadeMapa de Manutenção: vínculo entre categoria e atividade | Filtro de Categoria em todos os relatórios de desempenho e BI |
| Dados históricos de OS por categoria | Dashboard de BI: custo e volume por especialidadePlanejamento de equipe: dimensionamento por categoriaRelatório Mensal PCM: distribuição de esforço técnico | Centro de custo por especialidade no módulo Financeiro |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Colaboradores [CadastroBasico/FuncionarioIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'FuncionarioIndex', N'Cadastro de Colaboradores', N'2.9 — Gestor — Cadastros', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor-cadastros') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, RH e administradores. Realizado após cadastrar Funções e Departamentos. | Menu lateral > Cadastro Básico > Colaborador pcmbysim.com.br/CadastroBasico/FuncionarioIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar a equipe técnica e operacional da unidade de forma que cada colaborador possa receber OS, executar preventivas e ter seu tempo e custo calculados automaticamente. O cadastro de colaborador é diferente do cadastro de usuário (seção 1.3): o usuário define quem acessa o sistema; o colaborador define quem executa tarefas em campo. Um técnico pode ser ambos, ou apenas colaborador sem acesso ao sistema. Entender essa distinção é fundamental para uma implantação correta.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> As Funções precisam estar cadastradas: seção 2.10.
> Os Departamentos precisam estar cadastrados: seção 2.11.
> Se o colaborador também acessará o sistema (desktop ou app), crie primeiro o usuário: seção 1.3, e depois vincule no cadastro do colaborador.
> Tenha em mãos o custo-hora do colaborador para cálculo correto de despesas de manutenção.


> [!INFO]
> **IMPORTANTE SABER**
> Diferença entre Colaborador e Usuário:
> Usuário (seção 1.3): define login, senha e permissões de acesso ao sistema.
> Colaborador (esta seção): define quem pode receber e executar tarefas operacionais.
> Exemplos de colaborador sem usuário: técnico terceirizado que não acessa o sistema, apenas aparece como executor em OS.
> Exemplos de usuário sem colaborador: gestor que apenas consulta relatórios mas não executa tarefas em campo.
> Exemplos de ambos: técnico próprio que usa o app mobile para apontar OS, precisa dos dois cadastros vinculados.


> [!WARNING]
> **NA PRÁTICA, O CAMINHO REAL É PELO USUÁRIO, NÃO POR ESTA TELA**
> Confirmado com a equipe operacional: hoje o cadastro direto de Colaborador (o formulário desta seção) é usado raramente. O fluxo real do dia a dia é: criar/editar o colaborador em Administração > Usuários (seção 1.3) e ativar lá o switch Contabiliza Hora, destinado a quem executa apontamentos (técnicos, gestores de manutenção ou governança, camareiras). Ao ativar esse switch, o sistema gera automaticamente um registro de Colaborador no módulo selecionado para aquele usuário, já vinculado ao setor/módulo principal dele.


> [!DANGER]
> **PASSO A PASSO ABAIXO CONSIDERADO OBSOLETO**
> O formulário direto de Cadastro Básico > Colaborador (passo 4.1 abaixo) é um cadastro legado, restrito ao uso interno da equipe da SIM Services no cadastramento de perfis, não é o caminho a orientar o usuário final do hotel. Para o dia a dia da unidade, use exclusivamente Administração > Usuários (seção 1.3). O passo a passo abaixo fica mantido só como referência técnica.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Depto. CADASTRADO | → | 2️⃣ Função CADASTRADA | → | 3️⃣ Usuário CRIADO | → | 4️⃣ Colaborador CADASTRADO | → | 5️⃣ Custo/Hora DEFINIDO | → | 📊 Disponível para OS |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Cadastrar um novo colaborador

![Formulário de cadastro de colaborador — dados de identificação, função e configurações de custo](/screenshots/cadastro-colaborador-novo.png)


1. Acesse Cadastro Básico > Colaborador e clique em Novo.


**Bloco A, Identificação e vínculo**

1. Selecione a Unidade principal do colaborador.
1. Selecione o Tipo de Funcionário, só 2 opções reais: Próprio (CLT) ou Terceiro (prestador). Não existe opção de Estágio, apesar do que versões antigas deste manual indicavam.
1. Selecione o Módulo de atuação principal: Manutenção, Governança, Qualidade, etc.
1. Preencha o Nome completo do colaborador.
1. Se o colaborador tiver acesso ao sistema (app ou desktop), vincule no campo Usuário, selecione o ID-Usuário já cadastrado na seção 1.3.
1. Preencha o Telefone (campo real não documentado antes).


**Bloco B, Função e departamento**

1. Selecione a Função correspondente ao cargo (ex: Oficial de Manutenção, Eletricista, Camareira).


**Bloco C, Custo e produtividade**

1. No campo Valor Hora, informe o custo-hora do colaborador:


Fórmula: (Salário Bruto + Encargos) ÷ Horas Mensais Contratadas.

Exemplo: R$ 3.100,00 bruto ÷ 200 horas = R$ 15,50/hora.

1. Ative Contabiliza Hora para que o tempo de execução entre nos relatórios de produtividade e custo de manutenção.
1. Mantenha Ativo habilitado para que o colaborador apareça disponível para OS.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O colaborador aparece disponível para seleção como executor em OS e Preventivas.
> Se vinculado a um Usuário, as tarefas aparecerão no aplicativo mobile imediatamente.
> O custo de cada OS executada por este colaborador ainda não aparece de volta na própria OS, mesma pendência já documentada em §4.1 e §2.16.


### 4.2  Inativar um colaborador desligado

1. Localize o colaborador na listagem e clique em Editar (lápis).
1. Desative o switch Ativo.
1. Clique em Salvar.


> [!DANGER]
> Inativar o colaborador não exclui seu histórico. Todo o registro de OS executadas, preventivas realizadas e horas apontadas é preservado para auditoria, apenas as novas atribuições são bloqueadas.
> Se o colaborador também é Usuário, inative os dois registros: o Colaborador aqui e o Usuário em Administração > Usuários (seção 1.3).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade principal de lotação | Sim | Intercity Berrini |
| Tipo de Funcionário | Vínculo empregatício: só 2 opções reais | Sim | Próprio / Terceiro |
| Módulo | Área de atuação no sistema | Sim | Manutenção / Governança |
| Nome | Nome completo do colaborador | Sim | Carlos Oliveira |
| Usuário | ID-Usuário vinculado: necessário para uso do app | Não | carlos.oliveira@intercity |
| Telefone | Contato do colaborador: campo real não documentado antes | Não | (11) 99999-0000 |
| Função | Cargo ou especialidade técnica | Sim | Oficial de Manutenção |
| Valor Hora | Custo/hora para cálculo de despesa em OS | Não | R$ 15,50 |
| Contabiliza Hora | Inclui horas de execução nos relatórios de produtividade | — | Ativar para todos os executores |
| Ativo | Disponibiliza o colaborador para atribuição em OS | Sim | Sempre ativo |


Configurações por tipo de colaborador:

| Perfil | Tipo de Func. | Módulo | Usuário vinculado | Valor Hora | Contabiliza Hora |
| :--- | :--- | :--- | :--- | :--- | :--- |
| Técnico PCM próprio | Próprio | Manutenção | Sim (app mobile) | Sim | Sim |
| Técnico terceirizado | Terceiro | Manutenção | Não obrigatório | Sim | Sim |
| Camareira própria | Próprio | Governança | Sim (app mobile) | Sim | Sim |
| Supervisor de plantão | Próprio | Manutenção | Sim (desktop e app) | Sim | Sim |
| Gestor de PCM | Próprio | Manutenção | Sim (desktop) | Sim | Opcional |
| Consultor externo | Terceiro | Qualidade | Não obrigatório | Sim | Não |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Cadastre o colaborador antes de criar o usuário vinculado, assim você já tem o nome e a função definidos para preencher os campos do usuário de forma consistente.
> Sempre preencha o Valor Hora, mesmo para terceiros. O custo de mão de obra terceirizada é muitas vezes maior que a equipe própria, ter esse dado permite justificar contratações ou internalizar serviços.
> Para técnicos que atuam em mais de uma unidade, verifique com o Administrador se é necessário criar registros separados por unidade ou se o mesmo colaborador pode ser vinculado a múltiplas unidades.
> Use a listagem com filtro por Módulo para fazer auditorias periódicas da equipe ativa, é comum encontrar colaboradores desligados ainda como ativos após alguns meses.


> [!DANGER]
> Nunca exclua um colaborador, apenas inative. A exclusão apaga o vínculo com OS e preventivas passadas, criando registros órfãos no histórico.
> Um colaborador inativo com Usuário ativo ainda pode fazer login no sistema. Inative os dois registros simultaneamente no momento do desligamento.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O colaborador não aparece na lista de executores ao abrir uma OS | Colaborador inativo, módulo errado ou unidade diferente da OS | Edite o colaborador: verifique switch Ativo, campo Módulo (deve ser Manutenção para OS de PCM) e Unidade |
| As horas do colaborador não estão aparecendo nos relatórios de produtividade | Switch ''Contabiliza Hora'' está desativado | Edite o colaborador e ative o switch Contabiliza Hora: as próximas OS já terão o tempo computado |
| O custo de mão de obra está aparecendo zerado nas OS | Campo Valor Hora está vazio ou com valor zero | Edite o colaborador, preencha o Valor Hora corretamente e salve: OS futuras já terão o custo calculado |
| Um colaborador aparece duplicado na listagem | Foi cadastrado duas vezes: geralmente ao criar também como usuário | Identifique o duplicado, transfira o Usuário vinculado para o registro correto e inative o duplicado |
| O técnico não consegue ver as tarefas no app mobile | Colaborador sem Usuário vinculado ou Usuário sem acesso ao app habilitado | Edite o colaborador, vincule o Usuário correto. No usuário, ative o switch ''Acesso via Aplicativo'' |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Função (cargo)Cadastro Básico > Departamento (área)Administração > Usuários (acesso ao sistema) | OS: lista de executores disponíveisPreventiva e Rotina: responsável pela execuçãoPCM > Falta — impacta disponibilidade | Custo de mão de obra calculado automaticamente nas OSTarefas visíveis no app mobile quando vinculado ao usuário |
| Valor Hora e Contabiliza Hora | Relatórios de produtividade: custo por executorDashboard: % ociosidade e horas trabalhadasMódulo Financeiro: despesa de mão de obra por OS | Histórico preservado após inativação: apenas novas atribuições bloqueadas |', NULL, NULL, NULL, NULL, 1);

-- Plano de Ação [PlanoAcao/PlanoAcaoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'PlanoAcao', N'PlanoAcaoIndex', N'Plano de Ação', N'3.10 — Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, coordenadores de qualidade e diretores. Perfil mínimo: Gestor de PCM. | Menu Principal > Plano de Ação pcmbysim.com.br/PlanoAcao/PlanoAcaoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de criar, delegar e monitorar tarefas corretivas que fecham o ciclo de qualidade da unidade. Enquanto a auditoria identifica o problema e a OS resolve o técnico, o Plano de Ação formaliza a correção estrutural: quem é responsável, qual é o prazo e qual o percentual de execução. Sem o Plano de Ação, não conformidades detectadas em auditorias ficam sem dono e sem prazo, e o mesmo problema reaparece na próxima inspeção.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> O Plano de Ação pode ser gerado automaticamente por uma Auditoria Corporativa (se configurado) ou criado manualmente pelo gestor.
> Os responsáveis pelas tarefas precisam ser colaboradores ativos com acesso ao sistema.
> Para tarefas que exigem compra de material ou serviço, abra uma Requisição (seção 3.7) vinculada ao plano.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'O Plano de Ação pode nascer de duas origens e segue o mesmo ciclo de execução:

| 1️⃣ Origem: AUDITORIA | → | 2️⃣ Ou Criação: MANUAL | → | 3️⃣ Atribuição: RESPONSÁVEL | → | 4️⃣ Execução: % ATUALIZADO | → | 5️⃣ Conclusão: 100% | → | 📊 Histórico p/ Auditoria |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |


> [!INFO]
> **IMPORTANTE SABER**
> Diferença entre Plano de Ação e Ordem de Serviço:
> OS: reparo técnico específico, troca de peça, conserto de equipamento. Tem técnico executor e custo de material.
> Plano de Ação: melhoria de processo ou correção estrutural, ''criar procedimento de abertura de OS'', ''treinar equipe em NR-10'', ''instalar sinalização de segurança''. Tem responsável e percentual de progresso.
> Um item de auditoria pode gerar tanto uma OS (se for reparo físico) quanto um Plano de Ação (se for processo ou conformidade).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Criar um Plano de Ação manualmente

![Tela Plano de Ação — listagem com cards de status e botão Novo](/screenshots/plano-acao-listagem.png)


1. Acesse Plano de Ação no menu lateral e clique em Novo.
1. Preencha a Descrição da tarefa, seja específico: ''Criar POP de abertura de OS para hóspedes'' é melhor que ''Melhorar processo''.
1. Selecione o Solicitante, o formulário real não tem um campo Responsável (pessoa) como este manual chegou a indicar; a atribuição é feita por Solicitante (quem relata/pede) e Departamento (área responsável), não por uma pessoa individual.
1. Defina o Departamento relacionado à tarefa.
1. Defina a Prioridade (0-Crítica, 1-Alta, 2-Média, 3-Baixa), campo real não documentado antes.
1. Defina o Prazo de Execução. Seja realista, prazos impossíveis geram planos eternamente em ATRASADO.
1. Anexe um Arquivo se necessário (campo real não documentado antes).
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A tarefa aparece na listagem com status PENDENTE e 0% de execução.
> O responsável pode visualizar a tarefa atribuída a ele.


### 4.2  Atualizar o progresso de uma tarefa

1. Na listagem, localize a tarefa e clique em Editar (lápis).
1. Atualize o campo % Execução com o progresso real, use marcos intermediários (25%, 50%, 75%) para tarefas longas.
1. Se houver observações relevantes sobre o andamento, registre no campo de Observações.
1. Quando a tarefa estiver 100% concluída, altere o status para Concluído e salve.


> [!INFO]
> **RESULTADO ESPERADO**
> O percentual é atualizado visualmente na listagem.
> Ao atingir 100% e status Concluído, a tarefa sai dos pendentes e vai para o histórico.


### 4.3  Monitorar o painel de Planos de Ação

![Listagem de Planos de Ação — colunas de responsável, prazo, % execução e status com destaque para ATRASADO](/screenshots/plano-acao-detalhado.png)


1. Observe os itens em ATRASADO (prazo ultrapassado com % menor que 100), eles exigem intervenção imediata do gestor.
1. Use os filtros Departamento, Responsável e Status para ter visões específicas da equipe.
1. Para exportar o relatório completo para reuniões de diretoria, use Exportar (Excel / PDF).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Descrição | O que precisa ser feito: seja específico e mensurável | Sim | Criar POP de abertura de OS para hóspedes |
| Solicitante | Quem relata/pede a ação: não existe campo Responsável (pessoa) | Sim | Carlos Oliveira |
| Departamento | Área responsável pela tarefa | Sim | Manutenção |
| Prioridade | Nível de urgência: campo real não documentado antes | Não | 1-Alta |
| Arquivo | Upload de documento de apoio: campo real não documentado antes | Não | evidência.pdf |
| Prazo de Execução | Data limite para conclusão da tarefa | Sim | 30/06/2025 |
| % Execução | Progresso atual: atualizado pelo responsável | — | 0% / 25% / 50% / 75% / 100% |
| Status | Situação atual da tarefa | Auto | Pendente / Em Andamento / Concluído / Atrasado |
| Observações | Registro de andamento, bloqueios ou informações adicionais | Não | Aguardando aprovação do gestor regional |
| Origem | Se gerado por auditoria, referência ao item não conforme | Auto | Auditoria Corporativa: item 3.2 |


Estados possíveis de um Plano de Ação:

| Status | Condição | O que fazer |
| :--- | :--- | :--- |
| PENDENTE | Criado, não iniciado: 0% | Cobrar início do responsável se próximo do prazo |
| EM ANDAMENTO | % entre 1% e 99% | Monitorar progresso semanalmente |
| ATRASADO | Prazo ultrapassado e % menor que 100% | Intervenção imediata: renegociar prazo ou substituir responsável |
| CONCLUÍDO | % igual a 100% e status Concluído | Registrar aprendizado e fechar o ciclo na auditoria |
| BACKLOG | Status real não documentado antes | Priorizar quando houver capacidade |
| CANCELADA | Status real não documentado antes | Registrar motivo do cancelamento |
| VINCULADA | Status real não documentado antes | Verificar a que outro registro está vinculada |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Quebre tarefas grandes em subtarefas menores com marcos intermediários. ''Reformar toda a área de manutenção'' como um único item é ingerenciável, divida em etapas de 1 a 2 semanas cada.
> Revise o painel de Planos de Ação toda semana em reunião de equipe. A visibilidade coletiva do progresso é o principal fator de cumprimento de prazos.
> Ao gerar um Plano de Ação por auditoria, atribua o responsável no mesmo dia, tarefas sem dono definido raramente avançam.
> Use o campo Observações como diário de bordo da tarefa. Registre bloqueios, dependências e decisões tomadas, esse histórico é valioso na auditoria seguinte.


> [!DANGER]
> Planos de Ação em ATRASADO em duas auditorias consecutivas indicam falha sistêmica, ou o prazo era irreal, ou o responsável não tem os recursos necessários. Análise a causa antes de simplesmente reabrir o prazo.
> Nunca marque 100% sem ter entregado o resultado real. Planos de Ação ''concluídos'' que reaparecem na próxima auditoria destroem a credibilidade do processo de qualidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O Plano de Ação gerado pela auditoria não aparece na listagem | A auditoria não foi configurada para gerar Planos de Ação automaticamente | Acesse Cadastro Básico > Auditoria Corporativa, abra o modelo e ative o switch ''Gerar Plano de Ação''. Itens futuros serão gerados automaticamente |
| Não consigo atribuir um responsável: o campo está vazio | Nenhum colaborador ativo está vinculado ao departamento selecionado | Verifique em Cadastro Básico > Colaborador se há colaboradores ativos no departamento. Se não houver, selecione outro departamento ou adicione o colaborador ao departamento correto |
| O status continua PENDENTE mesmo após atualizar o percentual | O sistema muda para Em Andamento apenas quando o % é maior que 0% e o status é editado manualmente | Além de atualizar o %, altere o campo Status para ''Em Andamento'' e salve |
| Um Plano de Ação concluído reapareceu na auditoria como não conforme | O item foi concluído no sistema mas a ação física não foi implementada, ou o prazo de validade da correção expirou | Revise o critério de conclusão com o responsável. A auditoria verifica a realidade física, não apenas o registro no sistema |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Auditoria Corporativa e de Qualidade: geração automáticaGestor de PCM: criação manual | Dashboard: se configurado para exibir planos atrasadosRelatório de Auditoria: fechamento do ciclo de qualidadeMódulo Excel > Plano de Ação (extração de dados) | Notificação ao responsável ao ser atribuídoAtualização do índice de conformidade da auditoria ao concluir |
| PCM > Requisição — para tarefas que precisam de compraCadastro Básico > Colaborador — responsáveis | Histórico de não conformidades resolvidas por unidadeRelatório Mensal PCM: itens fechados no período | Exportação para Excel para apresentação à diretoria |', NULL, NULL, NULL, NULL, 1);

-- Históricos de Preventiva, Rotina e Laudo [PCM/HistoricoPreventiva]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'PCM', N'HistoricoPreventiva', N'Históricos de Preventiva, Rotina e Laudo', N'3.11 — Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, auditores, coordenadores de conformidade e diretores. | PCM > Histórico — Preventiva / Rotina / Laudo pcmbysim.com.br/PCM/HistoricoPreventiva |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de consultar, filtrar e exportar o registro completo de todas as execuções de preventivas, rotinas e laudos da unidade. O histórico é a memória técnica do sistema, ele prova que o plano de manutenção foi cumprido, permite análises de tendência de falhas em equipamentos e serve como a principal evidência em fiscalizações e auditorias. Sem o histórico bem alimentado, a unidade pode ter feito tudo certo mas não ter como provar.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Os históricos são gerados automaticamente pelo sistema a cada apontamento concluído, não há cadastro manual de histórico.
> Para que o histórico seja rico e útil, os apontamentos precisam ter sido feitos com qualidade: checklist respondido, observações preenchidas e fotos anexadas quando necessário.
> O acesso ao histórico depende do perfil, técnicos geralmente veem apenas seus próprios apontamentos; gestores veem toda a equipe.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Apontamento CONCLUÍDO | → | 2️⃣ Registro AUTOMÁTICO | → | 3️⃣ Histórico FILTRADO | → | 4️⃣ Detalhes VISUALIZADOS | → | 📊 Exportação |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Consultar o histórico de Preventivas

![Tela PCM > Histórico — Preventiva — listagem com filtros e ícones de visualização](/screenshots/historico-preventiva.png)


1. Acesse PCM > Histórico, Preventiva.
1. Aplique os filtros:


Unidade, selecione o hotel.

Preventiva, filtre por um plano específico (ex: ''Preventiva Mensal — Gerador'').

Tipo de Serviço, diferencie entre execuções internas e por terceiros.

Período, defina o intervalo de datas para a consulta.

1. Clique em Filtrar.
1. Na listagem, clique no ícone de visualização (olho) de qualquer registro para abrir o apontamento completo.
1. Dentro do apontamento, você pode ver: checklist respondido item a item, fotos anexadas, observações do técnico, data e hora exatas e assinatura digital.


> [!INFO]
> **RESULTADO ESPERADO**
> Você acessa o registro completo de qualquer preventiva executada, com todas as evidências necessárias para auditoria. A listagem tem 3 ícones por linha (não só o de visualização) — um deles abre a mesma tela usada para executar a preventiva, com os botões Reabrir Ordem de Serviço e Excluir (ver aviso abaixo).


### 4.2  Consultar o histórico de Rotinas

O processo é idêntico ao de Preventivas:

1. Acesse PCM > Histórico, Rotina.
1. Use os filtros: Unidade, Rotina, Tipo de Serviço e Período.
1. Clique em Filtrar e acesse qualquer registro pelo ícone de visualização.


> [!INFO]
> **IMPORTANTE SABER**
> Para auditorias de segurança ou seguros, o histórico de rotinas é a prova de que a unidade realiza inspeção contínua. A ausência de registros de ronda, especialmente noturnas, é frequentemente citada em relatórios de sinistros como fator agravante de responsabilidade.


### 4.3  Consultar o histórico de Laudos

1. Acesse PCM > Histórico, Laudo.
1. Use os filtros: Unidade, Manutenção (tipo de laudo) e Período.
1. Na listagem, além do ícone de visualização, há o ícone de download do arquivo, clique para baixar o PDF original do laudo registrado naquele apontamento.


> [!INFO]
> **RESULTADO ESPERADO**
> Você acessa o arquivo original de qualquer laudo histórico em segundos, sem precisar de pastas físicas ou e-mails antigos.


### 4.4  Exportar histórico para relatório gerencial

1. Com os filtros aplicados, clique em Exportar (Excel / PDF).
1. O arquivo gerado contém: datas, descrições, executores, tipos de serviço e valores, pronto para ser incluído em apresentações de resultado.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Data da Execução | Quando a preventiva, rotina ou laudo foi realizado | Auto | 27/05/2025 14:32 |
| Descrição | Nome do plano ou tipo de laudo executado | Auto | Preventiva Mensal: Ar Condicionado |
| Tipo de Serviço | Execução interna (equipe própria) ou por terceiros | Auto | Interno / Terceiros |
| Qtde. Equipamentos | Número de ativos cobertos no apontamento (preventiva) | Auto | 12 aparelhos |
| Valor | Custo registrado no apontamento, se informado | Auto | R$ 0,00 (interno) |
| Visualizar | Ícone olho: abre o apontamento completo com checklist | — | Clique para ver detalhes |
| Download | Ícone de arquivo: baixa o PDF original (histórico laudo) | — | Disponível apenas no histórico de laudos |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Consulte o histórico de preventivas de um equipamento antes de decidir pela substituição. Se o equipamento tem 5 anos de preventivas regulares sem falhas graves, ele pode ter mais vida útil do que parece. Se tem histórico de falhas recorrentes, a substituição é tecnicamente justificável, e o histórico é a prova para o argumento de Capex.
> Para preparar uma auditoria externa, gere os históricos dos últimos 12 meses com antecedência e verifique se há lacunas. É muito melhor identificar uma semana sem ronda registrada antes da auditoria do que durante ela.
> Use o filtro de Tipo de Serviço = Terceiros para verificar se os fornecedores externos estão cumprindo os contratos de manutenção, o histórico é a evidência de que o serviço foi prestado.


> [!DANGER]
> **ATENÇÃO — o histórico NÃO é imutável na interface**
> Um registro histórico de Preventiva concluído pode ser reaberto e editado: a tela acessada por um dos ícones do histórico tem os botões Reabrir Ordem de Serviço e Excluir, além de campos de texto editáveis. Não trate a interface como garantia de integridade para fins de auditoria.
> **Importante entender o que ''Excluir'' realmente faz:** o sistema nunca apaga um registro de fato, em nenhuma tela do PCM by SIM. O botão ''Excluir'' executa um **cancelamento lógico** (o registro é marcado como cancelado no banco de dados, não removido). Ou seja, o dado histórico continua existindo internamente mesmo depois de ''excluído'', mas o rótulo do botão pode levar o usuário a pensar, erradamente, que o apagamento é permanente e irreversível.
> Trate a não-edição de registros concluídos como uma boa prática de equipe (nunca reabrir/editar/excluir um apontamento já finalizado), não como uma trava técnica do sistema.
> A ausência de registros no histórico não significa que nada foi feito, mas do ponto de vista legal e de auditoria, o que não está registrado não aconteceu.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O histórico está vazio para um período em que sei que foram feitas manutenções | Os apontamentos foram iniciados mas não foram finalizados (status diferente de Concluído) | Acesse PCM > Preventiva ou Rotina, filtre por status ''Em Andamento'' no período e conclua os apontamentos pendentes |
| Não consigo ver o histórico de outra unidade | Perfil sem acesso à unidade desejada | Solicite ao Administrador habilitação de acesso à unidade nos dados do seu usuário |
| O ícone de download do arquivo não aparece no histórico de laudos | O apontamento foi registrado sem anexar o arquivo, ou o arquivo foi corrompido | Abra o apontamento e faça o upload do arquivo novamente através do módulo PCM > Laudo / Documentação |
| A exportação para Excel está gerando um arquivo com dados incompletos | O filtro de período está muito amplo e o relatório está truncado por limite de linhas | Divida a exportação em períodos menores (ex: trimestral) para garantir que todos os registros sejam incluídos |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| PCM > Preventiva — apontamentos concluídosPCM > Rotina — rondas concluídasPCM > Laudo / Documentação — laudos registrados | Auditoria Corporativa: evidências de execuçãoPMOC > Histórico — parte do dossiê técnicoRelatório Mensal PCM | Registro automático a cada apontamento finalizadoImutabilidade garantida pelo sistema após conclusão |
| Cadastro Básico > Equipamentos — prontuário do ativoCadastro Básico > Colaborador — executor registrado | Análise de confiabilidade de equipamentos (Capex vs Opex)Módulo Excel: extração para análise externa | Download do arquivo original de laudos para fiscalização |


> [!SUCCESS]
> **FASE 2 CONCLUÍDA, CAPÍTULO 3: PCM COMPLETO**
> 11 seções produzidas · Todas no padrão de 8 blocos
> 3.1 OS  ·  3.2 Preventiva  ·  3.3 Rotinas  ·  3.4 Cronograma
> 3.5 Laudo  ·  3.6 PMOC  ·  3.7 Requisição  ·  3.8 Aprovação
> 3.9 Faltas  ·  3.10 Plano de Ação  ·  3.11 Históricos


Próxima fase: Fase 3, Cadastro Básico e Suprimentos (Caps. 2 e 4)', NULL, NULL, NULL, NULL, 1);

-- Laudo e Documentação Técnica [AEB/LaudoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'AEB', N'LaudoIndex', N'Laudo e Documentação Técnica', N'3.5 — Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, coordenadores de conformidade e diretores responsáveis pela regularidade legal da unidade. | Menu lateral > PCM > Laudo / Documentação pcmbysim.com.br/PCM/ManutencaoLaudo |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de controlar a validade e o armazenamento de todos os laudos técnicos, licenças e certificados obrigatórios da unidade em um único painel. Isso significa nunca mais ser pego de surpresa em uma fiscalização: o sistema alerta automaticamente quando um documento está próximo do vencimento, e o histórico digital elimina a necessidade de pastas físicas. Um laudo vencido ou inexistente gera não conformidade crítica nas auditorias corporativas e pode resultar em interdição ou multa por parte dos órgãos reguladores.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Os tipos de laudo precisam estar cadastrados com suas periodicidades: Cadastro Básico > Laudo / Documentação.
> Para fazer o upload do documento, tenha o arquivo digital em PDF ou imagem (JPG/PNG) disponível no seu computador.
> O fornecedor responsável pelo laudo precisa estar cadastrado: Cadastro Básico > Fornecedor.
> Tenha em mãos a data de emissão e a data de validade do documento antes de iniciar o apontamento.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'O módulo de Laudo funciona como um calendário de obrigações legais com três estados visuais:

| Cor na listagem | Significado | Ação necessária |
| :--- | :--- | :--- |
| 🔴  VERMELHO | Laudo vencido: validade expirada | Contratar renovação imediatamente. Risco de interdição em fiscalização |
| 🟡  AMARELO | Laudo vence no mês corrente (próximos 30 dias) | Iniciar processo de renovação agora: evite deixar vencer |
| 🔵  AZUL | Laudo com vencimento futuro: em dia | Monitorar. Sem ação necessária no momento |
| 🟢  VERDE | Laudo renovado e upload realizado | Documento válido e evidência digital arquivada |


Ciclo completo de gestão de um laudo:

| 1️⃣ Tipo CADASTRADO | → | 2️⃣ Vencimento GERADO | → | 3️⃣ Laudo VENCIDO | → | 4️⃣ Renovação CONTRATADA | → | 5️⃣ Upload REALIZADO | → | 📊 Conformidade no Dashboard |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Monitorar o painel de laudos

![Tela PCM > Laudo / Documentação — listagem com código de cores por validade](/screenshots/laudo-listagem.png)


1. Acesse PCM > Laudo / Documentação.
1. Aplique os filtros Unidade e Período para carregar a visão que precisa.
1. Inicie sempre pelos documentos em VERMELHO, eles representam risco legal imediato.
1. Na sequência, trate os documentos em AMARELO, iniciar o processo de renovação agora evita que entrem no vermelho.
1. Use o botão Visualizar Cronograma para projetar os vencimentos dos próximos 12 meses e planejar o orçamento de renovações.


> [!INFO]
> **RESULTADO ESPERADO**
> Você tem uma visão completa do status legal da unidade com priorização visual automática.


### 4.2  Registrar o apontamento de um laudo renovado

![Formulário de apontamento — campos de data, fornecedor, valor e upload de arquivo](/screenshots/laudo-apontamento-real.png)


1. Na listagem, clique no X correspondente ao mês/laudo pendente, isso abre a tela real Apontamento - Laudo (URL /PCM/ApontamentoProgramada), o mesmo modelo universal de apontamento já usado em OS e Preventiva. Não existe um formulário específico de Laudo com Data de Emissão, Data de Validade ou Upload de Arquivo, apesar do que versões antigas deste manual indicavam.
1. Confira os campos pré-preenchidos: Setor, Serviço, Categoria-Serviço, Tipo de Serviço, Tipo-Ordem de Serviço.
1. Selecione o Colaborador responsável e o Fornecedor (empresa de inspeção, laboratório, engenheiro).
1. Preencha Solução com o resultado da inspeção/laudo.
1. Preencha Data Início / Hora Início e Data Término / Hora Término, não existe um campo único de Data de Emissão nem de Data de Validade nesta tela.
1. Se houver custo, preencha Valor. Preencha também Quantidade Equipamento se aplicável.
1. Clique em Salvar.


> [!INFO]
> **IMPORTANTE SABER, o apontamento é só a primeira etapa**
> Este passo 4.2 registra o **apontamento de horas** do laudo, não é aqui que o arquivo/documento em si é anexado. A anexação do documento (o PDF do laudo) é uma segunda etapa, feita na tela **Auditoria > Laudo / Documentação** (`pcmbysim.com.br/Auditoria/LaudoIndex`), é lá que o arquivo é de fato subido ao sistema. Sem completar essa segunda etapa, o laudo fica apontado mas sem o documento anexado.


> [!INFO]
> **COMO A VALIDADE É CALCULADA**
> A validade do laudo é calculada a partir da Data Término do apontamento somada à periodicidade configurada em Cadastro Básico > Laudo.


> [!INFO]
> **RESULTADO ESPERADO**
> O apontamento é salvo e atualiza o status do laudo na listagem.


### 4.3  Consultar o histórico de laudos anteriores

Para ver todas as versões anteriores de um documento e suas evidências arquivadas:

1. Acesse PCM > Histórico, Laudo (seção 3.11 deste manual).
1. Aplique os filtros de Unidade, Período e Tipo de Laudo.
1. Clique no ícone de visualização (olho) de qualquer registro para baixar o arquivo original anexado na época.


> [!INFO]
> **IMPORTANTE SABER**
> O histórico de laudos é a principal evidência em auditorias retrospectivas. Ele prova que a unidade sempre esteve em conformidade, não apenas no momento da fiscalização atual.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Tipo de Laudo | Documento a ser registrado: definido no Cadastro Básico | Auto | AVCB, Alvará Sanitário, NR-13 |
| Setor / Serviço | Pré-preenchidos a partir do laudo pendente | Auto | 01º Andar / Laudo de Inspeção... |
| Categoria-Serviço / Tipo de Serviço / Tipo-Ordem de Serviço | Classificação padrão do apontamento universal (mesma de OS/Preventiva) | Auto | Elétrica / Terceiros / Corretiva |
| Colaborador / Fornecedor | Responsável interno e empresa que executou o laudo | Sim | — |
| Solução | Resultado da inspeção: nome real do campo, não Data de Emissão | Sim | Aprovado sem ressalvas |
| Data/Hora Início e Término | 4 campos reais: não existe campo único de Data de Validade | Sim | 16/08/2026 |
| Valor | Custo do serviço de inspeção ou emissão do laudo | Não | R$ 1.800,00 |
| Quantidade Equipamento | Nº de equipamentos cobertos pelo laudo, se aplicável | Não | 1 |


Laudos obrigatórios mais comuns em unidades hoteleiras:

| Documento | Órgão fiscalizador | Periodicidade típica | Risco se vencido |
| :--- | :--- | :--- | :--- |
| AVCB: Auto de Vistoria do Corpo de Bombeiros | Corpo de Bombeiros | Anual | Interdição e multa |
| Alvará de Funcionamento | Prefeitura Municipal | Anual | Interdição imediata |
| Teste de Estanqueidade da rede de gás | Distribuidora / INMETRO | Anual | Interdição e risco de acidente |
| Laudo de SPDA (para-raios) | CREA / ART | Anual | Responsabilidade civil em sinistro |
| Análise de Potabilidade da água | Vigilância Sanitária | Semestral | Embargo sanitário |
| Laudos NR-13: Vasos de pressão | Ministério do Trabalho | Conforme norma | Autuação e interdição do equipamento |
| Certificado de Dedetização | Vigilância Sanitária | Semestral | Embargo sanitário |
| Limpeza de Caixa d''água | Vigilância Sanitária | Semestral | Embargo sanitário |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Cadastre todos os laudos obrigatórios da sua unidade de uma vez no Cadastro Básico > Laudo. Mesmo que a validade seja longa, o sistema avisa 30 dias antes do vencimento, não deixe nenhum de fora.
> Complete sempre as duas etapas: apontamento de horas (passo 4.2) e o upload do documento em Auditoria > Laudo / Documentação, um laudo apontado sem o arquivo anexado fica incompleto para fins de fiscalização.
> Use o filtro de ''Próximos 60 dias'' mensalmente para antecipar as renovações e incluí-las no planejamento orçamentário. Contratar com antecedência é geralmente mais barato.
> Atribua um responsável fixo pelo monitoramento de laudos em cada unidade, sem um dono claro, os vencimentos passam despercebidos.


> [!DANGER]
> Um laudo VERMELHO no Dashboard não é apenas um indicador visual, é um risco legal real. Qualquer fiscalização durante a vigência de um documento vencido pode resultar em embargo, multa ou interdição da unidade.
> Nunca registre uma data de validade incorreta para ''resolver'' o status vermelho no Dashboard. Isso falsifica o registro de conformidade da empresa e agrava a responsabilidade dos gestores em caso de sinistro ou fiscalização.
> A pontuação de ''Laudo/Documentação'' na tabela de Métricas por Atividade do Dashboard (seção 7.1) é calculada sobre todos os documentos ativos. Um único laudo vencido pode derrubar significativamente a nota da unidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Um laudo não aparece na listagem mesmo estando cadastrado | O tipo de laudo está inativo no Cadastro Básico, ou o filtro de unidade está errado | Acesse Cadastro Básico > Laudo / Documentação, verifique se o tipo está ativo e vinculado à unidade correta |
| Não encontro onde anexar o arquivo do laudo | O upload não fica na tela de apontamento: fica em Auditoria > Laudo / Documentação | Acesse Auditoria > Laudo / Documentação e faça o upload do arquivo lá |
| O laudo continua VERMELHO após o apontamento | Pode depender da periodicidade do Tipo de Laudo | Confirme a Data Término do apontamento e a periodicidade configurada em Cadastro Básico > Laudo; se persistir, reporte ao suporte técnico |
| A nota de Laudo/Documentação no Dashboard não muda após a atualização | O Dashboard tem cache de 60 segundos | Aguarde até 60 segundos para a atualização automática. Se persistir após 2 minutos, recarregue a página com F5 |
| Não encontro o campo de Valor ao registrar o apontamento | O campo pode estar oculto dependendo da configuração do perfil | Verifique com o Administrador se o campo Financeiro está habilitado para o seu perfil de acesso |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Laudo / Documentação (tipo e periodicidade)Cadastro Básico > Fornecedor | Dashboard: atividade ''Laudo/Documentação'' na tabela de pontuação (seção 7.1)Auditoria Corporativa: índice de conformidade | Alerta automático de vencimento 30 dias antesRegistro de custo no módulo Financeiro |
| Histórico de Laudos (PCM > Histórico — Laudo)Módulo de Estoque (materiais usados) | PMOC: documentação complementar do planoNormas e Procedimentos: repositório técnico | Download do arquivo original em qualquer fiscalização |', NULL, NULL, NULL, NULL, 1);

-- PMOC — Plano de Manutenção, Operação e Controle [PMOC/PMOCIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'PMOC', N'PMOCIndex', N'PMOC — Plano de Manutenção, Operação e Controle', N'3.6 — Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, engenheiros responsáveis (ART) e coordenadores de conformidade. Perfil mínimo: Gestor de PCM. | Menu lateral > PMOC pcmbysim.com.br/PMOC/PMOCIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de cadastrar e manter o Plano de Manutenção, Operação e Controle da unidade em conformidade com a Lei 13.589/2018, monitorar o cronograma de limpeza e manutenção de todos os sistemas de climatização por andar, e gerar o dossiê técnico completo que deve ficar disponível na unidade para apresentação à Vigilância Sanitária em qualquer momento. O PMOC é a exigência legal mais importante relacionada a sistemas de ar condicionado, a ausência ou desatualização do plano pode resultar em embargo sanitário e responsabilização criminal dos gestores.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Todos os equipamentos de ar condicionado precisam estar cadastrados: Cadastro Básico > Ar Condicionado.
> Os setores com carga térmica preenchida precisam estar cadastrados: Cadastro Básico > Setor.
> O engenheiro responsável precisa ter a ART (Anotação de Responsabilidade Técnica) registrada junto ao CREA antes do cadastro no sistema.
> O checklist técnico de PMOC precisa estar criado: Cadastro Básico > Checklist (ver estrutura completa, incluindo periodicidade por item, na seção 2.18).
> Perfil Administrador ou Gestor de PCM é necessário para criar ou editar o plano.


> [!INFO]
> **IMPORTANTE SABER**
> O que é a Lei 13.589/2018?
> Esta lei federal obriga todos os imóveis de uso coletivo (hotéis, hospitais, shoppings, escritórios) a manter um Plano de Manutenção, Operação e Controle para os sistemas de climatização, assinado por engenheiro habilitado. O objetivo é garantir a qualidade do ar interior e prevenir doenças respiratórias relacionadas a sistemas de ar condicionado mal conservados.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'O PMOC no PCM by SIM é dividido em quatro grandes etapas:

| 1️⃣ Plano CADASTRADO | → | 2️⃣ Cronograma GERADO | → | 3️⃣ Execução TÉCNICA | → | 4️⃣ Histórico REGISTRADO | → | 5️⃣ Dossiê TÉCNICO | → | 📊 Renovação do Plano |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |


O módulo PMOC tem cerca de 10 telas reais:

| Tela | Localização | Finalidade |
| :--- | :--- | :--- |
| Cadastro de PMOC | PMOC > Cadastro | Registrar o plano, ART, vigência e responsável técnico |
| Cronograma | PMOC > Cronograma | Matriz numérica de horas/execuções por mês (ver passo 4.2) |
| PMOC (genérica) | PMOC > PMOC | Tela específica para o PMOC Bimestral: hoje usada só por uma rede de hotéis cliente que opera nesse regime; a maioria das unidades usa o Cronograma mensal padrão |
| PMOC: Por Andar | PMOC > PMOC Andar | Visão setorizada por pavimento: facilita logística da equipe |
| PMOC: Agrupado | PMOC > PMOC Agrupado | Visão consolidada de equipamentos similares |
| Histórico: PMOC | PMOC > Histórico | Registro de execuções |
| Histórico: PMOC Agrupado | PMOC > Histórico Agrupado | Segunda tela de histórico, com visão agrupada |
| PMOC Mensal (relatório) | Relatório > PMOC Mensal | Dossiê do mês |
| PMOC Bimestral (relatório) | Relatório > PMOC Bimestral | Relatório bimestral de PMOC |
| Upload PMOC | Upload > PMOC | Importação de dados via planilha |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Cadastrar o plano PMOC e registrar a ART

![Tela PMOC > Cadastro — campos de ART, vigência e responsável técnico](/screenshots/pmoc-cadastro.png)


1. Acesse o menu lateral e clique em PMOC > Cadastro.
1. Clique em Novo Plano.
1. Selecione a Unidade à qual o plano pertence.
1. Preencha o Número da ART, código de registro do engenheiro responsável junto ao CREA. Este é o campo mais importante do cadastro.
1. Defina a Vigência: data de início e data de término do plano. A vigência é tipicamente de 12 meses.
1. Preencha também o Responsável Técnico e, se aplicável, Cliente - Integração / Unidade do Cliente - Integração (campos reais não documentados antes, prováveis de integração externa).
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O plano é criado e os equipamentos de ar condicionado cadastrados para a unidade são automaticamente incluídos no inventário do PMOC.
> O sistema começa a gerar o cronograma de manutenções com base nos equipamentos e tipos de ar condicionado cadastrados.


### 4.2  Visualizar e gerenciar o cronograma

![Tela PMOC > Cronograma — grade de equipamentos x meses com status de execução](/screenshots/pmoc-cronograma-real.png)


1. Acesse PMOC > Cronograma.
1. A tela real NÃO usa grade de 3 cores por célula. É uma matriz com TAG e Descrição do equipamento nas linhas e 12 meses nas colunas, mostrando um valor numérico por célula (execuções/horas, valor 0 quando não há execução), mesmo padrão de matriz já visto no Cronograma Semanal (seção 3.4).
1. Use os filtros Andar, Setor e Tipo de AC para focar em áreas específicas da unidade.
1. Para a visão operacional do dia a dia da equipe técnica, acesse PMOC, Por Andar, ela mostra cada pavimento separadamente, facilitando o deslocamento da equipe.


> [!INFO]
> **RESULTADO ESPERADO**
> Você tem a visão completa do status de conformidade de todos os equipamentos de climatização da unidade.


### 4.3  Executar o apontamento de manutenção PMOC

![Tela de apontamento PMOC — checklist técnico com itens específicos de climatização](/screenshots/pmoc-apontamento.png)


1. No cronograma, clique no equipamento com manutenção pendente.
1. O sistema abre o checklist técnico vinculado ao tipo de equipamento. Exemplos de itens típicos:


Limpeza de filtros, verificar estado e realizar limpeza ou substituição.

Verificação de gás refrigerante, checar pressão e ausência de vazamentos.

Limpeza da bandeja de condensado, evitar proliferação de fungos.

Inspeção de drenos, garantir escoamento adequado.

1. Responda cada item: Conforme, Não Conforme ou Não Aplicável.
1. Para itens Não Conformes, descreva o desvio nas Observações e tire foto de evidência.
1. Ao finalizar todos os itens, o técnico assina digitalmente o apontamento no aplicativo mobile.
1. Clique em Concluir.


> [!INFO]
> **RESULTADO ESPERADO**
> O valor da célula do equipamento no mês correspondente é atualizado no Cronograma (não muda de cor — ver passo 4.2).
> O apontamento é arquivado no Histórico PMOC com checklist preenchido, fotos e assinatura digital.
> A pontuação de ''PMOC'' na tabela de Métricas por Atividade do Dashboard (seção 7.1) é atualizada.


### 4.4  Gerar o dossiê técnico para fiscalização

O dossiê é o conjunto de documentos que a unidade deve apresentar à Vigilância Sanitária em caso de fiscalização. O sistema gera tudo em um único relatório:

1. Acesse PMOC > Relatório.
1. Selecione a Unidade e o Período de vigência do plano.
1. Clique em Exportar / Gerar Relatório.
1. O sistema gera o documento consolidado contendo: dados da ART, inventário de equipamentos, cronograma de manutenções e histórico de execuções com evidências fotográficas.


> [!DANGER]
> O dossiê do PMOC deve ficar impresso e disponível na unidade a qualquer momento, não apenas em formato digital. A Vigilância Sanitária pode solicitar o documento físico durante a fiscalização.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Hotel ou unidade física à qual o plano pertence | Sim | PCM by SIM |
| Número da ART | Código de registro do engenheiro responsável junto ao CREA | Sim | AT-2025-000123 |
| Vigência: Início | Data de início de validade do plano atual | Sim | 01/01/2025 |
| Vigência: Término | Data de expiração do plano: gera alerta de renovação | Sim | 31/12/2025 |
| Equipamento (PMOC) | Aparelho de AC incluído automaticamente via Cadastro Básico | Auto | AC Split TAG AC-302 |
| Tipo de AC | Categoria do equipamento: define o checklist e a periodicidade | Auto | Split Hi-Wall / Fan Coil / Chiller |
| Item do Checklist | Verificação técnica específica do PMOC | Sim | Conforme / Não Conforme / N.A. |
| Assinatura Digital | Confirmação do técnico executor via app mobile | Sim | Gerada automaticamente no app |


Periodicidades mínimas exigidas pela Lei 13.589/2018:

| Tipo de equipamento | Limpeza mínima | Inspeção técnica mínima |
| :--- | :--- | :--- |
| Split, ACJ, Janela | Mensal (filtros) / Semestral (completa) | Anual com ART |
| Fan Coil (sistema central) | Mensal (filtros) / Trimestral (completa) | Anual com ART |
| Chiller | Mensal (filtros) / Semestral (completa) | Anual com ART |
| Self Contained | Mensal (filtros) / Semestral (completa) | Anual com ART |
| Ambientes críticos (UTI, CC) | Quinzenal | Semestral com ART |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Renove o plano PMOC pelo menos 30 dias antes do vencimento, o processo de obtenção de uma nova ART junto ao CREA pode levar semanas.
> Faça o apontamento no sistema imediatamente após a execução física, não ao final do dia. A assinatura digital com horário exato é uma evidência muito mais forte do que um registro feito horas depois.
> Use a visão PMOC, Por Andar para planejar a rota da equipe técnica. Executar todos os aparelhos de um andar de uma vez reduz o deslocamento e aumenta a produtividade.
> Mantenha o inventário de equipamentos sempre atualizado. Cada aparelho novo instalado precisa ser cadastrado imediatamente, aparelhos sem PMOC são irregulares do ponto de vista legal.


> [!DANGER]
> PMOC com ART vencida equivale a não ter PMOC. A validade jurídica do plano depende da vigência da ART, renove antes que expire.
> Não adianta ter o sistema atualizado digitalmente se o dossiê físico não estiver disponível na unidade. A Vigilância Sanitária exige o documento impresso disponível para consulta imediata.
> Equipamentos de ar condicionado instalados em ambientes de hospedagem são considerados de ''uso coletivo'', a lei se aplica independentemente do porte do hotel ou do número de aparelhos.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Um equipamento de AC não aparece no cronograma do PMOC | O equipamento não está cadastrado ou o switch ''Ativo'' está desabilitado | Acesse Cadastro Básico > Ar Condicionado, verifique se o equipamento existe e se está ativo. O PMOC puxa automaticamente todos os aparelhos ativos da unidade |
| O cronograma não está gerando as manutenções nos meses corretos | O tipo de AC não tem periodicidade configurada | Acesse Cadastro Básico > Tipo de Ar Condicionado e verifique se a Periodicidade e o Checklist estão vinculados ao tipo do equipamento |
| O técnico não consegue assinar digitalmente pelo app | O usuário não tem permissão de acesso ao módulo PMOC no app mobile | Verifique em Administração > Usuários se o módulo PMOC está habilitado e se o switch ''Acesso via Aplicativo'' está ativo |
| O relatório do PMOC está incompleto: alguns equipamentos não aparecem | Filtro de período incorreto ou equipamentos inativados durante o período | Revise o filtro de datas e verifique se equipamentos foram inativados no período. Equipamentos inativados saem do relatório mas mantêm o histórico de apontamentos anteriores |
| A nota de PMOC no Dashboard está baixa mesmo com apontamentos em dia | Existem equipamentos ativos no cadastro que não têm nenhum apontamento registrado | Acesse PMOC > Cronograma e filtre por ''Sem apontamento''. Cada equipamento sem execução registrada derruba a nota |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Ar Condicionado (inventário)Cadastro Básico > Tipo de AC (periodicidade)Cadastro Básico > Setor (carga térmica) | Dashboard: atividade ''PMOC'' na tabela de pontuação (seção 7.1)Histórico PMOC: dossiê técnico | Inclusão automática de equipamentos no inventário do planoGeração automática do cronograma ao cadastrar o plano |
| Cadastro Básico > Checklist (itens técnicos)Módulo de Laudo — ART do PMOC | PCM > Preventiva — manutenções do PMOC viram preventivasMódulo Financeiro: custo de manutenção de climatização | Assinatura digital do técnico no app mobileExportação do dossiê para fiscalização |', NULL, NULL, NULL, NULL, 1);

-- Requisição de Serviços [PCM/Requisicao]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'PCM', N'Requisicao', N'Requisição de Serviços', N'3.7 — Solicitante', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:solicitante') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Qualquer colaborador com acesso ao módulo PCM: técnicos, supervisores, recepcionistas e gestores. | Menu lateral > PCM > Requisição pcmbysim.com.br/PCM/Requisicao |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de solicitar formalmente intervenções técnicas, reparos ou materiais que não estavam previstos no planejamento. A requisição é o canal oficial que substitui pedidos informais por rádio, WhatsApp ou verbal, ela garante que toda demanda seja registrada, priorizada e rastreada, e que o gestor de PCM tenha visibilidade total de tudo que a unidade precisa antes de qualquer compra ou mobilização de equipe.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> O setor e a U.H. relacionados ao problema precisam estar cadastrados para vinculação correta.
> Se o problema envolver um equipamento específico, ele precisa existir no sistema para o vínculo ser feito.
> Tenha uma descrição clara do problema antes de abrir a requisição, quanto mais precisa, mais rápida será a aprovação e a execução.
> Para requisições de material (compra), o produto precisa estar cadastrado em Cadastro Básico > Produto.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'A requisição percorre um ciclo de aprovação antes de gerar ação:

| 1️⃣ Solicitação AGUARDANDO | → | 2️⃣ Análise do GESTOR | → | 3️⃣ Aprovada? OS/OC GERADA | → | 4️⃣ Reprovada? Solicitante NOTIFICADO | → | 5️⃣ Histórico REGISTRADO | → | 📊 Auditável |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |


> [!INFO]
> **IMPORTANTE SABER**
> Diferença entre Requisição e Ordem de Serviço:
> Requisição: pedido formal de intervenção ou material, passa por aprovação antes de virar ação.
> Ordem de Serviço: a ação em si, técnico alocado, prazo definido, execução em campo.
> Uma requisição aprovada pode gerar automaticamente uma OS (serviço) ou seguir para o módulo de Estoque como Requisição de Compra (material).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Abrir uma nova requisição

![Tela PCM > Requisição — formulário de abertura com campos de prioridade, setor e descrição](/screenshots/requisicao-abertura.png)


1. Acesse PCM > Requisição.
1. Clique em Novo.
1. Selecione a Unidade onde o serviço ou material é necessário.
1. Defina a Prioridade: Baixa, Média, Alta ou Crítica. Seja honesto, prioridades infladas dificultam a gestão real do gestor.
1. Preencha o Setor e, se aplicável, a U.H. ou o Equipamento relacionado ao problema.
1. No campo Descrição, seja objetivo e técnico. Inclua: o que está errado, onde está, quando foi percebido e se há impacto imediato na operação.
1. Se tiver foto do problema, use Anexo para incluí-la, aumenta a velocidade de análise pelo gestor.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A requisição entra com status PENDENTE (badge amarelo) na listagem. O card do formulário desta tela mostra, por bug conhecido, o título "Ordem de Serviço" em vez de "Requisição" — mas é mesmo uma requisição, com numeração própria.
> O gestor de PCM recebe a notificação para análise.


### 4.2  Acompanhar o status da requisição

1. Acesse PCM > Requisição e localize sua solicitação na listagem.
1. Observe a coluna de Status:


| Status | Cor | Significado | O que fazer |
| :--- | :--- | :--- | :--- |
| PENDENTE | 🟡 Amarelo | Enviada, aguardando análise do gestor | Aguardar: não reabrir a mesma requisição |
| APROVADO | 🟢 Verde | Deveria gerar OS ou OC automaticamente — ver aviso abaixo sobre a tela de aprovação | Acompanhar a execução na tela de OS |
| REPROVADO | 🔴 Vermelho | Negada pelo gestor | Ler o motivo nas observações e avaliar nova abordagem |


> [!DANGER]
> **ATENÇÃO — aprovação sem controle funcional nesta tela**
> A tela PCM > Requisição (Aprovar/Reprovar) não tem nenhum botão de aprovar ou reprovar que funcione — só uma lista com checkboxes que não acionam nada. Isso significa que hoje uma requisição pode ficar PENDENTE indefinidamente, sem virar OS. Até a correção, trate a aprovação como um processo que precisa ser confirmado verbalmente com o gestor, não apenas pelo status no sistema.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Hotel onde o serviço ou material é necessário | Sim | PCM by SIM |
| Prioridade | Nível de urgência da necessidade | Sim | Baixa / Média / Alta / Crítica |
| Setor | Área física onde o problema ocorre | Sim | Cozinha / Lobby / Quarto 302 |
| U.H. | Unidade habitacional, se aplicável | Não | 302 |
| Equipamento | Ativo envolvido, se aplicável | Não | AC Split TAG AC-302 |
| Descrição | Relato claro do problema ou necessidade | Sim | Compressor do AC do quarto 302 não liga desde ontem |
| Anexo | Foto ou documento de suporte: acelera análise do gestor | Não | foto_compressor.jpg |
| Data | Preenchida automaticamente com a data atual | Auto | 27/05/2025 |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Abra uma requisição por problema. Misturar múltiplas necessidades em uma única requisição dificulta a análise e pode fazer com que parte do pedido seja aprovado e outra reprovada sem clareza.
> Prioridade é comunicação com o gestor, use Crítica apenas quando o problema está impactando hóspedes ou criando risco de segurança. O excesso de Críticas desvaloriza o indicador.
> Descreva o impacto operacional: ''Ar condicionado inoperante no quarto 302, hóspede em check-in hoje às 14h'' é muito mais acionável do que ''AC com defeito''.


> [!DANGER]
> Não abra múltiplas requisições para o mesmo problema ao não receber resposta imediata. Isso polui a fila do gestor e dificulta a priorização real. Se for urgente, comunique diretamente ao gestor e informe o número da requisição já aberta.
> Requisições reprovadas ficam no histórico permanentemente, nunca tente ''disfarçar'' uma requisição reprovada abrindo uma nova com descrição diferente para o mesmo problema sem contexto.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A requisição ficou em AGUARDANDO por mais de 24h sem resposta | O gestor não recebeu a notificação ou está com acúmulo na fila | Comunique diretamente ao gestor informando o número da requisição. Não abra uma nova requisição para o mesmo problema |
| Não consigo selecionar o equipamento no formulário | O equipamento não está cadastrado ou está inativo | Acesse Cadastro Básico > Máquinas/Equipamentos e verifique se o ativo existe e está ativo. Se não existir, descreva o equipamento no campo Descrição |
| A requisição foi aprovada mas nenhuma OS foi gerada | A aprovação foi feita sem vincular a ação de geração de OS pelo gestor | Comunique ao gestor que a requisição está aprovada mas sem OS gerada: o gestor precisa criar a OS manualmente ou vincular pelo módulo de aprovação |
| Não encontro minha requisição na listagem | O filtro de status está mostrando apenas um tipo (ex: Aguardando) e a requisição está em outro status | Remova todos os filtros de status para ver todas as requisições abertas por você |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Qualquer usuário com acesso ao módulo PCMCadastro Básico > Setor / U.H. / Equipamento | PCM > Aprovação de Requisições (seção 3.8) — fila do gestorOS: gerada após aprovaçãoEstoque > Requisição de Compra — para necessidades de material | Notificação automática ao gestor ao abrir nova requisição |
| Cadastro Básico > Prioridades | Dashboard: se configurado para mostrar volume de requisições pendentes | Aprovação deveria gerar OS automaticamente — ver aviso na seção 4.2 sobre a tela de aprovação |', NULL, NULL, NULL, NULL, 1);

-- Aprovação de Requisições [PCM/Requisicao]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'PCM', N'Requisicao', N'Aprovação de Requisições', N'3.8 — Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM e coordenadores com autoridade para liberar recursos e mobilizar equipe. | Menu lateral > PCM > Requisição (Aprovar / Reprovar) pcmbysim.com.br/PCM/RequisicaoAprovarReprovarIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de analisar, aprovar ou reprovar as solicitações da equipe de forma organizada e rastreável, com critérios claros de decisão. O painel de aprovação é o ''filtro financeiro'' da unidade: garante que nenhuma compra ou mobilização de equipe aconteça sem autorização prévia, permite priorizar o que realmente importa e cria um histórico auditável de todas as decisões gerenciais.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Você precisa ter perfil com permissão de aprovação: Gestor de PCM ou superior.
> Tenha o orçamento disponível e as prioridades operacionais da semana em mente antes de analisar as requisições, decisões sem contexto geram aprovações inconsistentes.
> Para requisições de material, verifique o saldo em estoque antes de aprovar uma compra, pode já haver o item disponível.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'A aprovação fecha o ciclo iniciado pela abertura de requisição e dispara a ação correspondente:

| 1️⃣ Requisição AGUARDANDO | → | 2️⃣ Análise do GESTOR | → | 3️⃣ Aprovada? OS/OC GERADA | → | 4️⃣ Reprovada? Solicitante NOTIFICADO | → | 5️⃣ Motivo REGISTRADO | → | 📊 Fila Atualizada |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'> [!DANGER]
> **ATENÇÃO — mesmo problema já documentado na seção 3.7**
> A URL desta seção (/PCM/RequisicaoAprovarReprovarIndex) é exatamente a mesma da seção 3.7 (Requisição de Serviços) — mesmos registros, mesmo problema. **Não existe nenhum botão de Aprovar ou Reprovar nesta tela.** A listagem real só tem 3 colunas (Nº Requisição, Data, Descrição), não as 7 descritas abaixo. Não existem os filtros de Prioridade ou Solicitante, nem checkbox de seleção múltipla, nem botão de aprovação em massa. Todo o passo a passo abaixo (4.1 a 4.4) descreve como o fluxo deveria funcionar quando a tela for corrigida — hoje essa funcionalidade não está disponível.


### 4.1  Acessar e filtrar a fila de aprovações

![Tela PCM > Aprovação de Requisições — listagem com filtros e botões de aprovação em massa](/screenshots/requisicao-aprovacao.png)


1. Acesse PCM > Requisição (Aprovar / Reprovar).
1. Utilize os filtros para organizar a análise:


Prioridade, comece sempre pelas Críticas e Altas.

Unidade, se você gerencia múltiplas unidades, filtre uma por vez.

Solicitante, útil para entender padrões de demanda por colaborador.

Local / U.H., para focar em áreas com maior demanda no momento.

1. Clique em Filtrar para atualizar a listagem.


> [!INFO]
> **RESULTADO ESPERADO**
> Você vê todas as requisições pendentes organizadas por prioridade, com informações de setor, solicitante e data.


### 4.2  Analisar e aprovar uma requisição

1. Leia a Descrição completa da requisição. Se houver foto anexada, visualize antes de decidir.
1. Avalie três critérios antes de aprovar:


Urgência real: o problema está impactando hóspedes ou a operação agora?

Recursos disponíveis: há técnico disponível? Há orçamento para o material?

Alternativas: o problema pode ser resolvido com recursos já disponíveis?

1. Se aprovada: marque a requisição e clique em Aprovar.
1. O sistema perguntará se deseja gerar uma OS automaticamente. Confirme para criar a OS já com os dados da requisição preenchidos.


> [!INFO]
> **RESULTADO ESPERADO**
> A requisição muda para status APROVADO (verde).
> O solicitante recebe notificação da aprovação.
> Se confirmada a geração de OS, ela já aparece na fila de pendências da equipe.


### 4.3  Reprovar uma requisição

1. Selecione a requisição e clique em Reprovar.
1. O sistema exigirá o preenchimento do campo Motivo da Reprovação. Sempre justifique, o solicitante precisa entender por que o pedido foi negado.
1. Clique em Confirmar.


> [!INFO]
> **RESULTADO ESPERADO**
> A requisição muda para REPROVADO (vermelho) com o motivo registrado.
> O solicitante recebe a notificação com a justificativa.
> O histórico da decisão fica disponível para auditoria.


### 4.4  Aprovação em massa

Para situações com muitas requisições de baixo risco acumuladas (ex: materiais de consumo rotineiros), o sistema permite aprovação múltipla:

1. Marque as caixas de seleção à esquerda de cada requisição que deseja aprovar em lote.
1. Clique em Aprovar Selecionadas.
1. Confirme a ação, todas as selecionadas são aprovadas simultaneamente.


> [!DANGER]
> Use a aprovação em massa com critério. Aprovar sem ler cada requisição individualmente pode gerar custos não planejados ou mobilizar a equipe para tarefas não prioritárias.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Nº Requisição | Identificador único da solicitação | Auto | REQ-2025-0412 |
| Data | Quando a requisição foi aberta | Auto | 27/05/2025 |
| Solicitante | Colaborador que abriu a requisição | Auto | Carlos Oliveira |
| Prioridade | Nível de urgência definido pelo solicitante | Auto | Alta |
| Setor / U.H. | Localização do problema | Auto | Quarto 302 |
| Equipamento | Ativo relacionado, se vinculado | Auto | AC Split TAG AC-302 |
| Descrição | Relato do problema pelo solicitante | Auto | Compressor não liga desde ontem |
| Motivo Reprovação | Justificativa obrigatória ao reprovar | Cond. | Peça em estoque: usar TAG EST-0023 |
| Ordem de Compra | Número da OC gerada após aprovação de material | Auto | OC-2025-0089 |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Processe a fila de aprovações pelo menos uma vez por turno, requisições com mais de 24h sem resposta criam a percepção de que o sistema não funciona e voltam via comunicação informal.
> Sempre justifique reprovações com clareza e, quando possível, indique a alternativa: ''Reprovado, item disponível no estoque, caixinha A3'' é infinitamente mais útil do que apenas ''Reprovado''.
> Use os filtros de Prioridade como sua agenda: Crítica → Alta → Média → Baixa. Nunca processe em ordem cronológica pura.
> Ao aprovar uma requisição de compra, verifique antes o saldo no estoque. Comprar o que já se tem desperdiça orçamento e espaço físico.


> [!DANGER]
> Aprovar requisições sem analisar o orçamento disponível é a causa mais comum de estouro de budget no final do mês. Cada aprovação tem custo, trate como decisão financeira, não operacional.
> Reprove com motivo sempre. Uma reprovação sem justificativa gera atrito desnecessário com a equipe e pode levar o solicitante a encontrar formas informais de contornar o processo.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A fila de aprovações está sempre cheia: nunca diminui | Volume de requisições maior que a capacidade de análise, ou muitas requisições de baixa prioridade acumulando | Configure um ciclo fixo: processar fila toda manhã e toda tarde. Para requisições de baixo valor e baixo risco, delegue a aprovação a um nível hierárquico abaixo |
| O sistema não gera a OS automaticamente após a aprovação | A opção de geração automática foi desmarcada durante a aprovação | Acesse OS > Listagem, clique em Novo e crie a OS manualmente vinculando à requisição aprovada pelo número |
| Não consigo reprovar: o botão está desabilitado | O perfil não tem permissão de reprovação, ou a requisição já foi aprovada por outro gestor | Verifique seu perfil de acesso com o Administrador. Se aprovada por outro gestor, é necessário cancelar a OS gerada separadamente |
| Recebi notificação de requisição mas ela não aparece na minha fila | A requisição foi enviada para outra unidade ou o filtro está ocultando | Remova todos os filtros da tela e verifique todas as unidades. Se ainda não aparecer, confirme com o solicitante qual unidade foi selecionada ao abrir |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| PCM > Requisição (seção 3.7) — solicitação do colaboradorPerfil > Hierarquia (seção 1.4) — define quem aprova | OS: gerada automaticamente após aprovação de serviçoEstoque > Requisição de Compra — gerada após aprovação de materialHistórico de Requisições: registro auditável | Notificação ao solicitante com status da decisãoGeração opcional de OS com dados herdados da requisição |
| Cadastro Básico > Prioridades — define urgência na fila | Módulo Financeiro: aprovações de compra impactam o budgetRelatório Mensal PCM: volume de aprovações e reprovações | Aprovação em massa para lotes de baixo risco |', NULL, NULL, NULL, NULL, 1);

-- Registro de Faltas [PCM/FaltaIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'PCM', N'FaltaIndex', N'Registro de Faltas', N'3.9 — Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, supervisores de turno e RH. | Menu lateral > PCM > Falta pcmbysim.com.br/PCM/FaltaIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar formalmente as ausências dos técnicos no sistema, garantindo que o planejamento de manutenção reflita a disponibilidade real da equipe. Sem o registro de faltas, o sistema continua alocando tarefas a técnicos ausentes, OS ficam paradas sem responsável, preventivas acumulam atraso e o Dashboard mostra conformidade irreal. O registro correto protege o planejamento e fornece dados de absenteísmo que auxiliam decisões de contratação e gestão de escala.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> As justificativas de falta precisam estar cadastradas: Cadastro Básico > Justificativa, Falta.
> O colaborador precisa estar ativo no sistema: Cadastro Básico > Colaborador.
> Registre a falta antes de o turno começar sempre que possível, isso evita que OS sejam atribuídas a técnicos ausentes.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Falta COMUNICADA | → | 2️⃣ Registro no SISTEMA | → | 3️⃣ OS REDISTRIBUÍDA | → | 4️⃣ Dado de ABSENTEÍSMO | → | 📊 Retorno Registrado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Registrar uma falta

![Tela PCM > Falta — formulário com colaborador, período e justificativa](/screenshots/registro-falta.png)


1. Acesse PCM > Falta e clique em Novo.
1. Selecione o Colaborador que está ausente.
1. Defina a Data Início / Hora Início e a Data Término / Hora Término da ausência, 4 campos reais, não só 2 datas. Para faltas de um único dia, as datas são iguais.
1. Selecione a Justificativa, Falta correspondente ao motivo da ausência. As opções reais confirmadas são: Atestado, Falta Injustificada, Férias, Férias Coletivas, Folga - Maternidade, Folga - Paternidade, nomenclatura diferente da sugerida em versões antigas deste manual (Afastamento Médico, Licença, Compensação de Banco de Horas, Treinamento Externo não existem como opções).
1. Se houver documento (ex.: atestado médico), anexe no campo Arquivo (upload real não documentado antes).
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O colaborador é retirado automaticamente da lista de executores disponíveis durante o período registrado.
> Novas OS não serão atribuídas a ele enquanto a falta estiver ativa.
> O dado alimenta os relatórios de absenteísmo da unidade.


### 4.2  Consultar e filtrar o histórico de faltas

1. Na tela principal de Faltas, utilize os filtros: Unidade, Colaborador, Período e Justificativa.
1. Clique em Filtrar para atualizar a listagem.
1. Para exportar o histórico para análise de RH, use o botão Exportar (Excel / PDF).


> [!INFO]
> **IMPORTANTE SABER**
> Justificativas reais:
> Atestado, atestado de saúde (INSS ou particular).
> Falta Injustificada, ausência sem comunicação ou documentação.
> Férias, ausência programada com aviso prévio.
> Férias Coletivas, período de férias coletivas da unidade.
> Folga - Maternidade, licença maternidade.
> Folga - Paternidade, licença paternidade.


### 4.3  Cadastrar Justificativas de Falta (cadastro auxiliar)

1. Acesse Cadastro Básico > Justificativa, Falta e clique em Novo.
1. Preencha a Descrição da justificativa (ex.: Atestado, Falta Injustificada, Férias).
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A nova justificativa passa a aparecer na lista de seleção do passo 4.1, ao registrar uma falta.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade física do colaborador ausente | Sim | Intercity Berrini |
| Colaborador | Técnico ou funcionário ausente | Sim | Carlos Oliveira |
| Data Início | Primeiro dia da ausência | Sim | 27/05/2025 |
| Data Término | Último dia da ausência: igual ao início para falta de 1 dia | Sim | 27/05/2025 |
| Hora Início / Hora Término | Campos reais de horário, além das datas | Não | 08:00 / 17:00 |
| Justificativa | Motivo da ausência: selecionado da lista pré-cadastrada | Sim | Afastamento Médico: INSS |
| Observação | Detalhes adicionais sobre a ausência, se necessário | Não | Atestado entregue ao RH |
| Arquivo | Upload de documento (ex.: atestado): campo real não documentado antes | Não | atestado.pdf |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Registre férias e licenças com pelo menos uma semana de antecedência, isso permite que o gestor reorganize o cronograma antes do impacto na operação.
> Após registrar uma falta, revise imediatamente as OS atribuídas ao técnico ausente no dia e redistribua para outro executor disponível.
> Use o histórico de faltas mensalmente em reuniões de gestão de pessoas, padrões de absenteísmo por colaborador ou por período são dados valiosos para RH.


> [!DANGER]
> OS atribuídas a um técnico em falta ficam ''travadas'' sem executor ativo, elas aparecem no Dashboard como PENDENTE mesmo sem ninguém para executá-las. Reatribua manualmente após registrar a falta.
> Não use o campo de faltas para registrar treinamentos externos ou eventos corporativos que tirem o técnico da unidade, crie uma justificativa específica para cada tipo de ausência para que os relatórios sejam precisos.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O colaborador continua aparecendo como disponível para OS mesmo com falta registrada | A data da falta não cobre o dia atual, ou a falta foi registrada após a atribuição da OS | Verifique o período registrado. Para OS já atribuídas antes do registro da falta, edite manualmente o executor da OS |
| A justificativa de falta desejada não aparece na lista | A justificativa não foi cadastrada ou está inativa | Acesse Cadastro Básico > Justificativa — Falta e crie ou reative a justificativa necessária |
| Não consigo editar uma falta já registrada | Perfil sem permissão de edição ou falta já encerrada | Solicite ao Administrador a edição. Para faltas encerradas, registre uma nova falta com o período corrigido |
| O relatório de absenteísmo não reflete as faltas do mês | As faltas foram registradas com justificativa errada ou fora do período filtrado | Revise o filtro de período e verifique se as justificativas usadas são as corretas para cada tipo de ausência |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Colaborador (equipe ativa)Cadastro Básico > Justificativa — Falta | PCM > Cronograma Semanal — disponibilidade da equipeOS: executor disponível para atribuiçãoRelatórios de RH: absenteísmo | Remoção automática do colaborador da lista de executores disponíveis durante o período |
| Calendário operacional da unidade | Planejamento de contratações e escala do RH | Exportação de histórico para análise gerencial |', NULL, NULL, NULL, NULL, 1);

-- Gestão de Estoque [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Gestão de Estoque', N'4.1 — Almoxarife', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:almoxarife') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de Almoxarifado, técnicos de PCM e gestores financeiros. | Menu lateral > Estoque pcmbysim.com.br/Estoque/Entrada |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de controlar todas as movimentacoes de materiais da unidade: registrar entradas de notas fiscais, apontar saidas vinculadas a OS, realizar inventarios fisicos e consultar o saldo em tempo real. O módulo de estoque e o que fecha o triangulo de custo da manutenção: quando o técnico aponta os materiais usados em uma OS, o sistema faz a baixa automática e registra o custo. Sem o estoque operando corretamente, os relatorios de custo de manutenção são incompletos.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Os produtos precisam estar cadastrados: seção 2.16.
> Os fornecedores precisam estar cadastrados: seção 2.15.
> Para vincular saidas a OS, as ordens de serviço precisam estar abertas no sistema: seção 3.1.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Produto CADASTRADO | → | 2️⃣ Entrada REGISTRADA | → | 3️⃣ Saldo DISPONÍVEL | → | 4️⃣ Saída REGISTRADA | → | 5️⃣ Custo na OS | → | 📊 Inventário Periódico |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1.A  Registrar entrada de material (recebimento de nota fiscal)

![Estoque > Entrada — campos de número de NF, fornecedor e lista de produtos](/screenshots/estoque-entrada.png)


1. Acesse Estoque > Entrada e clique em Novo.
1. Selecione a Unidade e preencha o Número do Documento (número da Nota Fiscal ou comprovante de entrega).
1. Se a entrada veio de uma Ordem de Compra aprovada, vincule no campo Ordem de Compra.
1. Selecione o Fornecedor responsável pela entrega.
1. Adicione os produtos: selecione o Produto, informe a Quantidade e o Preco Unitario real da nota.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O saldo de cada produto e incrementado automaticamente.
> O preco médio do produto e recalculado com base nas entradas acumuladas.
> A entrada fica registrada no histórico com o número da NF e o fornecedor para rastreabilidade.


### 4.1.B  Registrar saida de material (consumo em OS)

![Estoque > Saida — campos de OS vinculada, produto e quantidade retirada](/screenshots/estoque-saida.png)


1. Acesse Estoque > Saida e clique em Novo.
1. Selecione a Unidade e o Usuário / Executor que está retirando o material.
1. No campo Ordem de Serviço, vincule a OS para a qual o material está sendo utilizado — esse é o passo que registra o custo na OS.
1. Adicione os produtos: selecione o Produto e informe a Quantidade retirada.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O saldo do produto e decrementado automaticamente.
> O custo do material e registrado na OS vinculada.
> Se o saldo atingir o Ponto de Reposicao, o sistema gera alerta de recompra.


> [!WARNING]
> **CUSTO DE MATERIAL NÃO APARECE NA OS**
> O vínculo entre a Saída e a OS é gravado corretamente, mas o custo ainda não aparece de volta na tela da OS (nem no Apontamento, nem na visualização formal). Hoje o único jeito de conferir o gasto de uma OS é filtrar o histórico de Saída de Material pelo número dela. O alerta de recompra também ainda não existe como notificação — hoje é um contador na tela de Requisição de Compra (ver seção 4.2).


### 4.1.C  Realizar inventario físico

1. Acesse Estoque > Inventario e clique em Download de Planilha.
1. A planilha traz o saldo atual do sistema para cada produto. Realize a contagem física e preencha a coluna de Quantidade Real.
1. Suba a planilha ajustada pelo campo de Upload. O sistema corrige os saldos automaticamente.


> [!INFO]
> **IMPORTANTE SABER**
> Realize inventarios fisicos mensalmente ou bimestralmente. Divergencias entre o saldo do sistema e o físico sinalizam: falhas no processo de baixa (saidas sem registro), perdas não registradas, ou furtos.


Entrada de material:

| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| No Documento | Número da Nota Fiscal ou comprovante | Sim | NF 001234 |
| Ordem de Compra | Vinculo com OC aprovada: se aplicavel | Não | OC-2025-0089 |
| Fornecedor | Empresa que forneceu o material | Sim | Engetec Manutenção |
| Produto | Item recebido: selecionado do catalogo | Sim | Filtro HEPA 300x300 G4 |
| Quantidade | Volume recebido nesta entrada | Sim | 10 unidades |
| Preco Unitario | Valor real da nota: atualiza preco médio | Sim | R$ 35,00 |


Saida de material:

| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Ordem de Serviço | OS para a qual o material está sendo utilizado | Não | OS-2025-1042 |
| Usuario/Executor | Quem está retirando o material do almoxarifado | Sim | Carlos Oliveira |
| Produto | Item retirado: selecionado do catalogo | Sim | Filtro HEPA 300x300 G4 |
| Quantidade | Volume retirado nesta saida | Sim | 2 unidades |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Nº Documento | Número da Nota Fiscal ou comprovante | Sim | NF 001234 |
| Fornecedor | Empresa que forneceu o material | Sim | Engetec Manutenção |
| Produto | Item recebido ou retirado: selecionado do catálogo | Sim | Filtro HEPA 300x300 G4 |
| Quantidade | Volume recebido ou retirado | Sim | 10 unidades |
| Ordem de Serviço | OS vinculada à saída: registra custo na OS | Não | OS-2025-1042 |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Sempre vincule a saída a uma OS, é a única forma de calcular o custo real de cada manutenção.
> Fotografe a nota fiscal antes de registrar a entrada e guarde o original físico.
> Configure o Ponto de Reposição para todos os itens críticos antes de iniciar a operação.


> [!DANGER]
> Nunca ajuste o saldo de um produto diretamente sem passar pelo Inventário, isso quebra o rastreamento entre saldo e custo, e impede a auditoria de divergências futuras.
> O campo Ordem de Serviço na Saída de Material é opcional e o sistema NÃO avisa se ele ficar vazio, é fácil esquecer de preencher, e sem ele não há como rastrear depois o custo daquela OS. Trate como rotina obrigatória da equipe, mesmo não sendo obrigatório no sistema.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O produto não aparece para seleção na tela de saida | Produto inativo, saldo zerado com filtro ativo, ou unidade diferente | Verifique se o produto está ativo e se há saldo disponível. Remova o filtro de saldo se estiver ativo |
| A baixa de estoque não aconteceu após apontar materiais na OS | O material foi descrito no campo de texto livre da OS, não no módulo de Saida | Acesse Estoque > Saida, crie uma saida vinculando a OS e o produto correto |
| O saldo físico e diferente do sistema após o inventario | Saidas realizadas sem registro, ou entradas sem nota registradas | Corrija o saldo via Upload de Inventario. Implante o processo de registrar toda movimentacao em tempo real |
| O custo de materiais da OS está zerado | A saida não foi vinculada a OS, ou o preco unitario do produto estava zerado | Registre a saida no Estoque vinculando a OS. Verifique o preco unitario do produto no Cadastro Básico |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Produto (catalogo)Cadastro Básico > Fornecedor (origem)Estoque > Ordem de Compra (compras aprovadas) | OS: custo de material automático Módulo Financeiro: valorizacao do estoque Requisição de Compra: alerta de estoque mínimo | Saldo atualizado em tempo real após cada entrada ou saidaAlerta de reposicao ao atingir o Ponto de Reposicao |
| Movimentacoes e histórico de fornecimentos | Dashboard: indicadores de custo por equipamentoRelatorios de BI: custo de material por OS e por departamentoInventario: base para contagem física periódica | Preco médio recalculado automaticamente a cada entrada |', NULL, NULL, NULL, NULL, 1);

-- Requisição de Compra [Estoque/RequisicaoCompra]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Estoque', N'RequisicaoCompra', N'Requisição de Compra', N'4.2 — Almoxarife', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:almoxarife') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de Almoxarifado, PCM e qualquer colaborador autorizado a solicitar materiais. | Menu lateral > Estoque > Requisição de Compra pcmbysim.com.br/Estoque/RequisicaoCompra |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de formalizar solicitações de compra de materiais, iniciando o fluxo de suprimentos da unidade. A Requisição de Compra e o documento que evita que as compras acontecam de forma informal, ela registra o que precisa ser comprado, quem pediu, qual a urgência e aguarda aprovacao antes de qualquer gasto. Existe tambem o modo automático: o sistema gera a lista de itens abaixo do estoque mínimo com um clique.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Produtos com Ponto de Reposição configurado permitem o modo automático de geração de requisições: seção 2.16.
> O gestor responsável pela aprovação precisa estar vinculado como Gestor de Departamento: seção 2.12.
> Para o modo automático funcionar, o Ponto de Reposição precisa estar preenchido em cada produto.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Necessidade IDENTIFICADA | → | 2️⃣ Requisição ABERTA | → | 3️⃣ Aprovação do GESTOR | → | 4️⃣ Ordem de Compra GERADA | → | 5️⃣ Compra REALIZADA | → | 📊 Entrada no Estoque |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Tela de Requisição de Compra - listagem com status AGUARDANDO APROVACAO, APROVADO e REPROVADO](/screenshots/estoque-requisicao-compra-listagem.png)


1. Acesse Estoque > Requisição de Compra e clique em Novo.
1. Para requisição manual: selecione os produtos necessários e informe a quantidade, não existe campo de Justificativa no formulário real.
1. Para requisição automática: clique em Novo, Ponto de Reposição. O sistema lista automaticamente todos os itens com saldo abaixo do mínimo.
1. Revise os itens listados e ajuste as quantidades se necessário.
1. Clique em Salvar.


> [!DANGER]
> **ATENÇÃO — bug conhecido bloqueia o Salvar nos 2 modos**
> Hoje o Salvar falha com erro do servidor tanto no modo manual quanto no automático, para praticamente qualquer produto do catálogo real (bug de mapeamento de Unidade de Medida). Até a correção, trate a criação de Requisição de Compra pelo sistema como indisponível e comunique a necessidade diretamente ao gestor de Almoxarifado por fora do sistema.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Status | Cor | Significado | Próxima acao |
| :--- | :--- | :--- | :--- |
| AGUARDANDO APROVACAO | Azul | Salva e aguardando análise do gestor | Aguardar resposta: não reabrir a mesma solicitação |
| APROVADO | Verde | Autorizada: pronta para gerar OC | Acessar Estoque > Ordem de Compra |
| REPROVADO | Vermelho | Negada: motivo nas observacoes | Ler motivo e avaliar nova abordagem |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Use o modo automático de Ponto de Reposicao semanalmente, ele garante que nada passe do mínimo sem que uma requisição seja gerada.
> Use o botão Aprovar (✓) direto na listagem, não é preciso ir a nenhuma outra tela do PCM para aprovar uma Requisição de Compra (ver correção da seção Conexões abaixo).


> [!DANGER]
> Não abra multiplas requisicoes para o mesmo item ao não receber resposta, isso polui a fila do gestor. Comunique diretamente informando o número da requisição já aberta.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Requisição em AGUARDANDO há mais de 24h | Gestor não recebeu a notificação ou fila acumulada | Comunique diretamente ao gestor informando o número. Não abra nova requisição para o mesmo item |
| Modo automático não lista itens esperados | Produtos sem Ponto de Reposição ou saldo ainda acima do mínimo | Edite os produtos em Cadastro Básico > Produto e preencha o campo Ponto de Reposição |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Produto — ponto de reposicao O botão Aprovar (✓) na própria listagem desta tela abre um modal com Ordem Compra, Fornecedor e Arquivo (não é preciso ir a PCM > Aprovação de Requisições, que é a tela de aprovação de um sistema diferente — o de Requisição de Serviços da seção 3.7) | Estoque > Ordem de Compra — próxima etapa após aprovacao Módulo Financeiro: controle de budget de compras | Notificação automática ao gestor ao salvar a requisição |', NULL, NULL, NULL, NULL, 1);

-- Ordem de Compra [Estoque/OrdemCompra]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Estoque', N'OrdemCompra', N'Ordem de Compra', N'4.3 — Almoxarife', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:almoxarife') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Setor de Compras, gestores financeiros e gestores de almoxarifado. | Menu lateral > Estoque > Ordem de Compra pcmbysim.com.br/Estoque/OrdemCompra |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de formalizar o pedido junto ao fornecedor, controlar o que foi entregue e identificar pendencias. A Ordem de Compra e o documento que oficializa a compra aprovada: ela registra o fornecedor, os itens pedidos, as quantidades, o que já foi recebido e o que ainda está pendente de entrega. Sem o controle via OC, a unidade paga por itens não recebidos e não tem como cobrar atrasos ou entregas incompletas dos fornecedores.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A Requisição de Compra precisa estar aprovada antes de gerar a OC: seção 4.2.
> O fornecedor precisa estar cadastrado: seção 2.15.
> Tenha o orçamento ou proposta do fornecedor em mãos para validar os valores.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Requisição APROVADA | → | 2️⃣ OC CRIADA | → | 3️⃣ Pedido ENVIADO | → | 4️⃣ Material RECEBIDO | → | 📊 OC Concluída |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Estoque > Ordem de Compra — listagem com status, quantidades pedidas, recebidas e pendentes](/screenshots/estoque-ordem-compra.png)


1. A Ordem de Compra é gerada automaticamente pelo sistema quando uma Requisição de Compra é aprovada, não existe botão Novo nesta tela. Estoque > Ordem de Compra é uma tela só de consulta.
1. Use os filtros (Unidade, Fornecedor, Data, Status) para localizar a OC gerada a partir da sua requisição aprovada.
1. Acompanhe o Status e as quantidades Pedida / Recebida / Pendente diretamente na listagem.
1. Quando o material chegar, acesse Estoque > Entrada e vincule a OC, o sistema atualiza as quantidades recebidas e pendentes automaticamente.


> [!DANGER]
> Não existe nenhum caminho na interface para criar uma OC manualmente. Hoje a listagem fica vazia porque nenhuma Requisição de Compra consegue ser aprovada com sucesso (ver o bug conhecido na seção 4.2) — a OC só vai aparecer aqui depois que esse bug for corrigido.


> [!INFO]
> **RESULTADO ESPERADO**
> Status da OC atualizado automaticamente: AGUARDANDO RECEBIMENTO, RECEBIDO, RECEBIMENTO PARCIAL ou RECEBIMENTO A MAIOR, nomes reais confirmados no filtro da listagem, diferentes do Aberto/Recebido Parcial/Concluido de versões antigas deste manual.
> A OC fica no histórico para auditoria e controle de garantias.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Ordem Compra | Número identificador único da OC | --- | OC-2025-0089 |
| Fornecedor | Empresa responsável pela entrega | --- | Engetec Manutenção |
| Status | Fase atual do pedido: 4 valores reais confirmados no filtro | --- | AGUARDANDO RECEBIMENTO / RECEBIDO / RECEBIMENTO PARCIAL / RECEBIMENTO A MAIOR |
| Quantidade | Total de itens pedidos na OC | --- | 50 unidades |
| Qtde. Recebida | Itens já entregues e com entrada no estoque | --- | 30 unidades |
| Qtde. Pendente | Saldo aguardando entrega | --- | 20 unidades |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Sempre cobre o fornecedor por itens pendentes ao final do prazo de entrega, o histórico da OC e a prova formal de que o pedido foi feito e o prazo foi descumprido.
> Para fornecedores frequentes, mantenha o histórico de OCs anterior para negociar melhores prazos e precos nas proximas compras.


> [!DANGER]
> Não feche uma OC como Concluída antes de todas as quantidades serem recebidas. Uma OC fechada manualmente com pendencias oculta itens não entregues e pode comprometer auditorias de inventario.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Fornecedor não recebeu a OC | E-mail do fornecedor desatualizado ou incorreto | Verifique o e-mail em Cadastro Básico > Fornecedor e reenvie manualmente |
| Quantidade recebida não bate com a OC | Entrega parcial pelo fornecedor | Registre apenas o que foi efetivamente recebido. O saldo pendente fica visível na OC até o recebimento total |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Estoque > Requisição de Compra (aprovada)Cadastro Básico > Fornecedor | Estoque > Entrada — vincular recebimento a OC Módulo Financeiro: controle de compromisso orcamentario | Quantidade recebida atualizada automaticamente ao registrar entrada vinculada a OC |
| Histórico de OCs por fornecedor | Negociação de contratos e prazos de entrega Controle de garantias de pecas e serviços |  |', NULL, NULL, NULL, NULL, 1);

-- Relatório de Discrepâncias [Governanca/RelatorioUHNC]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Governanca', N'RelatorioUHNC', N'Relatório de Discrepâncias', N'5.11 — Governança & Camareira', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:camareira') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de Governança e supervisoras. | Menu lateral > Governança > Relatório > Relatório de Discrepâncias pcmbysim.com.br/Governanca/RelatorioDiscrepancias |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de consultar, por período, todo o histórico de apontamentos feitos na tela de Discrepâncias (seção 5.10), a visão de relatório complementar ao apontamento do dia a dia, útil para auditar um período e identificar padrões de problemas por U.H.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!INFO]
> **RELATÓRIO HISTÓRICO**
> Esta tela é o relatório histórico da tela de Discrepâncias (seção 5.10), os apontamentos feitos lá aparecem aqui, filtráveis por período.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Apontamentos REGISTRADOS | → | 2️⃣ Relatório FILTRADO | → | 3️⃣ Padrões IDENTIFICADOS | → | 📊 Ação Corretiva |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Governança > Relatório de Discrepâncias — indicadores e tabela histórica](/screenshots/governanca-relatorio-discrepancias.png)


### 4.1  Consultar o relatório

1. Acesse Governança > Relatório de Discrepâncias.
1. Selecione a Unidade e o Período (Data De/Até).
1. Clique em Filtrar.
1. Confira os 9 indicadores do topo: Total Planejado, Total Arrumado, Permanência, Saída, Divergências, Planej. s/ Execução, Exec. s/ Vistoria, Quartos N.Q.A e Não Perturbe.
1. Use Copiar / Imprimir / Excel / PDF para exportar.


> [!INFO]
> **RESULTADO ESPERADO**
> Tabela com uma linha por apontamento no período: Local/U.H., Data, Planejado Para, Executado Por, Vistoriado Por, Hora Término, Status UH, Stat. Gov, Divergência, AD/CR1/CR2, Bagagem e Observação, o mesmo nível de detalhe da tela de Discrepâncias, mas em formato de relatório histórico.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Divergências | Contagem de apontamentos com Stat. Gov = N/OK no período | Auto | 11 |
| Planej. s/ Execução | U.H.s planejadas que não tiveram apontamento registrado | Auto | 0 |
| Exec. s/ Vistoria | U.H.s executadas mas ainda sem vistoria registrada | Auto | 18 |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Revise o indicador Exec. s/ Vistoria semanalmente, um número alto e persistente indica que a supervisão não está acompanhando o ritmo da limpeza.
> Use a coluna Observação em conjunto com o Status UH para identificar problemas recorrentes por U.H. específica (ex.: sempre ''Mau Cheiro'' no mesmo quarto pode indicar problema estrutural, não operacional).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O relatório vem vazio para um período com movimento conhecido | Os apontamentos foram feitos pela tela de Apontamento clássico (seção 5.1), não pela de Discrepâncias (5.10) | Confirme com a equipe qual tela está sendo usada no dia a dia |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Governança > Discrepâncias (seção 5.10) | Reuniões de gestão de governança: evidência histórica | Atualizado automaticamente a cada apontamento |', NULL, NULL, NULL, NULL, 1);

-- Checklist de Governança [CadastroBasico/ChecklistIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'ChecklistIndex', N'Checklist de Governança', N'5.12 — Governança & Camareira', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:camareira') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Governantas, supervisoras e camareiras. | Menu lateral > Governança > Checklist pcmbysim.com.br/Governanca/ChecklistGovernanca |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de acompanhar, em um painel visual por Bloco e Andar, o andamento dos checklists de arrumação de cada U.H., o mesmo checklist de limpeza já mencionado na seção 5.1, agora com tela própria, atualizada em agosto/2026 para um layout em grade com legenda de cores.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> O Planejamento do turno precisa estar feito (seção 5.1, subseção 4.2) para que as U.H.s apareçam com checklist pendente.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Planejamento FEITO | → | 2️⃣ U.H. PENDENTE | → | 3️⃣ Checklist EXECUTADO | → | 4️⃣ Status ATUALIZADO | → | 📊 Aguardando/Concluído |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Governança > Checklist — grid de U.H.s por Bloco e Andar com legenda de status](/screenshots/governanca-checklist-grid.png)


### 4.1  Consultar o painel de checklists

1. Acesse Governança > Checklist.
1. Filtre por Unidade, Tipo de Checklist (Manutenção / Permanência / Saída), Status Front Office, Status Quarto ou Colaborador.
1. Confira os 4 cards de contagem no topo: Pendente, Concluído, Aguardando Liberação, Retrabalho.
1. Abaixo, as U.H.s aparecem em um grid organizado por Bloco e Andar, cada uma colorida conforme a legenda: Pendente, Aguardando Liberação, Concluído, Retrabalho, Sem Apontamento.


> [!INFO]
> **RESULTADO ESPERADO**
> Uma visão rápida, tipo mapa de calor, de quantas U.H.s de cada bloco/andar ainda estão pendentes, útil para redistribuir camareiras em tempo real durante o turno.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Tipo Checklist | Contexto da limpeza | Sim | MANUTENÇÃO / PERMANÊNCIA / SAÍDA |
| Status (grid) | Situação de cada U.H. no checklist | Auto | Pendente / Concluído / Aguardando Liberação / Retrabalho / Sem apontamento |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Use este painel nas rondas de meio de turno, visualizar por Bloco/Andar é mais rápido que consultar a lista completa de U.H.s da tela de Apontamento (seção 5.1).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Uma U.H. aparece como ''Sem apontamento'' | O Planejamento do turno não incluiu essa U.H. | Acesse Governança > Planejamento (seção 5.1) e confirme a atribuição |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Governança > Planejamento (seção 5.1) | Indicadores de andamento do turno em tempo real | Status atualizado a cada apontamento de checklist |', NULL, NULL, NULL, NULL, 1);

-- Auditoria Corporativa e de Qualidade [CadastroBasico/AuditoriaQualidadeIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'AuditoriaQualidadeIndex', N'Auditoria Corporativa e de Qualidade', N'5.4 — Qualidade & Auditoria', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:qualidade') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Auditores internos, gestores de qualidade e diretores. | Menu lateral > Auditoria > Auditoria Corporativa / Qualidade pcmbysim.com.br/Auditoria/AuditoriaCorporativoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de planejar, executar e analisar auditorias de conformidade na unidade. A auditoria é a ferramenta que fecha o ciclo de melhoria contínua: ela identifica onde os padrões não estão sendo seguidos, gera planos de ação corretivos e produz a nota que a diretoria usa para medir a saúde operacional de cada unidade da rede.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!INFO]
> **IMPORTANTE SABER, os 3 tipos de auditoria não são a mesma coisa**
> **Auditoria Corporativa**: a própria empresa que usa o sistema audita o padrão dentro do seu próprio ambiente, é a auto-auditoria interna.
> **Auditoria Externa**: quem executa é a SIM Services, auditando os clientes dela (ver também seção 5.6, para a variante voltada a Alimentos e Bebidas).
> **Auditoria (módulo Qualidade)**: só empresas que contratam o módulo de Qualidade fazem esse tipo de auditoria, dentro dos ambientes que cadastraram, tem um painel próprio de acompanhamento (Pendente / Atrasado / Em Andamento / Concluído). O módulo Qualidade também tem duas telas complementares testadas ao vivo: **Auditoria - Cronograma**, uma matriz por dia do mês mostrando quando cada auditoria está atrasada/a realizar/realizada; e **Tarefa**, uma agenda própria (Pendente/Atrasado/Em Andamento/Concluído, com visualização por mês) para tarefas do módulo, com seu próprio histórico.


| Tipo | Onde acessar | Foco | Periodicidade típica |
| :--- | :--- | :--- | :--- |
| Corporativa | Auditoria > Auditoria Corporativa | Conformidade com processos e padrões internos da rede | Mensal ou trimestral |
| Qualidade | Qualidade > Auditoria | Excelência operacional e experiência do cliente, só para quem tem o módulo | Semanal ou quinzenal |
| Externa | Auditoria > Auditoria Externa | Auditoria executada pela SIM Services no cliente | Semestral ou sob demanda |


> [!INFO]
> **IMPORTANTE SABER**
> ''Auditoria > Auditoria Externa'' (diferente da Auditoria Corporativa) não é um checklist com nota, é um registro de laudo/documento simples (Data, Data Validade, Descrição, Fornecedor, Valor, Arquivo), igual ao padrão usado no módulo A&B (seção 5.6). Não gera Plano de Ação nem Nota de conformidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Modelo CADASTRADO | → | 2️⃣ Auditoria INICIADA | → | 3️⃣ Checklist RESPONDIDO | → | 4️⃣ Nota CALCULADA | → | 5️⃣ Plano de Ação GERADO | → | 📊 Histórico Arquivado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Iniciar uma auditoria corporativa

![Auditoria > Corporativa — painel com nota atual e botão para nova auditoria](/screenshots/auditoria-corporativa.png)


1. Acesse Auditoria > Auditoria Corporativa e clique em Novo.
1. Selecione a Unidade e o modelo de auditoria a ser aplicado.
1. O checklist é organizado por categoria e subcategoria (ex.: ''01 - LAUDOS & SEGURANÇA'' > ''01 - AVCB''), e cada item real é respondido como **SIM / NÃO**, com 3 colunas por item: **Observação** (texto livre), **Prazo (dias)** e **Foto**.
1. Use a Observação para justificar respostas ''NÃO'' e a Foto para evidência, mesmo papel que o manual atribuía a ''Não Conforme''.
1. Para itens com prazo de correção, preencha Prazo (dias), é esse campo que alimenta o Plano de Ação, não uma opção separada de ''Gerar Plano de Ação''.
1. Ao concluir todos os itens, clique em Finalizar Auditoria.


> [!INFO]
> **RESULTADO ESPERADO**
> Nota percentual calculada automaticamente e exibida na listagem.
> Auditoria arquivada no histórico (seção 5.5).


> [!DANGER]
> **ATENÇÃO, a auditoria finalizada NÃO é imutável na interface**
> A tela de Apontamento ainda mostra os botões **Reabrir Auditoria** e **Excluir**, mesmo padrão já visto no histórico de Preventiva/Rotina (seção 3.11). ''Excluir'' aqui também não apaga o registro de fato, é um cancelamento lógico, o mesmo comportamento do resto do sistema. Não trate ''Finalizar Auditoria'' como uma trava técnica contra edição; é uma boa prática de equipe, não uma garantia do sistema.


### 4.2  Acompanhar a nota de conformidade

Na tela principal de Auditoria Corporativa, cada auditoria realizada aparece como um card com:

—  Nota em percentual (ex.: 91,28), exibida como número, não é um velocímetro/gauge circular.

—  Pontos Possíveis vs. Realizados: base de cálculo da nota.

—  Conformes, Não Conformidades, Não Respondidos e Não Aplicáveis.

> [!INFO]
> **IMPORTANTE SABER**
> Meta recomendada: manter a nota acima de 85% em todas as unidades.
> Sinal de alerta: queda de mais de 10 pontos em relação ao mês anterior exige reunião imediata com o gestor da unidade.
> Sinal crítico: dois ciclos consecutivos abaixo de 70% indicam falha sistêmica, envolva a diretoria.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Nota | Percentual de conformidade (0 a 100%) | --- | 91,28% |
| Pts. Possíveis | Total de pontos do checklist se todos forem Conf. | --- | 250 pontos |
| Pts. Realizados | Pontos obtidos com base nas respostas | --- | 245 pontos |
| Não Conformes | Itens reprovados que precisam de correção | --- | 5 itens |
| Não Respondidos | Itens em branco: comprometem a nota | --- | 0 itens recomendado |
| Não Aplicáveis | Itens fora do escopo da unidade | --- | 3 itens |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Nunca realize a auditoria sem fotos das não conformidades, a evidência fotográfica é o que diferencia uma auditoria auditável de uma mera opinião.
> Agende as auditorias com calendário fixo, surpreender a equipe é válido para algumas vistorias, mas auditorias corporativas geram mais valor quando a equipe sabe que acontecerão regularmente.
> Revise os Planos de Ação da auditoria anterior antes de iniciar a próxima, confirme que os itens foram corrigidos antes de verificar novamente.


> [!DANGER]
> Revise todas as respostas antes de clicar em Finalizar, reabrir uma auditoria depois deve ser exceção, não rotina (ver ressalva na subseção 4.1 sobre os botões Reabrir/Excluir).
> Notas artificialmente altas (auditores aprovando tudo sem verificar) destroem o valor do indicador. A nota de conformidade só é útil se refletir a realidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O modelo de auditoria não aparece para seleção | O cadastro de Auditoria Corporativa não foi criado ou está inativo | Acesse Cadastro Básico > Auditoria — Corporativo e verifique se o modelo está ativo e vinculado à unidade correta |
| Os Planos de Ação não são gerados automaticamente para os itens N.Conf. | O switch ''Gerar Plano de Ação'' não está ativado no cadastro do modelo | Edite o modelo em Cadastro Básico > Auditoria — Corporativo e ative o switch correspondente |
| A nota da auditoria está muito baixa sem motivo aparente | Há itens em branco (Não Respondidos) que contam como Não Conforme no cálculo | Revise o histórico da auditoria (seção 5.5) e verifique a coluna Não Respondidos: todo item precisa ser respondido |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Auditoria Corporativa (modelo) e Cadastro Básico > Checklist (itens) | Plano de Ação: correções para N.Conf. | Nota calculada automaticamente ao finalizar |
| Evidências fotográficas do auditor | Histórico de Auditoria (seção 5.5): evolução da nota ao longo do tempo | Planos de Ação gerados em tempo real para cada N.Conf. |', NULL, NULL, NULL, NULL, 1);

-- Normas, Procedimentos e Histórico de Auditorias [AEB/NormasProcedimentosIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'AEB', N'NormasProcedimentosIndex', N'Normas, Procedimentos e Histórico de Auditorias', N'5.5 — Qualidade & Auditoria', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:qualidade') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de qualidade, auditores e equipe técnica. | Auditoria > Normas e Procedimentos / Histórico pcmbysim.com.br/Auditoria/NormasProcedimentosIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de manter o repositório digital de normas e POPs da unidade, e consultar o histórico completo de auditorias corporativas realizadas com todos os indicadores de evolução. Essas duas funcionalidades são complementares: as normas definem o padrão que as auditorias medem, e o histórico prova que as auditorias foram realizadas e que o padrão evoluiu ao longo do tempo.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'![Auditoria > Normas e Procedimentos — listagem de documentos por tipo e status](/screenshots/auditoria-normas-procedimentos.png)


1. Acesse Auditoria > Normas e Procedimentos.
1. Para adicionar um documento, clique em Novo.
1. Selecione o Tipo (**Manual** ou **PIM**) e insira a Descrição e, se necessário, Comentários.
1. Faça o upload do arquivo (PDF recomendado) e clique em Salvar.


> [!INFO]
> **IMPORTANTE SABER**
> O sistema tem só 2 tipos de documento (Manual e PIM), a página existe para que cada empresa suba os próprios POPs e normas que já possui, não é uma lista fechada de categorias. Documentos recomendados para manter no repositório, como conteúdo (não como valor do campo Tipo):
> Manual de Boas Práticas de Manutenção.
> POPs de atividades críticas (elevadores, geradores, vasos de pressão).
> Normas Regulamentadoras aplicáveis: NR-10, NR-13, NR-35.
> Manual de Boas Práticas de Fabricação (para A&B).
> Procedimentos de emergência: fuga de gás, curto elétrico, incêndio.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Auditoria REALIZADA | → | 2️⃣ Histórico GERADO | → | 3️⃣ Normas PUBLICADAS | → | 4️⃣ Evolução ANALISADA | → | 📊 Evidência Disponível |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |



| O que analisar no histórico | Sinal de alerta | Ação recomendada |
| :--- | :--- | :--- |
| Queda de nota entre ciclos consecutivos | Redução superior a 10 pontos | Convocar reunião com gestor da unidade para identificar causa raiz |
| Mesmo item N.Conf. em 3+ auditorias | Problema sistêmico não resolvido | Rever o Plano de Ação: pode exigir investimento ou treinamento estrutural |
| Nota consistente acima de 95% | Indicador de excelência | Documentar as boas práticas e replicar para outras unidades da rede |
| Auditoria não realizada no período | Gap no ciclo de qualidade | Verificar com o gestor o motivo e replanejar a data |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Consultar o histórico de auditorias corporativas

1. Acesse Auditoria > Auditoria Corporativa, Histórico.
1. Filtre por Período, pelo modelo de Auditoria Corporativa e/ou por Status (Concluído / Em Andamento).
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Uma listagem com Data, Descrição, Executor, Pontos Possíveis/Realizados, Conforme, Não Conforme, Não Respondido, Não Aplicável, Nota e Status, o Status ''Em Andamento'' aparece quando uma auditoria foi iniciada mas ainda não foi finalizada.
> Esta tela só mostra resultado quando já existem auditorias realizadas no período filtrado, se estiver vazia, confirme se alguma auditoria corporativa foi de fato finalizada.


| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Documento não aparece no repositório | Documento inativo ou vinculado a outra unidade | Acesse Auditoria > Normas e Procedimentos, verifique a Unidade e ative o documento |
| Histórico não exibe um período específico | Filtro de período não cobre as datas das auditorias realizadas | Amplie o intervalo de datas no filtro ou remova o filtro de período para ver todos os registros |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Tipo | Manual ou PIM | Sim | Manual |
| Descrição | Nome do documento | Sim | POP de Abertura de OS para Hóspedes |
| Comentários | Observações adicionais sobre o documento | Não | Revisado em jan/2026 |
| Arquivo | PDF do documento armazenado no sistema | Sim | pop_abertura_os.pdf |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Mantenha todos os POPs com data de revisão no campo Comentários, um procedimento sem data pode estar desatualizado. Revise anualmente.
> Use o histórico de auditorias para preparação de auditorias externas. Se a Vigilância Sanitária perguntar sobre conformidade, o histórico é a prova mais sólida que você pode apresentar.


> [!DANGER]
> Nunca substitua um documento normativo (Manual, POP, NR) sem manter a versão anterior arquivada com sua data de vigência, em auditorias, é preciso provar qual versão estava em vigor no momento do evento analisado.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Documento não aparece no repositório | Documento inativo ou de outra unidade | Acesse Auditoria > Normas e Procedimentos, verifique a Unidade e ative o documento |
| Histórico não exibe um período específico | Filtro de período não cobre as datas das auditorias realizadas | Amplie o intervalo de datas no filtro ou remova-o para ver todos os registros |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Auditoria Corporativa e de Qualidade (gera histórico, seção 5.4) | Plano de Ação: fechamento do ciclo de qualidade | Histórico atualizado a cada auditoria finalizada |
| Documentos do repositório de normas | Treinamento de equipe: referência técnica | Status ''Em Andamento'' até a auditoria ser finalizada |', NULL, NULL, NULL, NULL, 1);

-- A&B — Alimentos e Bebidas [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'A&B — Alimentos e Bebidas', N'5.6 — Qualidade & Auditoria', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:qualidade') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de A&B, chefs de cozinha, nutricionistas e auditores sanitarios. | Menu lateral > A&B pcmbysim.com.br/AEB/AuditoriaExternaIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de gerenciar toda a conformidade sanitaria e de qualidade da área de alimentacao da unidade: realizar auditorias externas e internas, controlar contratos de fornecedores especializados, monitorar laudos obrigatórios e manter o repositorio de normas e POPs de cozinha. O módulo A&B e o que garante que a área de restauracao da unidade esteja sempre pronta para uma fiscalizacao da Vigilancia Sanitaria.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'| Tela | URL | Função |
| :--- | :--- | :--- |
| Auditoria Externa | AEB/AuditoriaExternaIndex | Registro de vistorias de órgãos reguladores (ANVISA, Vigilancia Sanitaria municipal) |
| Contratos | AEB/ContratoIndex | Controle de prestadores de serviço de A&B: higienizacao, dedetizacao, coleta de resíduos |
| Laudo / Documentação | AEB/LaudoIndex | Laudos obrigatórios: potabilidade da água, limpeza de caixa de gordura, higienizacao |
| Normas e Procedimentos | AEB/NormasProcedimentosIndex | Repositorio de POPs, Manual de Boas Praticas de Fabricacao e cronogramas de limpeza |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Contrato VIGENTE | → | 2️⃣ Serviço REALIZADO | → | 3️⃣ Laudo EMITIDO | → | 4️⃣ Laudo REGISTRADO | → | 📊 Auditoria Interna |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![A&B > Auditoria Externa — formulário de registro com nota, conformidades e plano de acao](/screenshots/aeb-auditoria-externa.png)


1. Acesse A&B > Auditoria Externa e clique em Novo.
1. Preencha: data da vistoria, órgão fiscalizador, auditor responsável e resultado geral.
1. Registre os itens avaliados com status e observacoes do fiscalizador.
1. Para irregularidades apontadas, crie Planos de Acao com responsável e prazo.
1. Clique em Salvar.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Laudo | Periodicidade mínima | Órgão competente | Risco se vencido |
| :--- | :--- | :--- | :--- |
| Análise de potabilidade da água | Semestral | Vigilancia Sanitaria | Embargo sanitario imediato |
| Limpeza de caixa de gordura | Semestral | Vigilancia Sanitaria | Autuacao e multa |
| Higienizacao de caixa d''água | Semestral | Vigilancia Sanitaria | Embargo sanitario imediato |
| Controle de vetores (dedetizacao) | Semestral | Vigilancia Sanitaria | Autuacao e embargo |
| Análise microbiologica de alimentos | Trimestral | ANVISA / Vigilancia | Interdição da cozinha |
| Higienizacao de coifas e dutos | Semestral | CBMERJ / Bombeiros | Risco de incendio e autuacao |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Mantenha todos os laudos de A&B digitalizados no sistema, em uma fiscalizacao surpresa, ter acesso imediato ao PDF do laudo e o que demonstra organização e conformidade.
> Cadastre os contratos de higienizacao com alerta de vencimento de 30 dias, renovacoes de contrato demoram. A falta de um contrato ativo e considerada irregularidade grave.
> Realize simulados de fiscalizacao mensalmente, percorra a cozinha como se fosse um fiscal e registre o resultado como uma auditoria interna.


> [!DANGER]
> A área de A&B e a mais fiscalizada em hoteis. Uma Vigilancia Sanitaria não avisada pode embargar a operação toda da unidade com base em irregularidades da cozinha, independente do resto estar em conformidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Um laudo venceu e não houve alerta | O laudo foi cadastrado sem data de validade ou a data estava incorreta | Edite o laudo, corrija a data de validade e salve. Configure a periodicidade no Cadastro Básico > Laudo para que o sistema gere alertas automaticamente |
| O contrato de higienizacao não aparece na listagem | Contrato vinculado a outra unidade ou Fornecedor inativo | Verifique a Unidade e o Fornecedor vinculado ao contrato em A&B > Contratos |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Fornecedor (prestadores de serviço)Cadastro Básico > Laudo (tipos e periodicidades) | PCM > OS — irregularidades geram chamados de manutenção Módulo Financeiro: custos de contratos e laudos de A&B Plano de Acao: correcoes de não conformidades | Alerta de vencimento de laudos e contratos 30 dias antes |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Checklist de Auditoria — Estrutura e Pesos [CadastroBasico/ChecklistIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'ChecklistIndex', N'Cadastro de Checklist de Auditoria — Estrutura e Pesos', N'5.7 — Qualidade & Auditoria', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:qualidade') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de Qualidade e usuários com perfil Auditor. | Cadastro Básico > Checklist, com Tipo de Checklist = Auditoria — mesma tela do cadastro geral de checklists (seção 2.13), mas com uma estrutura de item própria. |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de configurar corretamente o checklist que alimenta a Auditoria Corporativa (seção 5.4): não é só uma lista de perguntas SIM/NÃO, é um sistema de **peso em 3 camadas** (Grupo, Subgrupo e Item) que define o quanto cada resposta pesa na Nota final de conformidade. Configurar os pesos errado não trava o cadastro, mas distorce silenciosamente a Nota que a diretoria usa para medir a saúde da unidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A unidade precisa estar cadastrada: seção 2.1.
> Esta seção documenta a estrutura de cadastro com base numa planilha real de Auditoria (mais de 600 itens reais, cobrindo Laudos & Segurança, Sistemas & Máquinas, Edificação & Operação e U.H. em Dia).
> Não confunda esta estrutura com a de Rotina/Preventiva/Governança (seção 2.13), Auditoria tem campos próprios que as outras não têm.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Estrutura CRIADA | → | 2️⃣ Checklist PUBLICADO | → | 3️⃣ Auditor RESPONDE | → | 4️⃣ Nota CALCULADA | → | 📊 Histórico Arquivado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  A estrutura de 3 níveis com peso

O item de um checklist de Auditoria tem 14 campos, mais que o dobro dos 6 usados em Governança (seção 2.13):


| Campo do item | O que faz | Exemplo real |
| :--- | :--- | :--- |
| Sequência Checklist | Código Grupo.Subgrupo.Item (GG.SS.III): 3 níveis, diferente do GG.III de Rotina/Preventiva | 01.01.001 |
| Tipo de Checklist (do item) | Formato de resposta: ver passo 4.4 | SIM / NÃO / N.A. |
| Grupo | Macro-área da auditoria | 01 - LAUDOS & SEGURANÇA |
| Peso Grupo | Quanto esse Grupo vale no total da auditoria | 50 |
| Subgrupo | Divisão dentro do Grupo | 01 - AVCB |
| Peso Subgrupo | Quanto esse Subgrupo vale dentro do Grupo | 100 |
| Item do Checklist | Texto da pergunta | POSSUI AVCB VIGENTE? |
| Peso do Item | Quanto essa pergunta específica vale dentro do Subgrupo | 80.00 |
| Valor Mínimo / Valor Máximo | Faixa aceitável: só para Tipo NUMÉRICO | 0.00 / 0.00 |
| Unidade Medida | Unidade: só para Tipo NUMÉRICO | — |
| Departamento Responsável | Quem deve corrigir se o item for reprovado: ver passo 4.2 | MANUTENÇÃO |
| Permite foto | Habilita evidência fotográfica | SIM |
| Gera Ordem de Serviço | Abre OS automaticamente se reprovado | NÃO |


> [!INFO]
> **O QUE ISSO PERMITE NA PRÁTICA**
> Os 3 níveis de peso deixam a Nota final mais equilibrada: um Grupo crítico (ex.: ''01 - LAUDOS & SEGURANÇA'', peso 50) pesa mais que um Grupo secundário (ex.: ''02 - SISTEMAS & MÁQUINAS'', peso 30); dentro de cada Grupo, um Subgrupo pode valer mais que outro; e dentro de cada Subgrupo, uma pergunta crítica pode valer mais que uma pergunta simples (na planilha real, ''POSSUI AVCB VIGENTE?'' vale 80 pontos dentro do seu Subgrupo, enquanto ''PURGA PERIÓDICA?'' de outro Subgrupo vale só 10). Isso evita que uma pergunta trivial tenha o mesmo impacto na Nota que uma pergunta de segurança crítica.


> [!INFO]
> **RECOMENDAÇÃO**
> Antes de configurar pesos num checklist real de produção, valide o resultado da fórmula (Peso Grupo × Peso Subgrupo × Peso do Item, seção 5.4) com uma auditoria de teste.


### 4.2  Departamento Responsável, não confundir com Tipo de Checklist

O campo Departamento Responsável define para qual área o item aponta quando reprovado, é uma lista de 11 departamentos reais: A&B, Administrativo, Cozinha, Estacionamento, Gerência, Governança, Manutenção, Qualidade, Recepção, Restaurante, Técnico.


> [!DANGER]
> **ARMADILHA DE NOME PARECIDO**
> ''Governança'' aparece como opção de Departamento Responsável aqui, mas isso é diferente de ''Tipo de Checklist = Governança'' no cabeçalho (seção 2.13). Um item de um checklist de AUDITORIA pode ter Departamento Responsável = Governança (ou seja, é a Governança quem corrige aquele item reprovado) sem que o checklist em si seja do Tipo Governança. São dois campos de telas diferentes que só coincidem no nome.


### 4.3  Sequência em 3 níveis (Grupo.Subgrupo.Item)

Diferente do formato GG.III (Grupo.Item) usado em Rotina, Preventiva e Governança (seção 2.13), a Auditoria usa GG.SS.III (Grupo.Subgrupo.Item), por isso 01.01.001 é o primeiro item do primeiro Subgrupo do primeiro Grupo, e 02.05.001 é o primeiro item do 5º Subgrupo do 2º Grupo.

Na planilha real analisada, os Grupos confirmados foram: ''01 - LAUDOS & SEGURANÇA'', ''02 - SISTEMAS & MÁQUINAS'', ''03 - EDIFICAÇÃO & OPERAÇÃO'' e ''04 - UH EM DIA'' (uma seção inteira dedicada a inspecionar apartamentos individualmente dentro da mesma auditoria corporativa), mais de 600 itens ao todo.


### 4.4  Os formatos de resposta de cada item

| Tipo de Checklist (item) | Exemplo real da planilha |
| :--- | :--- |
| SIM / NÃO / N.A. | POSSUI AVCB VIGENTE? |
| TEXTO | NÚMERO / REGISTRO |
| NUMÉRICO | Leitura de medição, com Valor Mínimo/Máximo e Unidade |
| SIM / NÃO | Pergunta binária sem opção ''não se aplica'' |
| DATA | Data de um documento ou evento |
| HORA | Horário de uma verificação |


> [!INFO]
> **IMPORTANTE SABER**
> Assim como em Rotina/Preventiva (seção 2.13), a lista de seleção do sistema mostra mais de 6 opções, as demais são de uso interno do sistema (alimentam a funcionalidade de Discrepâncias) e não fazem parte do fluxo normal de criação de checklist de Auditoria.


### 4.5  Adicionar itens, via upload de planilha

1. Na tela do checklist criado com Tipo = Auditoria, clique em Download de Modelo para baixar a planilha template de 14 colunas.
1. Preencha a planilha com os itens, definindo Peso Grupo, Peso Subgrupo e Peso do Item para cada linha.
1. Salve a planilha e retorne ao sistema. Clique em Upload de Arquivo e selecione o arquivo preenchido.
1. O sistema importa todos os itens de uma vez.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Sequência Checklist | Código Grupo.Subgrupo.Item | Sim | 01.01.001 |
| Tipo de Checklist (item) | Formato de resposta | Sim | SIM/NÃO/N.A. |
| Grupo | Macro-área da auditoria | Sim | 01 - LAUDOS & SEGURANÇA |
| Peso Grupo | Peso do Grupo no total da auditoria | Sim | 50 |
| Subgrupo | Divisão dentro do Grupo | Sim | 01 - AVCB |
| Peso Subgrupo | Peso do Subgrupo dentro do Grupo | Sim | 100 |
| Item do Checklist | Texto da pergunta | Sim | POSSUI AVCB VIGENTE? |
| Peso do Item | Peso da pergunta dentro do Subgrupo | Sim | 80.00 |
| Valor Mínimo / Máximo | Faixa aceitável: só para Tipo NUMÉRICO | Não | 0.00 / 0.00 |
| Unidade Medida | Unidade: só para Tipo NUMÉRICO | Não | °C |
| Departamento Responsável | Área que corrige o item se reprovado: 11 opções reais | Sim | MANUTENÇÃO |
| Permite foto | Habilita anexo de foto como evidência | Sim | SIM |
| Gera Ordem de Serviço | Abre OS automaticamente se reprovado | Sim | NÃO |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Reserve os pesos mais altos para itens de segurança e conformidade legal (AVCB, laudos obrigatórios), são os que geram maior risco real se reprovados.
> Revise a soma dos pesos por Subgrupo periodicamente, pesos desbalanceados ao longo do tempo (itens adicionados sem ajustar os demais) distorcem a Nota gradualmente.
> Use o Departamento Responsável de forma consistente, é o que direciona a correção para a equipe certa depois de uma reprovação.


> [!DANGER]
> Um Peso do Item configurado alto demais numa pergunta trivial infla a Nota mesmo com itens críticos reprovados, e isso não gera nenhum erro visível no cadastro. Revise a distribuição de pesos com o mesmo cuidado que revisaria uma planilha financeira.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A Nota de conformidade não reflete a gravidade real das reprovações | Pesos de Grupo/Subgrupo/Item mal distribuídos | Revise os pesos priorizando itens de segurança e conformidade legal |
| Um item reprovado não aponta para a equipe certa | Departamento Responsável preenchido incorretamente ou em branco | Edite o item e selecione o Departamento correto |
| O upload da planilha falha ou importa pesos errados | Planilha não seguiu o modelo de 14 colunas específico de Auditoria | Baixe novamente o modelo com Tipo de Checklist = Auditoria selecionado antes de exportar |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Departamento (lista de Departamento Responsável) | Auditoria Corporativa (seção 5.4): checklist respondido pelo auditor, Nota calculada com base nos pesos | Nota recalculada automaticamente conforme os pesos configurados |
| Estrutura de Grupo/Subgrupo/Item com peso | Histórico de Auditoria (seção 5.5): evolução da Nota ao longo do tempo | Itens reprovados apontam automaticamente para o Departamento Responsável |', NULL, NULL, NULL, NULL, 1);

-- Green Planet — Configuração de Medições [GreenPlanet/LancamentoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'GreenPlanet', N'LancamentoIndex', N'Green Planet — Configuração de Medições', N'6.1 — Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de operação, sustentabilidade e supervisores de manutenção responsáveis por parametrizar o Green Planet antes do uso diário. | Menu lateral > Cadastro Básico > Grupo / Item de Medição pcmbysim.com.br/GreenPlanet/Configuracao |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de estruturar os grupos e medidores (itens de medição) de água, energia e gás da unidade, a base sem a qual nenhum lançamento diário de consumo pode ser feito. Uma configuração bem-feita, com unidades de medida e metas corretas, é o que garante que os KPIs calculados automaticamente na seção 6.2 realmente reflitam a realidade da operação.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A unidade precisa estar cadastrada e ativa: seção 2.1.
> Tenha em mãos a relação de medidores físicos existentes na unidade (hidrômetros, medidores de energia, medidores de gás) antes de começar o cadastro.
> Defina previamente os nomes padronizados dos grupos (ex.: ''Água de Consumo'', ''Energia Elétrica'', ''Gás Natural/GLP'') para manter consistência entre unidades da rede.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Grupo CRIADO | → | 2️⃣ Item CRIADO | → | 3️⃣ Unidade DEFINIDA | → | 4️⃣ Meta CONFIGURADA | → | 📊 Disponível p/ Lançamento |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Cadastro Básico > Grupo — Item de Medição — formulário de criação](/screenshots/cadastro-grupo-item-medicao.png)


### 4.1  Criar Grupo de Medição

1. Acesse Cadastro Básico > Grupo, Item de Medição e clique em Novo.
1. Preencha a Descrição do grupo (ex.: ''Água de Consumo'', ''Energia Elétrica'', ''Gás Natural'').
1. Clique em Salvar.


### 4.2  Criar Item de Medição (medidor específico)

1. Acesse Cadastro Básico > Item, Medição e clique em Novo.
1. Selecione o Grupo correspondente.
1. Preencha: Descrição do medidor, Unidade de Medida (m³, kWh, L), Meta de Consumo e Forma de Leitura (Leitura direta ou Somatória), não existe campo de Fator de Conversão, apesar do que versões antigas deste manual indicavam.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O medidor aparece disponível na grade de lançamento diário (seção 6.2).


Grupos de medição recomendados para uma unidade hoteleira:

| Grupo sugerido | Itens de medicao tipicos | Unidade de medida |
| :--- | :--- | :--- |
| Água de Consumo | Hidrometro Geral, Hidrometro Cozinha, Hidrometro SPA, Piscina | m3 |
| Energia Elétrica | Medidor Geral, Medidor Cozinha, Medidor Climatizacao, Medidor Iluminacao | kWh |
| Gas Natural/GLP | Medidor de Gas Geral, Medidor Cozinha, Medidor Caldeira | m3 ou kg |
| Diesel | Tanque Gerador, Tanque Caldeira | Litros |
| Gases de Efeito Estufa | Emissao estimada por consumo de energia e combustivel | kgCO2e |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Grupo | Categoria macro à qual o medidor pertence | Sim | Água de Consumo |
| Descrição (medidor) | Nome do medidor físico | Sim | Hidrômetro Cozinha |
| Unidade de Medida | Como o consumo é registrado | Sim | m³ / kWh / L |
| Forma de Leitura | Leitura (mostrador direto) ou Somatória (valor acumulado): campo real, substitui Fator de Conversão | Não | Leitura |
| Meta de Consumo | Referência usada para detectar desvios na seção 6.2 | Não | Baseada em histórico |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Padronize os nomes de grupo entre todas as unidades da rede, facilita a comparação (benchmarking) no relatório de Green Planet.
> Cadastre todos os medidores físicos existentes de uma vez, mesmo que alguns comecem sem meta definida, é mais fácil ajustar metas depois do que lembrar de cadastrar um medidor esquecido.


> [!DANGER]
> A meta de consumo zerada no cadastro do item de medição faz com que qualquer leitura seja considerada ''dentro do normal'', configure metas reais baseadas no histórico da unidade antes de liberar os lançamentos (seção 6.2).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Um medidor não aparece na grade de lançamento (seção 6.2) | O Item de Medição não foi criado ou está inativo | Acesse Cadastro Básico > Item — Medição e verifique o cadastro e o switch Ativo |
| Não consigo vincular um medidor a um grupo | O Grupo ainda não foi criado | Crie primeiro o Grupo em Cadastro Básico > Grupo — Item de Medição, depois o medidor |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Unidades (seção 2.1) | Green Planet > Lançamento e Análise de Consumo (seção 6.2) — grade de medidores disponível para leitura diária | Medidor disponível imediatamente após o cadastro |', NULL, NULL, NULL, NULL, 1);

-- Lançamento e Análise de Consumo [Financas/DespesaIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Financas', N'DespesaIndex', N'Lançamento e Análise de Consumo', N'6.2 — Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de operação, sustentabilidade e supervisores de manutenção. | Menu lateral > Green Planet > Medição pcmbysim.com.br/GreenPlanet/LancamentoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar o consumo diário de água, energia e gás da unidade e interpretar os KPIs automáticos gerados pelo sistema, consumo per capita, consumo por UH ocupada e desvios em relação à média histórica, para identificar desperdícios e vazamentos antes que virem fatura alta.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Os grupos e itens de medição precisam estar cadastrados com meta de consumo definida: seção 6.1.
> Tenha os dados de ocupação do dia (quantidade de hóspedes e quartos ocupados) disponíveis, eles entram no cálculo dos KPIs.
> Defina um horário fixo do dia para o lançamento (recomendado: início do turno da manhã), consistência de horário é o que torna o consumo comparável entre os dias.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Ocupação INFORMADA | → | 2️⃣ Leitura REGISTRADA | → | 3️⃣ KPI CALCULADO | → | 4️⃣ Desvio IDENTIFICADO | → | 📊 OS de Inspeção Aberta |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Green Planet > Medição — grade de medidores com campos de leitura e dados de ocupação](/screenshots/green-planet-medicao.png)


1. Acesse Green Planet > Medição.
1. Selecione a Unidade e confirme a Data da leitura.
1. Preencha os dados de ocupação: Qtde. Hóspedes e Quartos Ocupados.
1. Para cada medidor listado, insira a Leitura Atual conforme o mostrador físico.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A leitura é salva e o registro fica marcado com asterisco (**) para evitar duplicidade, isso foi confirmado em teste real.
> Os KPIs (consumo per capita, por UH ocupada e desvio) NÃO aparecem nesta tela depois de salvar, testado com um lançamento real e nenhum indicador apareceu na tela de Medição. Eles provavelmente ficam disponíveis em Relatório > Green Planet, uma tela separada, consulte os KPIs por lá, não espere vê-los aqui.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Data da Leitura | Dia ao qual o lançamento se refere: no formulário real, é um filtro no painel esquerdo, não um campo dentro do card Medição | Sim | 04/07/2026 |
| Qtde. Hóspedes | Ocupação do dia: usada no cálculo per capita | Sim | 320 |
| Quartos Ocupados | Ocupação do dia: usada no cálculo por UH | Sim | 145 |
| Leitura Atual | Valor lido no mostrador físico do medidor | Sim | 1248 (m³) |


Indicadores calculados automaticamente a partir do lançamento:

| KPI | Fórmula | Como usar |
| :--- | :--- | :--- |
| Consumo per Capita | Consumo do dia / Qtde. Hóspedes | Benchmarking entre unidades: quanto cada hóspede consome em média |
| Consumo por UH Ocupada | Consumo do dia / Quartos Ocupados | Eficiência por quarto: detecta desperdicio em áreas comuns |
| Variacao em relacao a média | (Consumo atual - Média historica) / Média historica | Desvio acima de 20% pode indicar vazamento ou equipamento com defeito |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Registre as medições sempre no mesmo horário, preferencialmente no início do turno da manhã.
> Quando o consumo de água subir mais de 15% em relação à média sem aumento de ocupação, abra imediatamente uma OS de inspeção de vazamentos, uma tubulação rompida pode gerar prejuízos de dezenas de milhares de reais por semana.
> Use os dados de Green Planet para negociar com as concessionárias, histórico de consumo detalhado pode embasar pedidos de redução de tarifa.


> [!DANGER]
> A meta de consumo zerada no cadastro do item de medição (seção 6.1) faz com que qualquer leitura seja considerada ''dentro do normal'', confirme que as metas foram configuradas antes de confiar nos alertas de desvio.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O cálculo per capita está distorcido | Os dados de ocupação (hóspedes e quartos) foram preenchidos incorretamente ou deixados zerados | Edite o lançamento do dia e corrija os campos de ocupação |
| O sistema sinaliza desvio mas o consumo parece normal | A meta ou média histórica está configurada incorretamente no item de medição | Revise o campo Meta de Consumo no cadastro do medidor (seção 6.1) |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Green Planet > Configuração de Medições (seção 6.1) — grupos, medidores e metas | PCM > OS — alerta de desvio pode gerar chamado de inspeção de vazamento | Alerta automático de desvio brusco no consumo diário |', NULL, NULL, NULL, NULL, 1);

-- Relatorios de Desempenho e Business Intelligence [Governanca/RelatorioUHNC]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Governanca', N'RelatorioUHNC', N'Relatorios de Desempenho e Business Intelligence', N'7.2 — Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, diretores e analistas de operações. | Menu lateral > Relatorio / BI pcmbysim.com.br/Relatorio |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de extrair, filtrar e interpretar os principais relatorios analiticos do sistema, dados que vao alem do tempo real do Dashboard e mostram tendencias, padrão de falhas, custo acumulado e desempenho de equipe ao longo de períodos. Os relatorios de BI são as ferramentas de prestação de contas para a diretoria e de planejamento para o gestor.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'| Relatorio | Caminho no sistema | O que responde |
| :--- | :--- | :--- |
| Manutenção x Categoria | Relatório > Manutenção x Categoria | Volume de OS por categoria de serviço: real, não documentado antes |
| Manutenção x Equipamento | Relatório > Manutenção x Equipamento | Histórico de falhas por TAG: base para decisao de Capex vs Opex |
| Manutenção x Executor | Relatório > Manutenção x Executor | OS executadas, por técnico: real, não documentado antes |
| Manutenção x Setor | Relatório > Manutenção x Setor | Volume de OS por setor: real, não documentado antes |
| Manutenção x Solicitante | Relatório > Manutenção x Solicitante | Volume de OS por quem solicitou: real, não documentado antes |
| Manutenção x Tipo | Relatório > Manutenção x Tipo | Volume por Tipo de Ordem de Serviço: real, não documentado antes |
| Custo: Horas Trabalhadas | Relatório > Custo - Horas Trabalhadas | Gasto real de mao de obra por período |
| Tempo Médio - Atendimento | Relatório > Tempo Médio - Atendimento | SLA por prioridade: quanto tempo a equipe leva para resolver cada nível |
| Horas Trabalhadas / Funcionário % Ociosidade | Relatório > Horas Trabalhadas | Horas apontadas e ociosidade por técnico no período |
| O.S. - Aberto x Concluído | Relatório > O.S. - Aberto x Concluído | Cumprimento do fluxo de OS: real, não documentado antes |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Relatório SELECIONADO | → | 2️⃣ Filtros APLICADOS | → | 3️⃣ Visualização GERADA | → | 4️⃣ Dados EXPORTADOS | → | 📊 Análise de BI |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Tela de relatorio — filtros laterais com Unidade, Período, Categoria e botao Filtrar](/screenshots/relatorio-generico.png)


1. Acesse o relatorio desejado pelo menu lateral.
1. Aplique os filtros:


Unidade, selecione uma ou todas as unidades da rede.

Período, defina Data Inicio e Data Fim.

Categoria / Executor / Equipamento, filtros adicionais conforme o relatorio.

1. Clique em Filtrar.
1. Para exportar, clique em Exportar Excel ou Exportar PDF.


> [!INFO]
> **RESULTADO ESPERADO**
> Relatorio gerado com os dados do período. O Excel permite análises adicionais fora do sistema (tabelas dinamicas, gráficos, comparativos).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'Este e o relatorio mais estratégico do sistema. Ele responde a pergunta que todo gestor enfrenta: devo continuar mantendo este equipamento ou e hora de substituir?

| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Filtro de uma ou todas as unidades da rede | Sim | Todas as unidades |
| Período | Intervalo de datas do relatório (Data Início e Data Fim) | Sim | 01/06/2025 a 30/06/2025 |
| Categoria / Executor / Equipamento | Filtros adicionais conforme o relatório selecionado | Não | Categoria: Elétrica |


| Situacao | Custo acumulado x Valor do ativo | Decisao recomendada |
| :--- | :--- | :--- |
| Custo < 20% do valor novo | Equipamento saudavel | Manter: investimento em manutenção ainda compensa |
| Custo entre 20% e 50% | Atenção: monitorar de perto | Avaliar condição tecnica e prever substituicao no próximo orcamento |
| Custo > 50% do valor novo | Equipamento caro de manter | Justificar Capex de substituicao com o histórico de custos |
| Custo > 100% do valor novo | Ativo já custou mais do que um novo | Substituicao urgente: apresentar o relatorio como evidência para diretoria |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Use o relatorio de OS por Equipamento mensalmente para identificar os ''10 equipamentos mais problematicos''. Eles são os candidatos para manutenção intensiva ou substituicao.
> Exporte os relatorios em Excel e crie uma pasta compartilhada com o histórico de cada mes. Com 12 meses de dados, os gráficos de tendencia comecam a ter valor preditivo real.
> Apresente o relatorio de Custo de Manutenção em reuniões de budget, e a justificativa tecnica mais solida para negociar o orcamento do próximo ano.


> [!DANGER]
> Relatórios exportados em Excel refletem o momento exato da exportação, se novos apontamentos forem feitos depois, é preciso exportar novamente para ter os dados atualizados.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Relatório em branco mesmo com dados no sistema | Filtro de período não cobre as datas ou unidade errada | Remova todos os filtros e aplique somente Unidade e um período amplo |
| Custo de manutenção zerado em todas as OS | Colaboradores sem Valor Hora ou saídas de estoque não vinculadas | Verifique Valor Hora em Cadastro Básico > Colaborador e confirme as saídas de estoque |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Todos os apontamentos operacionais (OS, Preventiva, Estoque, Colaboradores) | Reuniões de diretoria: dados de desempenho e custo Decisoes de Capex: histórico de custo por ativo Planejamento de equipe: producao por técnico | Dados consolidados em tempo real: relatorio reflete qualquer apontamento feito ha segundos |
| Custo de mao de obra (Valor Hora dos colaboradores) | Relatorio Mensal PCM (seção 7.3): base de dados Módulo Excel (seção 7.5): exportação avancada |  |', NULL, NULL, NULL, NULL, 1);

-- Relatorios de Governanca [Governanca/RelatorioUHNC]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Governanca', N'RelatorioUHNC', N'Relatorios de Governanca', N'7.4 — Governança & Camareira', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:camareira') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de Governança, supervisoras e diretores de operações. | Menu lateral > Governança > (cada relatório é um item de menu próprio) pcmbysim.com.br/Governanca/RelatorioCamareiraUH |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de extrair todos os relatórios analíticos do módulo de Governança, produtividade de camareiras e supervisoras, consumo de enxoval, histórico de limpeza e mudanças de status de U.H.. Esses relatórios são os instrumentos de gestão que transformam a operação de governança de intuitiva em baseada em dados.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'| Relatório | URL | O que mostra |
| :--- | :--- | :--- |
| Camareira x UH | Governanca/RelatorioCamareiraUH | Matriz de U.H.s apontadas por camareira e dia do mês (seção 5.3) |
| Camareira x NC | Governanca/RelatorioCamareiraNC | Matriz de não conformidades por camareira, item e dia do mês (seção 5.3) |
| Vistoriador x UH | Governanca/RelatorioVistoriadorUH | Matriz de vistorias realizadas por supervisor(a) e dia do mês |
| U.H. x NC | Governanca/RelatorioUHNC | Não conformidades por U.H. (variante do Camareira x NC quebrada por quarto) |
| Horas Trabalhadas Gov. | Governanca/FuncionarioHorasTrabalhadasGovernanca | Matriz de horas por colaboradora e mês do ano (seção 5.3) |
| Uso de Enxoval | Governanca/RelatorioConsumoEnxovalDia | Consumo diário por item de enxoval, com 3 formas de cálculo (Uso / Uso por Hóspede / Uso por U.H. Ocupada) |
| Inventário de Enxoval | Governanca/InventarioEnxoval | Acuracidade do estoque de enxoval: saldo físico vs. esperado pelo sistema (seção 5.2) |
| Log - Alteração de Status | Governanca/LogAlteracaoStatusGov | Log de cada mudança de status de U.H.: quem alterou, quando e para qual status |
| Histórico (Governança) | Governanca/Historico | Registro completo de cada limpeza, com horário de início/fim, tempo gasto, checklist, NC, OS e vistoria |


> [!INFO]
> **IMPORTANTE SABER**
> Não existe uma tela única que gere um mapa de status dos quartos por período, o status de U.H. em tempo real é acompanhado na tela de Apontamento (seção 5.1). O Log - Alteração de Status é diferente: é um histórico de cada troca de status (Unidade, Data, Bloco, Andar, U.H., Status, Origem, Usuário), útil para auditar quem mudou o quê.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Apontamentos REALIZADOS | → | 2️⃣ Dados CONSOLIDADOS | → | 3️⃣ Relatório FILTRADO | → | 4️⃣ Análise GERENCIAL | → | 📊 Ação Corretiva |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |



| Frequência | Relatório | Pergunta que responde | Ação típica |
| :--- | :--- | :--- | :--- |
| Diário | Apontamento (seção 5.1) | Quantos quartos ainda estão sujos ou em manutenção? | Redistribuir camareiras para priorizar quartos com check-in confirmado |
| Semanal | Camareira x NC | Alguma camareira tem NCs recorrentes no mesmo item? | Treinamento individual no item específico |
| Semanal | Camareira x UH | A produção diária por camareira está dentro do esperado? | Investigar causas de queda de produtividade |
| Mensal | Horas Trabalhadas | A carga horária está bem distribuída na equipe? | Ajustar escala do próximo mês |
| Mensal | Uso de Enxoval | Há tendência de perda no ciclo de lavanderia? | Intensificar conferência no retorno da lavanderia |
| Bimestral | Inventário de Enxoval | O estoque físico bate com o sistema? | Ajustar saldo via inventário e investigar divergências |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Governança > Relatório Camareira x UH — exemplo de relatório de produtividade em matriz](/screenshots/governanca-relatorio-camareira-uh.png)


1. No menu lateral, acesse Governança e clique no relatório desejado (cada um é um item de menu próprio, sem uma tela intermediária de escolha).
1. Selecione a Unidade e o período, a maioria dos relatórios de produtividade usa um seletor de Mês único; Histórico e Inventário usam um período De/Até.
1. Opcionalmente filtre por Camareira ou Vistoriador específico para análise individual.
1. Clique em Filtrar.
1. Para exportar, use os botões Copiar / Excel / PDF / Imprimir disponíveis na tela.
1. Use os dados nas reuniões semanais de Governança, combine os relatórios para uma visão completa.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Camareira x UH | Matriz de U.H.s por camareira e dia | --- | Uso semanal |
| Camareira x NC | Matriz de não conformidades por camareira, item e dia | --- | Uso semanal |
| Log - Alteração de Status | Histórico de mudanças de status por U.H. | --- | Uso diário / auditoria |
| Histórico (Governança) | Registro completo por quarto e período, com horário | --- | Uso mensal / Jurídico |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Combine o relatório de Camareira x NC com o de Camareira x UH: uma camareira com muitas UHs e poucas NCs é a referência da equipe. Reconheça publicamente.
> Use o Log - Alteração de Status para auditar mudanças suspeitas, por exemplo, um quarto que voltou de ''Manutenção'' para ''Sujo'' sem uma OS associada encerrada.
> Exporte o Histórico (Governança) para o setor jurídico se houver reclamação de hóspede, ele é a prova de que o quarto foi limpo e vistoriado antes do check-in.


> [!DANGER]
> Nunca use os relatórios de Camareira x UH/NC isoladamente para decisões disciplinares, sempre cruze com o Histórico e converse com a supervisora antes de qualquer ação com a colaboradora.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Preciso do horário de uma limpeza específica e não acho em nenhum relatório de produtividade | Os relatórios de produtividade são matrizes agregadas por dia | Consulte a tela Governança > Histórico — é lá que fica o detalhe por limpeza, com horário |
| Relatório em branco para o período | Nenhum apontamento de Governança foi salvo no sistema | Verifique se as camareiras estão finalizando (não apenas iniciando) os apontamentos |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Governança > Apontamento (execução registrada no app ou na web) | Gestão de RH: avaliação de desempenho e treinamento | Dados atualizados a cada apontamento concluído |
| Governança > Enxoval (movimentações de lavanderia) | Módulo Financeiro: custo de mão de obra de Governança | Log de Alteração de Status atualizado a cada troca de status |', NULL, NULL, NULL, NULL, 1);

-- Interface Opera — Integração com o PMS [Configuracao/InterfaceOpera]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Configuracao', N'InterfaceOpera', N'Interface Opera — Integração com o PMS', N'7.7 — Administrador', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:administrador') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Exclusivo para a equipe interna da SIM Services: não fica disponível para os clientes. | Menu lateral > Configuração > Interface Opera pcmbysim.com.br/Configuracao/InterfaceOpera |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de configurar a integração entre o PCM by SIM e o sistema Opera (PMS, Property Management System) da unidade. Essa integração é o que elimina a duplicidade de trabalho entre recepção e manutenção: o status de ocupação dos quartos no Opera alimenta automaticamente o módulo de Governança do PCM, e o status de limpeza do PCM reflete na disponibilidade de check-in do Opera.

> [!WARNING]
> **ANTES DE COMEÇAR**
> Esta tela é configurada pela equipe da SIM Services, não pelo cliente, é aqui que a integração com o PMS de cada hotel é feita. Clientes que precisam de uma integração com o PMS devem solicitar à SIM Services, não configurar diretamente.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'| Dado | Direção | De | Para | Benefício |
| :--- | :--- | :--- | :--- | :--- |
| Status de ocupação da UH | Opera -> PCM | Opera (PMS) | PCM Governança | PCM sabe quais quartos são saída (checkout) ou permanência sem digitar manualmente |
| Status de limpeza da UH | PCM -> Opera | PCM Governança | Opera (PMS) | Recepção vê em tempo real quais quartos estão prontos para check-in |
| Bloqueio por manutenção | PCM -> Opera | PCM Manutenção | Opera (PMS) | Opera impede a venda de quartos bloqueados por OS de PCM |

> [!INFO]
> **IMPORTANTE SABER**
> A tabela acima descreve o comportamento esperado da integração.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Credenciais OBTIDAS | → | 2️⃣ Configuração no PCM | → | 3️⃣ Frequência DEFINIDA | → | 4️⃣ Config. SALVA | → | 📊 Sincronia Monitorada |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Configuração > Interface Opera — campos de autenticação e frequência](/screenshots/config-interface-opera-real.png)


1. Acesse Configuração > Interface Opera.
1. Selecione a Unidade.
1. Preencha o Hostname do servidor Opera.
1. Informe Username e Password de acesso.
1. Preencha as credenciais de API: App Key, Client ID e Client Secret, fornecidas pela equipe de TI do Opera.
1. Defina a Frequência (minutos), o intervalo de sincronização entre os dois sistemas.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> Integração configurada, a sincronização passa a rodar automaticamente no intervalo definido em Frequência.


> [!DANGER]
> A configuração da Interface Opera deve ser feita em conjunto com a equipe de TI do Opera, as credenciais (Hostname, Username, Password, App Key, Client ID, Client Secret) são geradas pelo lado do Opera e não pelo PCM by SIM. Nunca tente configurar sem o suporte técnico dos dois lados.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade do PCM associada à integração | Sim | Hotel by SIM Services |
| Hostname | Endereço do servidor Opera | Sim | Fornecido pela equipe de TI do Opera |
| Username / Password | Credenciais de acesso ao servidor Opera | Sim | Fornecidas pela equipe de TI do Opera |
| App Key / Client ID / Client Secret | Credenciais de autenticação da API (padrão OAuth) | Sim | Fornecidas pela equipe de TI do Opera |
| Frequência (minutos) | Intervalo entre sincronizações | Sim | 15 |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Configure a Interface Opera junto com a equipe de TI do Opera, todas as credenciais são geradas pelo lado do Opera.
> Ajuste a Frequência de acordo com a criticidade da unidade, intervalos menores mantêm os sistemas mais sincronizados, mas geram mais tráfego entre eles.
> Após manutenção programada no Opera, confirme com o cliente se a integração retomou normalmente.


> [!DANGER]
> Nunca compartilhe Username, Password, App Key, Client ID ou Client Secret por canais não seguros (e-mail, chat), solicite que a equipe de TI do Opera os informe diretamente na configuração ou por um canal criptografado.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O status de limpeza do PCM não reflete na recepção Opera | Alguma credencial expirou ou a Frequência está alta demais | Acesse Configuração > Interface Opera e confirme as credenciais com o time de TI do Opera |
| Quartos bloqueados no PCM continuam disponíveis para venda no Opera | O mapeamento de status entre os dois sistemas não está correto | Verifique com a equipe de TI do Opera o mapeamento de status entre PCM e Opera |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Opera PMS (via API) | Governança > Apontamento — Status Front Office (seção 5.1) | PCM > OS — identificação do hóspede ao abrir chamado |
| Cadastro Básico > Unidade | Recepção Opera: disponibilidade de quartos do PCM | Sincronização automática no intervalo definido em Frequência |', NULL, NULL, NULL, NULL, 1);

-- Relatório Dinâmico de Itens Auditáveis [Auditoria/Relatorio]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Auditoria', N'Relatorio', N'Relatório Dinâmico de Itens Auditáveis', N'7.8 — Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de qualidade, auditores e administradores do sistema. | Menu lateral > Auditoria > Relatório itens Auditáveis pcmbysim.com.br/Auditoria/Relatorio |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de criar relatórios customizados de itens auditáveis com os filtros específicos da sua necessidade. É a ferramenta certa para levantar evidências organizadas de conformidade, por exemplo, para uma auditoria externa ou para acompanhar a recorrência de um item específico do Mapa de Manutenção ao longo do tempo.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **PRÉ-REQUISITO, POR QUE UM ITEM APARECE (OU NÃO) NESTE RELATÓRIO**
> Para um item aparecer aqui, duas condições precisam ser verdadeiras: (1) o item precisa fazer parte de um checklist de Rotina ou Preventiva (seção 2.13), e (2) dentro desse checklist, o item vistoriado precisa estar marcado como ''item auditável''. Um item que existe no Mapa de Manutenção (seção 2.17) mas não está marcado como auditável em nenhum checklist simplesmente não aparece no filtro, não é bug, é o funcionamento esperado.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Item AUDITÁVEL | → | 2️⃣ Apontamento REALIZADO | → | 3️⃣ Relatório FILTRADO | → | 4️⃣ Dados EXPORTADOS | → | 📊 Evidências Apresentadas |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Auditoria > Relatório itens Auditáveis — filtro por Unidade, Período e item do Mapa de Manutenção](/screenshots/relatorio-dinamico.png)


> [!INFO]
> **DUAS TELAS DIFERENTES COM NOME PARECIDO**
> Existem duas telas distintas envolvendo ''itens auditáveis''. **Cadastro Básico > Auditoria, Qualidade** (`CadastroBasico/RelatorioItensAuditaveisIndex`, apesar do nome na URL) é só um cadastro simples, 3 campos: Unidade, Descrição, Ativo, que define quais itens existem para auditoria. **O relatório de fato é esta tela: Auditoria > Relatório itens Auditáveis** (`Auditoria/Relatorio`), descrita no passo a passo abaixo.


1. Acesse Auditoria > Relatório itens Auditáveis.
1. Selecione a Unidade, o Período (De/Até) e busque o item do Mapa de Manutenção que quer analisar (campo de busca com autocomplete, ex.: ''RECEPÇÃO'').
1. Clique em Filtrar.
1. Exporte em Excel ou PDF para uso externo.


> [!INFO]
> **IMPORTANTE SABER**
> Diferente do que uma versão antiga deste manual descrevia, o filtro real não é por Módulo (PCM/Governança/A&B) nem por Status (Conforme/N.Conforme/N.Aplicável), é por item específico do Mapa de Manutenção (seção 2.17), dentro de um período. O painel de resultado ''Itens Auditáveis'' mostra as ocorrências desse item no período selecionado.


> [!INFO]
> **IMPORTANTE SABER**
> O Relatório Dinâmico é especialmente útil para auditorias externas: permite mostrar apenas os itens conformes de um determinado módulo, ou apenas os não conformes com os Planos de Ação vinculados, uma evidência organizada e profissional.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade cujos apontamentos serão analisados | Sim | Hotel by SIM Services |
| Período (De/Até) | Intervalo de datas dos apontamentos a incluir | Sim | 01/08/2026 a 31/08/2026 |
| Item do Mapa de Manutenção | Item específico a analisar, com busca por autocomplete | Sim | RECEPÇÃO |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Salve os filtros mais usados como atalho, ex: ''Não Conformes do último mês em PCM'' para uso semanal rápido.
> Antes de reclamar que um item não aparece no relatório, confirme que ele está marcado como ''item auditável'' dentro do checklist de Rotina/Preventiva correspondente, essa é a causa mais comum de resultado vazio.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Relatório Dinâmico sem itens de um módulo | Módulo sem checklists ou auditorias no período filtrado | Amplie o período e confirme que apontamentos foram realizados para o módulo |
| Um item do Mapa de Manutenção nunca aparece no relatório | O item não está marcado como ''item auditável'' em nenhum checklist de Rotina/Preventiva | Edite o checklist (seção 2.13) e marque o item como auditável |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Checklists de Rotina e Preventiva (seção 2.13): itens marcados como auditáveis | Evidências para auditorias externas e internas | Apontamento de Rotina/Preventiva alimenta o relatório automaticamente |
| Mapa de Manutenção (seção 2.17): catálogo de itens | Planos de Ação: correções vinculadas aos não conformes | — |', NULL, NULL, NULL, NULL, 1);

-- Upload em Massa (Cadastro Básico e PMOC) [Upload/UploadExcel]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Upload', N'UploadExcel', N'Upload em Massa (Cadastro Básico e PMOC)', N'7.9 — Administrador', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:administrador') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Administradores e equipe responsável pela implantação/carga inicial da unidade. | Menu lateral > Upload pcmbysim.com.br/Upload/UploadExcel |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de importar dados em massa por planilha, evitando digitação manual item a item em dois cenários: a carga inicial de cadastros básicos de uma unidade nova, e o apontamento em massa de execuções de PMOC. É a ferramenta certa quando o volume de dados é grande demais para cadastrar um por um pela interface.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'| Tela | URL | Função principal |
| :--- | :--- | :--- |
| Upload: Cadastro Básico | Upload/UploadExcel | Importa em massa 13 tipos de cadastro básico a partir de planilha |
| Upload: PMOC | Upload/UploadPMOC | Importa em massa apontamentos de execução do PMOC a partir de planilha |


> [!WARNING]
> **ANTES DE COMEÇAR**
> Use sempre o modelo de Planilha disponibilizado na própria tela (link Planilha), o layout de colunas é rígido, e uma planilha fora do padrão gera erro de importação.
> Para o upload de Cadastro Básico, confirme que os dados ''pai'' já existem quando aplicável (ex.: para importar U.H.s em massa, os Setores já precisam estar cadastrados).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Modelo BAIXADO | → | 2️⃣ Dados PREENCHIDOS | → | 3️⃣ Tabela/Unidade SELECIONADA | → | 4️⃣ Arquivo ENVIADO | → | 📊 Importação Processada |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Upload > PMOC — seleção de Unidade, Tabela e upload de planilha](/screenshots/upload-pmoc.png)


### 4.1  Importar cadastros básicos em massa

1. Acesse Upload > Cadastro Básico.
1. Selecione a Unidade.
1. Selecione a Tabela a importar: Apartamento (U.H.), Ar Condicionado, Categoria, Departamento, Equipamento, Fornecedor, Função, Funcionário, Itens Gerais, Local, PMOC Bup, Setor ou Usuário.
1. Clique em Planilha para baixar o modelo correto daquela Tabela, se ainda não tiver.
1. Preencha a planilha seguindo exatamente as colunas do modelo.
1. Selecione o Arquivo preenchido e envie.


> [!INFO]
> **IMPORTANTE SABER**
> Cada Tabela tem seu próprio modelo de planilha, com colunas diferentes, sempre baixe o modelo específico da Tabela que vai importar, não reutilize um modelo de outra tabela.
> Esta tela é especialmente útil na implantação de uma unidade nova: importar de uma vez, por exemplo, todas as U.H.s ou todos os Itens Gerais de um hotel, em vez de cadastrar um a um em Cadastro Básico.


### 4.2  Importar apontamentos de PMOC em massa

1. Acesse Upload > PMOC.
1. Selecione a Unidade.
1. Clique em Planilha para baixar o modelo de apontamento de PMOC.
1. Preencha a planilha com as execuções realizadas (item, data de execução, colaborador etc., conforme o modelo).
1. Selecione o Arquivo preenchido e envie.


> [!INFO]
> **RESULTADO ESPERADO**
> Os apontamentos da planilha entram no sistema como se tivessem sido registrados manualmente em PMOC (seção 3.6), útil quando um lote de execuções foi feito em campo (ex.: com papel ou planilha offline) e precisa ser digitado de uma vez.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade que receberá os dados importados | Sim | Hotel by SIM Services |
| Tabela | Tipo de cadastro a importar (upload de Cadastro Básico) | Sim | Equipamento |
| Planilha (modelo) | Modelo de colunas a ser baixado e preenchido | Sim | Baixar antes de preencher |
| Arquivo | Planilha preenchida a ser enviada | Sim | .xlsx |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Sempre baixe o modelo de Planilha na hora de fazer o upload, em vez de reaproveitar um arquivo antigo, o modelo pode ter sido atualizado.
> Faça uma carga de teste pequena (5-10 linhas) antes de importar uma planilha grande, mais fácil de corrigir erro de formatação num lote pequeno.
> Guarde a planilha enviada como comprovante, é a evidência do que foi importado, útil em caso de divergência posterior.


> [!DANGER]
> Uma importação em massa mal feita cadastra (ou apanta) um volume grande de dados incorretos de uma vez, sempre revise a planilha com atenção antes de enviar, o retrabalho de corrigir em massa é maior do que o de cadastrar um por um.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A importação falha ou não processa nenhuma linha | Planilha fora do modelo (colunas renomeadas, fora de ordem ou faltando) | Baixe o modelo novamente pelo link Planilha e preencha sem alterar a estrutura de colunas |
| Alguns registros da planilha não aparecem após a importação | Dependência não cadastrada (ex.: Setor referenciado que não existe) | Cadastre a dependência primeiro (ex.: Setor em seção 2.2) e reenvie apenas as linhas que falharam |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Planilha preenchida pelo usuário | Cadastro Básico (13 tabelas) e PMOC (apontamentos, seção 3.6) | Registros importados ficam disponíveis imediatamente nos módulos correspondentes |', NULL, NULL, NULL, NULL, 1);

-- Cadastro de Ativos Fixos [AtivoFixo/AssetIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'AtivoFixo', N'AssetIndex', N'Cadastro de Ativos Fixos', N'8.1 — Ativo Fixo', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:ativofixo') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Administradores, gestores de patrimônio e coordenadores de PCM. | Menu lateral > Ativo Fixo > Ativo Fixo pcmbysim.com.br/AtivoFixo/AssetIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de cadastrar cada bem patrimonial do hotel, móveis, eletrodomésticos, equipamentos, com sua etiqueta patrimonial, dados fiscais e localização física, formando a base de dados que sustenta todo o controle de Ativo Fixo: movimentações, inventários e depreciação.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> A Unidade, o Setor e o Local / U.H. onde o ativo está fisicamente precisam estar cadastrados: seções 2.1, 2.2 e 2.3.
> O Colaborador Responsável pelo ativo precisa estar cadastrado: seção 2.9.
> Tenha em mãos os dados fiscais do bem (nota fiscal, valor de compra, data de compra) antes de iniciar o cadastro.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Bem ADQUIRIDO | → | 2️⃣ Etiqueta GERADA | → | 3️⃣ Ativo CADASTRADO | → | 4️⃣ Localização/Responsável DEFINIDOS | → | 📊 Pronto p/ Movimentação |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Ativo Fixo > Ativo Fixo — listagem com filtros de Unidade, Código, Descrição, Status e Localização](/screenshots/ativo-fixo-listagem.png)


### 4.1  Cadastrar um novo ativo

1. Acesse Ativo Fixo > Ativo Fixo e clique em Novo.
1. Selecione a Unidade e preencha o Código (etiqueta patrimonial) e a Descrição do bem.
1. Preencha os dados de identificação: Nº Série e TAG (se aplicável ao tipo de bem).
1. Preencha os dados fiscais: Conta Contábil, Data de Compra, Valor de Compra, Tempo de Depreciação (em meses) e o número da Nota Fiscal.
1. Defina o Status inicial: ATIVO, EM MANUTENÇÃO, BAIXADO ou TRANSFERIDO.
1. Selecione o Setor e o Local / U.H. onde o bem está fisicamente localizado.
1. Selecione o Responsável pelo ativo.
1. Anexe uma foto ou documento do bem em Arquivo, se desejar.
1. Clique em Salvar.


> [!INFO]
> **IMPORTANTE SABER**
> O Código funciona como a etiqueta patrimonial física do bem, a mesma numeração que deve estar afixada no objeto real, permitindo conferência visual direta durante os inventários (seção 8.4).


### 4.2  Consultar e filtrar o cadastro de ativos

1. Acesse Ativo Fixo > Ativo Fixo.
1. Use os filtros Unidade, Código, Descrição, Status (Todos/Ativo/Manutenção/Baixado/Transferido) e Localização.
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Lista de ativos com Unidade, Código, Descrição, Status e Local, clique em um registro para ver o detalhe completo ou editar.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Código | Etiqueta patrimonial do bem | Sim | 1083414 |
| Descrição | Nome/identificação do bem | Sim | Cama Box Bipartida Queen Size |
| Nº Série | Número de série do fabricante | Não | — |
| TAG | Identificador técnico adicional | Não | — |
| Status | Situação atual do ativo | Sim | ATIVO / BAIXADO / EM MANUTENÇÃO / TRANSFERIDO |
| Setor / Local / U.H. | Localização física atual do bem | Sim | 3º ANDAR / 0301 |
| Valor de Compra | Valor pago pelo bem, base para depreciação | Não | R$ 1.200,00 |
| Tempo de Depreciação | Vida útil contábil do bem, em meses | Não | 60 |
| Responsável | Colaborador responsável pelo ativo | Não | Governanta |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Cadastre o ativo no sistema no mesmo dia em que a etiqueta patrimonial física é afixada no bem, evita etiquetas ''órfãs'' sem registro correspondente.
> Preencha sempre o Setor e o Local / U.H. reais, é esse campo que orienta a conferência física durante os inventários (seção 8.4).
> Mantenha o Status atualizado: um bem em manutenção prolongada deve ficar como EM MANUTENÇÃO, não ATIVO, para não distorcer os relatórios de disponibilidade.


> [!DANGER]
> Nunca reutilize um Código de etiqueta patrimonial de um ativo já baixado para um bem novo, isso quebra o histórico de movimentações (seção 8.2) do ativo original.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Um ativo não aparece na busca por Localização | O Setor ou Local / U.H. foi preenchido incorretamente no cadastro | Edite o ativo e corrija o Setor e o Local / U.H. |
| O Status do ativo não reflete a realidade | O ativo mudou de situação (quebrou, foi transferido) mas o cadastro não foi atualizado | Registre a mudança pela tela de Movimentação (seção 8.2), que atualiza o Status automaticamente |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Unidades, Setor e U.H. (seções 2.1 a 2.3) / Cadastro de Colaboradores (seção 2.9) | Movimentação de Ativos Fixos (seção 8.2) / Inventário de Ativos Fixos (seções 8.3 e 8.4) | O ativo cadastrado fica disponível imediatamente para seleção em movimentações e contagens de inventário |', NULL, NULL, NULL, NULL, 1);

-- Movimentação de Ativos Fixos [AtivoFixo/assetMovement]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'AtivoFixo', N'assetMovement', N'Movimentação de Ativos Fixos', N'8.2 — Ativo Fixo', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:ativofixo') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Administradores e gestores de patrimônio. | Menu lateral > Ativo Fixo > Movimentação pcmbysim.com.br/AtivoFixo/assetMovement |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar formalmente toda mudança relevante na vida de um ativo, baixa (descarte), manutenção ou transferência de localização, mantendo um histórico completo e rastreável de cada bem patrimonial.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> O ativo precisa já estar cadastrado: seção 8.1.
> Para transferências, tenha definido o Setor e o Local / U.H. de destino antes de iniciar.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Evento IDENTIFICADO | → | 2️⃣ Movimentação REGISTRADA | → | 3️⃣ Status/Localização ATUALIZADOS | → | 📊 Histórico Consultável |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Ativo Fixo > Movimentação — filtros de Unidade, Tipo, Documento, Período, Origem e Destino](/screenshots/ativo-fixo-movimentacao.png)


### 4.1  Registrar uma movimentação

1. Acesse Ativo Fixo > Movimentação e clique em Novo.
1. Selecione a Unidade e o Tipo de movimentação: BAIXA, MANUTENÇÃO ou TRANSFERÊNCIA.
1. Preencha a Data da movimentação e o Nº Documento (referência interna, ex.: número da nota ou ordem de serviço relacionada).
1. Selecione o Ativo Fixo (pelo Código cadastrado na seção 8.1).
1. Para TRANSFERÊNCIA, selecione o novo Setor e Local / U.H. de destino.
1. Se aplicável, preencha o Fornecedor (ex.: empresa que fez a manutenção) e o Valor da movimentação.
1. Preencha a Observação e anexe um Arquivo (nota fiscal, laudo, foto) se necessário.
1. Clique em Salvar.


> [!INFO]
> **IMPORTANTE SABER**
> O Tipo de movimentação escolhido reflete diretamente no Status do ativo cadastrado (seção 8.1): BAIXA leva o ativo a BAIXADO, MANUTENÇÃO leva a EM MANUTENÇÃO, e TRANSFERÊNCIA atualiza o Setor/Local do ativo mantendo o Status ATIVO.


### 4.2  Consultar o histórico de movimentações

1. Acesse Ativo Fixo > Movimentação.
1. Filtre por Unidade, Ativo Fixo específico, Tipo de movimentação, Nº Documento, Período (De/Até), Origem ou Destino.
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Lista com Nº Documento, Data, Ativo, Descrição do Ativo, Tipo de Movimentação, Origem, Destino e Valor de cada movimentação registrada, a linha do tempo completa de mudanças de cada bem.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Tipo de movimentação | Natureza do evento registrado | Sim | BAIXA / MANUTENÇÃO / TRANSFERÊNCIA |
| Data movimentação | Quando o evento ocorreu | Sim | 14/08/2026 |
| Nº Documento | Referência interna da movimentação | Não | OS-00123 |
| Ativo Fixo | Bem sendo movimentado (pelo Código) | Sim | 1083414 |
| Setor / Local de destino | Novo local do ativo (transferências) | Sim (transferência) | 2º ANDAR / 0210 |
| Valor | Custo associado à movimentação (ex.: manutenção) | Não | R$ 250,00 |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Registre a movimentação no mesmo dia do evento físico, um ativo fisicamente transferido sem registro no sistema gera divergência no próximo inventário (seção 8.4).
> Sempre preencha o Nº Documento com uma referência real (nota fiscal, OS), facilita auditoria futura do motivo da movimentação.


> [!DANGER]
> Uma TRANSFERÊNCIA salva sem o Setor e o Local / U.H. de destino corretamente preenchidos **não atualiza o Status do ativo**, o sistema retorna à listagem normalmente, sem mensagem de erro, mas o ativo continua com o Status e a localização antigos. Sempre confirme, depois de salvar, que o Status do ativo mudou (seção 8.1) antes de considerar a movimentação concluída.


> [!DANGER]
> Uma BAIXA é, na prática operacional, definitiva, trate como o descarte real do bem. Confirme que o ativo realmente saiu de uso antes de registrar, para não perder rastreabilidade de um bem que ainda está em operação.
> A BAIXA funciona como o Controle de Baixas do módulo: arquiva todas as informações do ativo (histórico de movimentações, fotos de inventário, seção 8.5) em vez de apagá-las. Use a Observação e o Arquivo anexado para registrar o motivo real do descarte.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O Status do ativo não mudou após a movimentação | O Setor e/ou Local / U.H. de destino não foram selecionados corretamente ao salvar (falha silenciosa confirmada em teste ao vivo) | Acesse o cadastro do ativo (seção 8.1), confirme se o Status foi atualizado e, se não, refaça a movimentação garantindo que Setor e Local / U.H. de destino estejam realmente preenchidos antes de clicar em Salvar |
| Não encontro o ativo na lista de seleção da movimentação | O ativo não está cadastrado ou está com Status BAIXADO | Verifique o cadastro em Ativo Fixo > Ativo Fixo (seção 8.1) |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro de Ativos Fixos (seção 8.1) | Histórico patrimonial do bem / Relatórios de custo de manutenção de ativos | Status e localização do ativo atualizados automaticamente a cada movimentação |', NULL, NULL, NULL, NULL, 1);

-- Gestão de Inventário de Ativos Fixos [AtivoFixo/assetInventoryMng]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'AtivoFixo', N'assetInventoryMng', N'Gestão de Inventário de Ativos Fixos', N'8.3 — Ativo Fixo', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:ativofixo') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Administradores e gestores de patrimônio. | Menu lateral > Ativo Fixo > Gestão de Inventário pcmbysim.com.br/AtivoFixo/assetInventoryMng |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de abrir, acompanhar e encerrar as campanhas de inventário físico de ativos fixos, o processo periódico que confirma se o que está cadastrado no sistema realmente existe nos andares do hotel.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!INFO]
> **DUAS TELAS QUE TRABALHAM JUNTAS**
> Esta seção (Gestão de Inventário) controla o **ciclo da campanha**, quando um inventário começa e termina. A contagem física propriamente dita, item por item, é feita na tela Inventário (seção 8.4). Faça as duas em conjunto: abra a campanha aqui, depois conte os ativos lá.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Campanha ABERTA | → | 2️⃣ Contagem FÍSICA | → | 3️⃣ Contagem REGISTRADA | → | 4️⃣ Divergências IDENTIFICADAS | → | 📊 Campanha Finalizada |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Ativo Fixo > Gestão de Inventário — listagem de campanhas com Status ABERTO/EM EXECUÇÃO/FINALIZADO](/screenshots/ativo-fixo-gestao-inventario.png)


### 4.1  Consultar campanhas de inventário

1. Acesse Ativo Fixo > Gestão de Inventário.
1. Filtre por Unidade, Descrição, Período (De/Até) ou Status: ABERTO, EM EXECUÇÃO ou FINALIZADO.
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Lista de campanhas com Unidade, Descrição, Data Início, Usuário Início, Data Término, Usuário Término e Status. Clique no ícone ''+'' de uma linha para expandir os detalhes da campanha.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Descrição | Nome/identificação da campanha de inventário | Sim | controle de ativos - pcm by sim |
| Status | Situação atual da campanha | Auto | ABERTO / EM EXECUÇÃO / FINALIZADO |
| Data Início / Usuário Início | Quando e quem abriu a campanha | Auto | 14/08/2026: PCM |
| Data Término / Usuário Término | Quando e quem encerrou a campanha | Auto | Preenchido ao finalizar |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Abra uma campanha de inventário por vez, por unidade, campanhas simultâneas na mesma unidade dificultam saber qual contagem é a válida.
> Planeje a campanha para um período de baixa ocupação, minimizando o incômodo de contar bens em áreas de hóspedes.


> [!DANGER]
> Não finalize uma campanha de inventário sem revisar as divergências encontradas na contagem (seção 8.4), finalizar sem investigar perpetua erros no cadastro.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Não sei qual campanha está ativa para contar | Mais de uma campanha aberta para a mesma unidade | Filtre por Status ABERTO ou EM EXECUÇÃO e confirme com o gestor qual é a campanha vigente |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro de Ativos Fixos (seção 8.1) | Contagem de Inventário de Ativos (seção 8.4) | A campanha aberta aqui fica disponível para contagem na tela Inventário |', NULL, NULL, NULL, NULL, 1);

-- Contagem de Inventário de Ativos Fixos [AtivoFixo/assetInventoryMng]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'AtivoFixo', N'assetInventoryMng', N'Contagem de Inventário de Ativos Fixos', N'8.4 — Ativo Fixo', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:ativofixo') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Equipe de campo designada para a contagem física (governança, manutenção ou administração). | Menu lateral > Ativo Fixo > Inventário pcmbysim.com.br/AtivoFixo/assetInventory |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de realizar a contagem física dos ativos, andar por andar e ambiente por ambiente, confirmando no sistema que cada bem etiquetado realmente está onde o cadastro (seção 8.1) diz que está.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Uma campanha de inventário precisa estar ABERTA ou EM EXECUÇÃO: seção 8.3.
> Tenha a lista de etiquetas patrimoniais visível ou o leitor de código à mão para agilizar a contagem.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Campanha ABERTA | → | 2️⃣ Setor/Local SELECIONADOS | → | 3️⃣ Código do Ativo INFORMADO | → | 4️⃣ Contagem REGISTRADA | → | 📊 Divergências Analisadas |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Ativo Fixo > Inventário — seleção de Setor, Local/U.H. e campo de código do ativo para contagem](/screenshots/ativo-fixo-inventario-contagem.png)


### 4.1  Contar os ativos de um ambiente

1. Acesse Ativo Fixo > Inventário.
1. Selecione o Setor (ex.: ''3º ANDAR'') a ser contado.
1. Selecione o Local / U.H. específico dentro do Setor (ex.: ''0301'').
1. Informe o Código do Ativo Fixo encontrado fisicamente no ambiente (digitando ou lendo a etiqueta) e confirme.
1. Repita o processo para cada ativo encontrado no ambiente, depois avance para o próximo Local / U.H.


> [!INFO]
> **IMPORTANTE SABER**
> Cada Código informado é conferido contra o cadastro de Ativos Fixos (seção 8.1) para aquele Setor/Local, ativos cadastrados para o ambiente e não informados na contagem, ou ativos informados que não deveriam estar ali, geram as divergências que a Gestão de Inventário (seção 8.3) usa para o fechamento da campanha.


> [!INFO]
> **MOVIMENTAÇÃO AUTOMÁTICA POR DIVERGÊNCIA DE LOCAL**
> Contar um ativo num Local / U.H. diferente do cadastrado gera automaticamente uma Movimentação de Transferência (seção 8.2), em vez de só apontar a divergência para ajuste manual depois.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Setor | Andar ou área do hotel sendo contado | Sim | 3º ANDAR |
| Local / U.H. | Ambiente específico dentro do Setor | Sim | 0301 |
| Código do Ativo Fixo | Etiqueta patrimonial lida/digitada durante a contagem | Sim | 1083414 |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Conte ambiente por ambiente, sem pular, contagens feitas ''de memória'' fora do local físico são a principal causa de divergência falsa no inventário.
> Divida a contagem por dupla (um lê a etiqueta, outro confirma no sistema) em áreas com muitos ativos, como salões de eventos.


> [!DANGER]
> Um ativo contado no Local / U.H. errado gera duas divergências (falta no ambiente correto, sobra no ambiente errado), confirme sempre o Setor e o Local / U.H. antes de começar a informar códigos.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O Código do ativo não é aceito na contagem | O ativo não está cadastrado, está BAIXADO, ou pertence a outro Setor/Local | Verifique o cadastro em Ativo Fixo > Ativo Fixo (seção 8.1) e corrija o Setor/Local se necessário |
| Muitos ativos aparecem como divergência ao final | Ativos foram transferidos fisicamente sem registro de Movimentação (seção 8.2) | Registre as movimentações reais antes da próxima campanha, para que o cadastro reflita a localização física |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro de Ativos Fixos (seção 8.1) / Gestão de Inventário (seção 8.3) | Relatório de divergências da campanha de inventário | Cada código informado é conferido em tempo real contra o cadastro do Setor/Local selecionado |', NULL, NULL, NULL, NULL, 1);

-- Cadastro e Registro Fotográfico no Inventário (Hand-on/Hand-off) [AtivoFixo/assetInventoryMng]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'AtivoFixo', N'assetInventoryMng', N'Cadastro e Registro Fotográfico no Inventário (Hand-on/Hand-off)', N'8.5 — Ativo Fixo', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:ativofixo') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Equipe de campo designada para a contagem física (governança, manutenção ou administração). | Menu lateral > Ativo Fixo > Inventário pcmbysim.com.br/AtivoFixo/assetInventory — mesma tela da Contagem de Inventário (seção 8.4). |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de aproveitar a contagem física do inventário (seção 8.4) para corrigir falhas de cadastro em tempo real, cadastrando na hora um ativo encontrado no ambiente que ainda não existia no sistema, e de acompanhar o estado de conservação de cada bem ao longo do tempo através de fotos comparativas: a primeira foto (Hand-on) tirada no primeiro inventário do ativo, e as fotos dos inventários seguintes (Hand-off) usadas para comparar e identificar itens quebrados, danificados ou fora de conformidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Uma campanha de inventário precisa estar ABERTA ou EM EXECUÇÃO: seção 8.3.
> Isso resolve um problema real de operação: ativos que existem fisicamente mas nunca foram cadastrados só eram descobertos como ''sobra'' na divergência do inventário (seção 8.4) — agora podem ser corrigidos no mesmo momento da contagem.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Ativo sem CADASTRO | → | 2️⃣ Cadastro na HORA | → | 3️⃣ 1ª Foto HAND-ON | → | 4️⃣ Comparação HAND-OFF | → | 5️⃣ Divergência IDENTIFICADA | → | 📊 Não Conforme? Marcado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Cadastrar um ativo durante o inventário

1. Na tela de Inventário (seção 8.4), ao não encontrar o Código de um ativo existente fisicamente no ambiente, use a opção de cadastro rápido.
1. Preencha os dados mínimos do ativo — os mesmos campos essenciais do cadastro completo (seção 8.1): Descrição, Setor e Local / U.H. (já pré-preenchidos pelo contexto do inventário em andamento).
1. O ativo passa a existir no cadastro (seção 8.1) e é automaticamente contabilizado como contado nesta campanha.


### 4.2  Registrar a foto do ativo (Hand-on / Hand-off)

1. No primeiro inventário em que um ativo é contado, o sistema solicita (ou permite) o registro de uma foto do bem, esse é o registro Hand-on, a referência visual do estado original.
1. Nos inventários seguintes, uma nova foto é tirada do mesmo ativo, o Hand-off, permitindo comparar visualmente o estado atual contra o registrado anteriormente.
1. Caso a comparação mostre dano, quebra ou desgaste fora do esperado, o item pode ser marcado como não conforme.


> [!INFO]
> **POR QUE ISSO IMPORTA**
> Antes, o controle de conservação dos ativos dependia de inspeção visual e memória da equipe. Com a foto de cada inventário, fica documentado objetivamente como o bem evoluiu ao longo do tempo, útil tanto para decidir manutenção/substituição quanto para justificar uma Baixa (seção 8.2) com evidência.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Cadastro rápido durante inventário | Permite cadastrar um ativo não encontrado no sistema, direto da contagem | A confirmar | — |
| Foto Hand-on | Foto do ativo no seu primeiro inventário | A confirmar | Referência de estado original |
| Foto Hand-off | Foto do ativo nos inventários seguintes | A confirmar | Comparação de conservação |
| Não Conformidade | Marcação de item quebrado/danificado identificado na comparação | A confirmar | — |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Tire a primeira foto (Hand-on) com boa iluminação e enquadramento padronizado, é a referência que todas as comparações futuras vão usar.
> Cadastre o ativo encontrado sem registro na hora, durante a própria contagem, deixar para depois é a forma mais comum desse tipo de pendência nunca ser resolvida.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Não é possível comparar a conservação de um ativo | O ativo foi cadastrado antes desta funcionalidade existir e não tem foto Hand-on registrada | A primeira foto tirada a partir de agora passa a valer como novo Hand-on de referência |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Contagem de Inventário (seção 8.4) | Cadastro de Ativos Fixos (seção 8.1): ativos cadastrados na hora / Movimentação (seção 8.2): possível Baixa por não conformidade | Ativo cadastrado durante o inventário já entra contabilizado na campanha em andamento |', NULL, NULL, NULL, NULL, 1);

-- Controle de Gastos (Budget) [Financas/ControleGastoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Financas', N'ControleGastoIndex', N'Controle de Gastos (Budget)', N'A.1 — Financeiro', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:financeiro') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores financeiros, diretores e coordenadores de PCM. | Menu lateral > Finanças > Controle de Gastos pcmbysim.com.br/Financas/ControleGastoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de planejar o orçamento anual da unidade por departamento e acompanhar em tempo real o gasto real acumulado versus o previsto, a ferramenta central de prestação de contas financeira do PCM by SIM para a diretoria.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Os Departamentos precisam estar cadastrados: seção 2.11.
> Os Tipos de Despesa precisam estar cadastrados antes do primeiro lançamento: Cadastro Básico > Tipo de Despesa.
> Defina o ciclo de planejamento (mensal ou anual) com a diretoria antes de inserir a primeira previsão.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Budget DEFINIDO | → | 2️⃣ Despesas LANÇADAS | → | 3️⃣ Contratos REGISTRADOS | → | 4️⃣ Gasto Real ACUMULADO | → | 📊 Saldo Calculado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Finanças > Controle de Gastos — tabela comparativa Previsão vs Gasto Real vs Saldo](/screenshots/financas-controle-gastos.png)


1. Acesse Finanças > Controle de Gastos.
1. Para o início de cada ano ou ciclo, insira a Previsão de Gasto por mês e departamento.
1. O sistema popula automaticamente o Gasto Real com base em: saídas de estoque + lançamentos de despesa (seção A.2) + valores de contratos (seção A.3).
1. O Saldo (Previsão − Gasto Real) aparece em verde se dentro do orçamento, ou vermelho se estourado.


> [!INFO]
> **IMPORTANTE SABER**
> O budget é a principal ferramenta de prestação de contas para a diretoria.
> Um gasto real consistentemente abaixo da previsão pode indicar atraso em manutenções necessárias, não necessariamente eficiência.
> Um gasto real consistentemente acima da previsão indica necessidade de revisão do orçamento ou dos processos operacionais.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Mês/Competência | Período de referência do orçamento | Sim | Maio/2025 |
| Departamento | Centro de custo sendo planejado | Sim | Manutenção |
| Previsão de Gasto | Valor orçado para o período | Sim | R$ 15.000,00 |
| Gasto Real | Calculado automaticamente pelo sistema | --- | Somatório automático |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Revise o budget mensalmente comparando com o gasto real acumulado, não apenas no fechamento do ano.
> Alinhe os Tipos de Despesa (usados no lançamento, seção A.2) com o plano de contas do financeiro corporativo antes de começar, facilita a auditoria.


> [!DANGER]
> Um saldo estourado (vermelho) recorrente sem justificativa documentada é um dos primeiros pontos verificados em auditorias corporativas, mantenha o motivo registrado assim que identificar o desvio.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O Gasto Real está muito diferente do esperado | Saídas de estoque ou despesas lançadas com o departamento ou competência errados | Filtre as despesas e saídas de estoque do período e corrija a competência/departamento incorretos |
| A Previsão de Gasto não aparece para um departamento | O departamento ainda não teve o budget do período cadastrado | Acesse Finanças > Controle de Gastos e insira a previsão do departamento para o período |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Estoque > Saída (custo automático) / Finanças > Despesa (seção A.2) / Finanças > Contrato (seção A.3) | Dashboard de custo operacional / Reuniões de diretoria | Gasto Real recalculado automaticamente a cada novo lançamento |', NULL, NULL, NULL, NULL, 1);

-- Lançamento de Despesas [Financas/DespesaIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Financas', N'DespesaIndex', N'Lançamento de Despesas', N'A.2 — Financeiro', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:financeiro') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores financeiros e coordenadores de PCM. | Menu lateral > Finanças > Despesa pcmbysim.com.br/Financas/DespesaIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar despesas que não passam pelo estoque, serviços de terceiros, taxas, despesas administrativas, com evidência digital anexada, alimentando automaticamente o Controle de Gastos (seção A.1).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> O Departamento responsável precisa estar cadastrado: seção 2.11.
> O Tipo de Despesa precisa estar cadastrado antes do lançamento: Cadastro Básico > Tipo de Despesa.
> Tenha a nota fiscal, recibo ou comprovante digitalizado em mãos antes de iniciar, o campo de evidência é obrigatório na prática de auditoria.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Despesa IDENTIFICADA | → | 2️⃣ Competência DEFINIDA | → | 3️⃣ Evidência ANEXADA | → | 4️⃣ Despesa SALVA | → | 📊 Gasto Real Atualizado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Finanças > Despesa — formulário de novo lançamento](/screenshots/financas-despesa-novo.png)


### 4.1  Lançar uma despesa

1. Acesse Finanças > Despesa e clique em Novo.
1. Preencha a Competência (mês ao qual o gasto pertence) e o Tipo de Despesa.
1. Informe o Valor e o Departamento responsável.
1. Faça o upload da evidência (nota fiscal, recibo ou comprovante).
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A despesa passa a compor o Gasto Real do departamento e da competência selecionados no Controle de Gastos (seção A.1).


### 4.2  Cadastrar Tipos de Despesa (cadastro auxiliar)

1. Acesse Cadastro Básico > Tipo de Despesa e clique em Novo.
1. Preencha a Descrição do tipo (ex.: Materiais de Manutenção, Serviços Terceirizados).
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O novo tipo passa a aparecer na lista de seleção do passo 4.1, ao lançar uma despesa.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Competência | Mês de referência do gasto: regime de competência | Sim | Maio/2025 |
| Tipo de Despesa | Categoria do gasto | Sim | Materiais de Manutenção / Serviços Terceirizados |
| Departamento | Centro de custo responsável | Sim | Manutenção |
| Evidência (Anexo) | Nota fiscal, recibo ou comprovante digitalizado | Sim | PDF ou imagem |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Lance despesas sempre com a competência correta, uma despesa lançada no mês errado distorce o comparativo mensal de orçamento (seção A.1).
> Configure os Tipos de Despesa alinhados com o plano de contas do financeiro corporativo.


> [!DANGER]
> Despesas sem evidência digital ficam vulneráveis em auditorias. Implante a regra: sem nota fiscal digitalizada, sem lançamento aprovado.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Um Tipo de Despesa não aparece para seleção | O tipo está inativo ou não foi cadastrado | Acesse Cadastro Básico > Tipo de Despesa e verifique o cadastro e o switch Ativo |
| A despesa lançada não aparece no Controle de Gastos | Competência ou Departamento preenchidos incorretamente | Edite o lançamento e corrija a Competência e o Departamento |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Tipo de Despesa e Departamento (seção 2.11) | Finanças > Controle de Gastos (seção A.1) — Gasto Real | Soma automática no Gasto Real do departamento e competência lançados |', NULL, NULL, NULL, NULL, 1);

-- Gestão de Contratos [AEB/ContratoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'AEB', N'ContratoIndex', N'Gestão de Contratos', N'A.3 — Financeiro', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:financeiro') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores financeiros, diretores e coordenadores de PCM. | Menu lateral > Finanças > Contrato pcmbysim.com.br/Financas/ContratoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de controlar os contratos recorrentes com fornecedores, vigência, valor mensal e renovação, recebendo alertas automáticos antes do vencimento e evitando interrupção de serviços essenciais por falta de acompanhamento.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> O Fornecedor precisa estar cadastrado: seção 2.15.
> Tenha em mãos a Data de Início, Data de Término e Valor Mensal definidos no contrato assinado com o fornecedor.
> Defina quem é o responsável por acompanhar as renovações, sem um dono claro, os vencimentos passam despercebidos.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Fornecedor SELECIONADO | → | 2️⃣ Contrato CADASTRADO | → | 3️⃣ Vigência MONITORADA | → | 4️⃣ Renovação AVALIADA | → | 📊 Renovar ou Encerrar |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Finanças > Contrato — formulário de cadastro com vigência e valor mensal](/screenshots/financas-contrato-novo.png)


1. Acesse Finanças > Contrato e clique em Novo.
1. Selecione o Fornecedor e preencha: Descrição do serviço, Data de Início, Data de Término e Valor Mensal.
1. O sistema gera alertas de renovação 30 dias antes do vencimento.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> Contrato ativo, com o valor mensal somado automaticamente ao Gasto Real do departamento no Controle de Gastos (seção A.1).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Fornecedor | Empresa contratada | Sim | Empresa de Dedetização XYZ |
| Descrição do Serviço | O que o contrato cobre | Sim | Manutenção de elevadores |
| Data de Início / Término | Vigência do contrato | Sim | 01/01/2025 a 31/12/2025 |
| Valor Mensal | Custo recorrente somado ao Gasto Real | Sim | R$ 3.500,00 |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Revise os contratos ativos mensalmente pelo filtro de ''Vencendo nos próximos 60 dias'', renovações tardias são mais caras e podem gerar interrupção no serviço.


> [!DANGER]
> Contratos vencidos que continuam sendo pagos representam risco jurídico. O sistema alerta, mas a ação de renovar ou encerrar precisa ser do gestor.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O alerta de contrato vencendo não chegou | O campo de Data de Término do contrato está incorreto ou em branco | Edite o contrato e preencha corretamente a Data de Término |
| O valor do contrato não aparece no Gasto Real | O contrato está fora da vigência (Data de Início futura ou Data de Término já passada) | Verifique as datas de vigência do contrato em Finanças > Contrato |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Fornecedores (seção 2.15) | Finanças > Controle de Gastos (seção A.1) — Gasto Real mensal | Alerta automático de renovação 30 dias antes do vencimento |', NULL, NULL, NULL, NULL, 1);

-- Administração de Usuários e Permissões [Administracao/UsuarioIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Administracao', N'UsuarioIndex', N'Administração de Usuários e Permissões', N'Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Exclusivo para Administradores do sistema e gestores com permissão ao módulo Administração. | Menu lateral > Administração > Usuários pcmbysim.com.br/Administracao/UsuarioIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de criar novos acessos para colaboradores, editar permissões de usuários existentes, inativar contas de desligados e redefinir senhas sem depender de suporte técnico externo. Uma gestão de usuários bem feita garante que cada colaborador acesse apenas o que precisa, que os apontamentos sejam rastreáveis por pessoa e que o cálculo de produtividade e custo de mão de obra seja preciso.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Você precisa ter perfil Administrador para acessar e modificar usuários.
> Antes de criar um usuário, certifique-se de que os seguintes cadastros já existem no sistema:
> — Departamento do colaborador: Cadastro Básico > Departamento
> — Função/cargo do colaborador: Cadastro Básico > Função
> — Perfil de acesso correspondente: Administração > Perfil
> O ID-Usuário (login) deve estar no formato de e-mail e ser único em toda a rede, não pode se repetir em nenhuma unidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Usuários ACESSADO | → | 2️⃣ Usuário CRIADO | → | 3️⃣ Perfil DEFINIDO | → | 4️⃣ Colaborador VINCULADO | → | 📊 Usuário Salvo |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Acessar e filtrar a listagem de usuários

![Tela de listagem de usuários — filtros superiores e tabela de dados](/screenshots/admin-usuario-listagem.png)


1. No menu lateral, clique em Administração e depois em Usuários.
1. Use os filtros para localizar usuários existentes: Unidade, Nome, Departamento e Ativo (Sim/Não).
1. Clique em Filtrar para atualizar a listagem com os critérios escolhidos.


> [!INFO]
> **IMPORTANTE SABER**
> Ações disponíveis na listagem para cada usuário:
> Editar (lápis), abre o cadastro completo para alterações.
> Excluir (X), remove o acesso permanentemente após confirmação.
> Redefinir Senha (setas circulares), atalho para resetar a senha sem abrir o cadastro completo.


### 4.2  Criar um novo usuário

![Formulário de cadastro de usuário — blocos A, B, C, D e E](/screenshots/admin-usuario-novo.png)


1. Na tela de listagem, clique no botão Novo no canto superior direito.


**Bloco A, Dados de conexão e perfil de acesso**

1. Selecione a Unidade principal do colaborador.
1. Escolha o Módulo de acesso, pode marcar mais de um (ex: Manutenção + Governança).
1. Defina o Módulo Padrão: a tela que aparecerá primeiro após o login.
1. Preencha o ID-Usuário no formato de e-mail (ex: joao.silva@intercity.berrini). Deve ser único em toda a rede.
1. Informe o E-mail de Recuperação de Senha para envio de links de redefinição.


**Bloco B, Identificação e setorização**

1. Digite o Nome completo do colaborador.
1. Preencha o Apelido, nome abreviado que aparecerá nas OS físicas e notificações.
1. Selecione o Perfil de segurança (ex: Governança, Execução, Técnico PCM, Gestor de Manutenção, nomes simplificados neste manual para ficarem compreensíveis independente do nome exato do perfil configurado; confira a lista real de perfis da sua unidade em Administração > Perfil, Hierarquia, seção 1.4).
1. Associe o Departamento.
1. Defina a Senha e repita em Confirmar Senha, é a chave de acesso inicial do usuário.
1. Opcionalmente, informe o Celular de contato.


**Bloco C, Switches de acesso**

1. Ative Acesso via Aplicativo para técnicos e camareiras que usarão o app mobile.
1. Ative Acesso via Website para quem usará computadores e notebooks.
1. Mantenha Ativo habilitado para que o login funcione imediatamente após o cadastro.
1. Ative Colaborador se este usuário também executará tarefas e terá Ordens de Serviço atribuídas.


**Bloco D, Dados financeiros do colaborador (aparece ao ativar ''Colaborador'')**

1. Mantenha Contabiliza Hora ativado para que as horas de execução entrem nos relatórios de produtividade.
1. Informe o Valor Hora (custo bruto ÷ horas mensais contratadas). Exemplo: R$ 15,50.
1. Selecione o Tipo de Funcionário: Próprio (CLT) ou Terceiro (prestador de serviços).
1. Selecione a Função correspondente ao cargo (ex: Oficial de Manutenção, Eletricista, Camareira).


**Bloco E, Acesso a múltiplas unidades**

1. Na tabela inferior, ative o switch ao lado de cada unidade que este usuário deve acessar. Para colaboradores de apenas uma unidade, ative somente a unidade de lotação.
1. Revise todos os campos e clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O sistema válida os dados e exibe um pop-up de confirmação de cadastro.
> O usuário já pode fazer login com o ID-Usuário e a senha definidos.
> Se o switch Colaborador foi ativado, o nome do técnico já aparece disponível para atribuição em Ordens de Serviço.


### 4.3  Editar ou inativar um usuário existente

1. Na listagem de usuários, localize o colaborador e clique no ícone de Editar (lápis).
1. Faça as alterações necessárias nos campos, todos os blocos ficam disponíveis para edição.
1. Para inativar sem excluir, desative o switch Ativo. O histórico de apontamentos do usuário é preservado.
1. Clique em Salvar para confirmar.


> [!DANGER]
> Ao clicar em Excluir (X), o usuário é removido permanentemente. Prefira a opção de inativar para preservar o histórico de apontamentos e garantir a rastreabilidade das ações anteriores.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade principal de lotação do colaborador | Sim | Intercity Berrini |
| Módulo | Área de trabalho no sistema: pode marcar mais de um | Sim | Manutenção, Governança |
| Módulo Padrão | Tela exibida ao fazer login | Sim | Manutenção |
| ID-Usuário | Login de acesso: formato e-mail, único na rede | Sim | joao.silva@intercity.berrini |
| E-mail Recuperação | Endereço para envio de links de redefinição de senha | Não | joao@email.com |
| Nome | Nome completo: aparece em OS e relatórios | Sim | João Silva |
| Apelido | Nome curto para OS físicas e notificações | Não | João |
| Perfil | Nível de permissão e visibilidade no sistema | Sim | Técnico PCM |
| Departamento | Setor ao qual o colaborador pertence | Sim | Manutenção |
| Senha / Confirmar Senha | Chave de acesso inicial do usuário | Sim | Mínimo 8 caracteres |
| Acesso via App | Habilita uso do aplicativo mobile | — | Ativar para técnicos e camareiras |
| Acesso via Website | Habilita uso pelo navegador em computadores | — | Ativar para gestores |
| Ativo | Define se o login está habilitado | — | Sempre ativo para novos usuários |
| Colaborador | Habilita atribuição de OS e cálculo de produtividade | — | Ativar para técnicos executores |
| Valor Hora | Custo/hora para cálculo de despesa de mão de obra em OS | — | R$ 15,50 |
| Tipo de Funcionário | Vínculo empregatício: Próprio (CLT) ou Terceiro | — | Próprio |
| Função | Cargo/especialidade técnica do colaborador | — | Oficial de Manutenção |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Inative imediatamente o usuário de qualquer colaborador desligado. Um login ativo de ex-funcionário é uma brecha de segurança grave, apontamentos podem ser feitos indevidamente em nome da empresa.
> Sempre preencha o Valor Hora corretamente. Este dado alimenta o cálculo de custo de manutenção por OS, que é a base para justificar investimentos em Capex.
> Crie um usuário por pessoa, nunca crie logins genéricos como ''tecnico01''. A rastreabilidade depende de que cada ação seja vinculada a uma pessoa real.
> Revise a lista de usuários ativos a cada 90 dias, é comum encontrar logins esquecidos de colaboradores transferidos ou desligados.


> [!DANGER]
> O ID-Usuário não pode ser repetido em nenhuma unidade da rede, mesmo que o colaborador seja de outra cidade. O sistema rejeitará o cadastro em caso de duplicidade.
> Usuários sem o switch ''Colaborador'' ativado não aparecem na lista de executores ao abrir uma OS, certifique-se de ativar para todos os técnicos de campo.
> Nunca use a função Excluir para desligamentos. Use sempre a inativação, os apontamentos históricos ficam vinculados ao ID e são necessários para auditorias.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Sistema rejeita o ID-Usuário ao salvar | Login já existe em outra unidade ou está duplicado na rede | Tente um ID-Usuário diferente: adicione o nome da unidade ao e-mail, ex: joao.silva@intercity.berrini2 |
| Colaborador não aparece como executor nas OS | Switch ''Colaborador'' não foi ativado no cadastro | Edite o usuário, ative o switch Colaborador e salve: o nome aparecerá imediatamente |
| Usuário não consegue acessar um módulo específico | O perfil atribuído não tem permissão para aquele módulo | Acesse Administração > Perfil, confirme se o módulo está habilitado. Se necessário, crie um perfil com as permissões corretas |
| Usuário consegue ver dados de outra unidade sem autorização | O switch da unidade indevida está ativo na tabela multihotéis | Edite o usuário, role até o Bloco E e desative o switch da unidade que ele não deveria acessar |
| O botão Salvar não responde ao criar novo usuário | Campo obrigatório vazio ou formato incorreto no ID-Usuário | Verifique os campos Unidade, Módulo, ID-Usuário (formato e-mail), Nome e Perfil. Todos obrigatórios |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Administração > Perfil e Hierarquia (seção 1.4) | Cadastro Básico > Colaborador — espelho operacional do usuário | Login ativo libera acesso imediato a todos os módulos do perfil |
| Cadastro Básico > Departamento e Função | Ordens de Serviço: campo Executor (lista de colaboradores ativos) | Valor Hora alimenta o custo de mão de obra no módulo Financeiro |
| Cadastro Básico > Unidades | Relatórios de Produtividade e Desempenho por colaborador | Acesso multihotéis controlado pela tabela de unidades no Bloco E |', NULL, NULL, NULL, NULL, 1);

-- Como Acessar o Sistema [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Como Acessar o Sistema', N'Comece por aqui', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:universal') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Todos os usuários, primeiro acesso e uso diário do PCM by SIM. | Acesso via navegador ou aplicativo mobile pcmbysim.com.br |', NULL, NULL, NULL, N'https://drive.google.com/file/d/1GBl3fTl0yIy030S79iU3I734m65Yipn_/preview', 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de acessar o PCM by SIM pelo navegador e pelo aplicativo mobile, recuperar a senha em caso de esquecimento e entender as diferencas entre os dois modos de acesso. O sistema foi projetado para funcionar em qualquer dispositivo: gestores usam o desktop para análises e configurações; técnicos e camareiras usam o app mobile para apontar tarefas em campo.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> Você precisa ter um usuário criado pelo Administrador com e-mail e senha.
> Verifique com o Administrador se o seu perfil já foi configurado com as permissões corretas.
> Para uso do aplicativo mobile, baixe o app PCM by SIM na loja do seu dispositivo.
> O endereço correto do sistema é **pcmbysim.com.br**. Não confunda com **simservices.com.br**, que é o site institucional da SIM Services (só tem um botão ''Acessar o PCM by SIM'' que leva até o sistema real).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Credenciais RECEBIDAS | → | 2️⃣ Login REALIZADO | → | 3️⃣ Senha ATUALIZADA | → | 4️⃣ Permissões CONFERIDAS | → | 📊 Sistema em Uso |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Tela de login real do PCM by SIM — campos ID-Usuário e Senha](/screenshots/login-tela-real.png)


1. Abra o navegador (Chrome recomendado) e acesse: pcmbysim.com.br
1. Informe o ID-Usuário e a Senha fornecidos pelo Administrador: geralmente o seu login será composto de "nome + último nome @ nome da unidade"
1. Clique em Login.
1. Na primeira vez, recomendamos a troca de sua senha. Crie uma senha forte (mínimo 8 caracteres, letras e números).


> [!INFO]
> **RESULTADO ESPERADO**
> Você está na tela principal do sistema. O menu lateral esquerdo da acesso a todos os módulos conforme o seu perfil de acesso.


### 4.1 Acesso pelo aplicativo mobile

1. Baixe o aplicativo PCM by SIM na loja do seu dispositivo (App Store ou Google Play).
1. Abra o app e informe o E-mail e a Senha.
1. As tarefas atribuidas ao seu usuário aparecem automaticamente na tela inicial de acordo com o seu perfil de usuário


> [!INFO]
> **IMPORTANTE SABER**
> O app mobile e otimizado para execução de tarefas em campo: apontar OS, executar checklists de preventiva e registrar limpezas de governanca.
> O desktop e otimizado para gestão: relatorios, configurações, aprovacoes e análise de indicadores.
> Ambos usam o mesmo login, a diferenca e apenas a interface.


### 4.2 Recuperar senha esquecida

Por efeitos de segurança a recuperação de Senhas só é realizada pelo Administrador do sistema em sua unidade.


> [!DANGER]
> Não há redefinição pelo próprio login. Solicite ao Administrador da sua unidade que redefina sua senha em Administração > Usuários (seção 1.3, ícone de setas circulares ''Redefinir Senha'' na listagem)..', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| ID-Usuário | Login cadastrado pelo Administrador (indicamos o primeiro + ultimo nome), usado no navegador e no app mobile | Sim | nome+ultimonome@hotel.com.br |
| Senha | Senha de acesso ao sistema | Sim | Mínimo 8 caracteres, letras e números |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Acesse o sistema sempre pelo Chrome (recomendado) para garantir compatibilidade total com todos os recursos.
> Crie uma senha forte no primeiro acesso: mínimo 8 caracteres, com letras maiúsculas, minúsculas e números.
> Ao sair do computador, use sempre o botão ''Sair'' do sistema, não feche apenas a aba do navegador.
> Para o app mobile, ative as notificações para receber alertas de tarefas atribuídas em tempo real.


> [!DANGER]
> Sessões ficam ativas até o logout manual, sempre use o botão ''Sair'' em computadores compartilhados (recepção, PCM), nunca apenas feche a aba do navegador.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Não consigo logar: ''usuário ou senha incorretos'' | Senha errada, Caps Lock ativo ou usuário inativo | Tente novamente. Use ''Esqueci minha senha'' ou contate o Administrador |
| O sistema abre mas não vejo os módulos esperados | Perfil sem permissão para os módulos necessários | Solicite ao Administrador que revise as permissões do seu perfil (seção 1.3) |
| O aplicativo mobile não sincroniza as tarefas | Sem conexão com a internet ou app desatualizado | Verifique a conectividade e atualize o app na loja |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Administracao > Usuários — credenciais criadas pelo Administrador (seção 1.3) | Todos os módulos do sistema: acesso controlado pelo perfil do usuário App mobile: execução de tarefas em campo | Sessao iniciada com registro de log de acesso para auditoria de segurança |', NULL, NULL, NULL, NULL, 1);

-- Dashboard — Visao Geral e KPIs [Governanca/Dashboard]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Governanca', N'Dashboard', N'Dashboard — Visao Geral e KPIs', N'Supervisores e Técnicos', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:tecnico') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, diretores e qualquer usuário com perfil de consulta. | Menu lateral > Dashboard (tela raiz, ''/'') pcmbysim.com.br/ |', NULL, NULL, NULL, N'https://drive.google.com/file/d/1S06qI3OJiJmSiSCg7lPCeyYTWBP30job/preview', 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de ler e interpretar todos os indicadores do Dashboard principal do PCM by SIM, o painel de controle em tempo real que mostra a saúde operacional da unidade em um único olhar. O Dashboard não e um relatorio: ele e a tela que o gestor abre ao chegar ao trabalho para saber imediatamente se o dia comecara bem ou precisara de intervencao.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> Este material abordará exclusivamente a tela do DASHBOARD Principal de Manutenção, a tela de "Desempenho de manutenção é tratada em outra seção dedicada somente a este tópico.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Apontamentos REALIZADOS | → | 2️⃣ Dados CONSOLIDADOS | → | 3️⃣ Painel ATUALIZADO | → | 4️⃣ Desvio IDENTIFICADO | → | 📊 Ação Corretiva |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |



![Dashboard raiz — Ordem de Serviço, Painel de Controle e gráficos](/screenshots/dashboard-raiz-real.png)


### 3.1  Painel Ordem de Serviço

No topo do Dashboard, um bloco só de Ordens de Serviço:

| Contador | O que mostra |
| :--- | :--- |
| Nova | Atalho para abrir uma OS nova |
| O.S. Atrasada | OS com prazo vencido, status diferente de Concluída |
| O.S. Em Aberto | OS criadas e ainda não iniciadas |
| O.S. Vinculada | OS geradas a partir de outra OS (ex.: desdobramento) |
| O.S. Em Andamento | OS com execução já iniciada |
| O.S. Concluídas | OS finalizadas no período |
| Backlog | OS represadas sem execução |


### 3.2  Painel de Controle, 6 indicadores com barra de progresso

Não são velocímetros circulares, são barras de progresso horizontais, uma por atividade, cada uma com um rótulo de status (Crítico / Em Andamento / Bom, conforme a faixa de percentual) e um link ''Ver detalhes'':

| Indicador | Vai para | Observação |
| :--- | :--- | :--- |
| Preventiva | `/PCM/ManutencaoPreventiva` | — |
| Laudo | `/PCM/ManutencaoLaudo` | Aparece no lugar de ''OS em Dia'' do conceito antigo |
| Rotinas | `/PCM/ManutencaoRotina` | — |
| PMOC | `/PMOC/PMOC2` | Aparece no lugar de ''Auditoria'' do conceito antigo |
| U.H. em Dia | `/UH/ChecklistUH` | — |
| Green Planet | `/GreenPlanet/LancamentoIndex` | — |


### 3.3  Gráficos O.S. Aberto x Concluído e Tempo Médio de Atendimento

1. O gráfico de linha ''O.S. - Aberto x Concluído'' mostra Aberto, Concluído e Saldo ao longo do período.
1. O gráfico de rosca ''Tempo Médio - Atendimento'' mostra a distribuição percentual das OS por faixa de tempo de atendimento: No Dia, 1 Dia, 3 Dias, 5 Dias, >5 Dias.


### 3.4  Principais Ocorrências

1. Use as abas Dia Anterior / Semana / Mês / Ano para trocar o período.
1. A tabela mostra os itens (Máquina/Equipamento/Itens Gerais) com mais ocorrências no período, com a Quantidade de registros, útil para identificar problemas recorrentes num mesmo item ou ambiente.


> [!INFO]
> **REFERÊNCIA COMPLEMENTAR, tela ''Desempenho - Manutenção'' (`Home/Desempenho?unidade=X`)**
> Tela diferente do Dashboard, acessada por outro item de menu. Mostra uma **Nota Geral** numérica (ex.: 42,00) com **Ranking** de posição entre unidades da rede, seguida de uma tabela ponderada **Métricas por Atividade** com 7 linhas (Laudo/Documentação, Preventiva, PMOC, Rotina, U.H. em Dia, Atend. no Dia, Green Planet), cada uma com colunas OK/Pendente/Total/% Atendido/Peso/Nota, a soma ponderada das Notas forma a Nota Geral. Uma segunda tabela de métricas de HH (HH Não Apontado, % OS Pendente, HH Extra, Prev. x Corretiva) também compõe o cálculo. Use esta tela quando precisar comparar o desempenho da sua unidade com outras da rede, o Dashboard (raiz) é para acompanhamento do dia a dia de uma única unidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| O.S. Atrasada / Em Aberto / Vinculada / Em Andamento / Concluídas / Backlog | Contadores do painel Ordem de Serviço | Auto | 8 / 0 / 1 / 4 / 34 / 0 |
| Preventiva / Laudo / Rotinas / PMOC / U.H. em Dia / Green Planet | Barras de progresso do Painel de Controle, com status Crítico/Em Andamento/Bom | Auto | 50% Crítico |
| Tempo Médio - Atendimento | % de OS por faixa de tempo (No Dia/1/3/5/>5 dias) | Auto | 66,67% No Dia |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Mantenha o Dashboard aberto em uma TV ou monitor dedicado na sala do PCM durante todo o dia, visibilidade continua cria cultura de responsabilidade na equipe.
> Use o Dashboard em reuniões de passagem de turno: o turno que sai apresenta os números para o turno que entra. A reunião dura 5 minutos e todos sabem exatamente o que está pendente.
> Compare os indicadores com o mesmo dia da semana anterior, não com o dia anterior, sazonalidade de ocupacao distorce a comparacao diária.


> [!DANGER]
> Um Dashboard com todos os indicadores em ''Bom'' todos os dias pode indicar metas muito baixas, não necessariamente excelencia operacional. Revise as metas anualmente com base no histórico.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O indicador de Preventiva está zerado mesmo com preventivas realizadas | Os apontamentos foram iniciados mas não finalizados: status diferente de Concluído | Acesse PCM > Preventiva, filtre por ''Em Andamento'' e conclua os apontamentos pendentes |
| O indicador de U.H. em Dia não está atualizando | Os checklists de vistoria não estao sendo respondidos ou o Tipo de U.H. não tem checklist vinculado | Verifique o Tipo de U.H. das unidades e confirme que o checklist ''U.H. em Dia'' está vinculado (seção 2.4) |
| O Dashboard não mostra dados de uma unidade específica | O usuário não tem permissão de acesso a essa unidade | Solicite ao Administrador que habilite o acesso a unidade no perfil do usuário (seção 1.3) |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Todos os módulos operacionais alimentam o Dashboard em tempo real: OS, Preventiva, Rotina, Laudo, PMOC, U.H., Green Planet | Reuniões de gestão: dados objetivos para decisoes | Atualizacao em tempo real: sem necessidade de refresh manual |
| — | Tela complementar ''Desempenho - Manutenção'': Ranking de unidades da rede |  |', NULL, NULL, NULL, NULL, 1);

-- Planejamento e Apontamentos de Arrumações [Governanca/Planejamento]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Governanca', N'Planejamento', N'Planejamento e Apontamentos de Arrumações', N'Governança & Camareira', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:camareira') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de Governança, recepcionistas e supervisoras de limpeza. | Menu lateral > Governança > Apontamento pcmbysim.com.br/Governanca/Apontamento |', NULL, NULL, NULL, N'https://drive.google.com/file/d/1bHzSea11eI5aSvWQxhFouWPHmeDo5BG3/preview', 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de acompanhar em tempo real o estado de limpeza de cada quarto da unidade e garantir que nenhum hóspede seja alocado num apartamento com problema. A governança começa pelo Planejamento (definir o que precisa ser arrumado antes do turno começar) e se completa no Apontamento (registrar o que foi de fato executado), as camareiras com acesso ao módulo completo de Governança recebem as tarefas direto no aplicativo mobile em ''Minhas U.H.s''; quem não tem o app completo aponta a arrumação pela tela web de Apontamento, que também serve para consultar o que está sendo feito em tempo real e para registrar arrumações retroativas.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> As U.H.s precisam estar cadastradas: seção 2.3.
> Os Tipos de U.H. com checklists de governança precisam estar configurados: seção 2.4.
> Os colaboradores de governança precisam ter acesso ao app mobile (ou perfil web) configurado em Administração > Usuários: seção 1.3.


| Status Quarto | Significado |
| :--- | :--- |
| Sujo | Quarto usado, aguarda limpeza |
| Arrumação | Camareira em atividade no quarto |
| Limpo | Limpeza concluída, aguarda vistoria |
| Inspeção | Vistoria em andamento ou aprovada |
| Manutenção | Reforma, intervenção técnica ou bloqueio por OS: quarto indisponível |

> [!INFO]
> **IMPORTANTE SABER**
> Existe também um ''Status Front Office'', separado do ''Status Quarto'', é a informação de ocupação que vem do PMS do hóspede (Opera ou outro), não um status definido pela Governança. Os dois aparecem lado a lado nos filtros do Apontamento.
> Os 5 valores acima são os oficiais do Status Quarto, não existe um status separado de ''Bloqueado''; bloqueio por problema técnico é representado pelo status ''Manutenção''.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Planejamento DEFINIDO | → | 2️⃣ Camareira NOTIFICADA | → | 3️⃣ Arrumação EXECUTADA | → | 4️⃣ Apontamento REGISTRADO | → | 5️⃣ Vistoria REALIZADA | → | 📊 U.H. Liberada |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Acompanhar e apontar arrumações em tempo real

![Governança > Apontamento — tabela de U.H.s com status em tempo real](/screenshots/governanca-apontamento-camareira-real.png)


1. Acesse Governança > Apontamento.
1. Use os filtros (Unidade, Data, Bloco, Andar, U.H., Status Front Office, Status Quarto, Tipo de Limpeza) para focar na visualização que precisa.
1. A tabela mostra cada U.H. com o Status Quarto e o Status Front Office atuais, é possível acompanhar em tempo real o que está sendo apontado pelas camareiras.
1. Use os botões da tela (Apontar arrumação, Definir tipo de limpeza, Alterar status Governança) para registrar ou corrigir um apontamento, inclusive de forma retroativa, selecionando uma data anterior.


> [!INFO]
> **IMPORTANTE SABER**
> Quem tem o plano de Governança completo faz a arrumação pelo aplicativo mobile: a camareira abre ''Minhas U.H.s'' e vê os quartos planejados para ela, com status. Quem não tem o app completo aponta diretamente por esta tela web.
> O checklist de arrumação também pode ser preenchido direto na web, pela tela Governança > Checklist, que filtra por Tipo de Checklist (Manutenção/Permanência/Saída), Status Front Office, Status Quarto e Colaborador, com 4 contadores de andamento por U.H.: Pendente, Concluído, Aguardando Liberação e Retrabalho.


### 4.2  Planejar a distribuição de limpezas antes do turno

1. Acesse Governança > Planejamento.
1. Selecione a Unidade e a Data do turno a planejar.
1. Use os filtros (Bloco, Andar, Status Front Office, Status Quarto) para localizar as U.H.s.
1. Marque as U.H.s desejadas e clique em Definir Camareira para atribuir a responsável, ou em Tipo de Arrumação para definir o tipo de limpeza previsto.
1. Use Limpar Planejamento para remover atribuições feitas por engano.


> [!INFO]
> **IMPORTANTE SABER**
> O Planejamento define o que é esperado antes do turno começar; o Apontamento (subseção 4.1) registra o que foi executado de fato, os dois se cruzam no histórico de planejamento vs. execução (subseção 4.3).


### 4.3  Consultar histórico de planejamento vs. execução

1. Acesse Governança > Planejamento, Histórico.
1. Filtre por Unidade, Tipo de Limpeza, Camareira, Bloco, Andar, U.H. ou Período.
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Lista mostrando, por U.H. planejada, se a limpeza foi executada, quantas não conformidades foram registradas e se houve vistoria, a base para comparar planejamento e execução.


### 4.4  Importar status de U.H. por planilha (unidades sem integração com o PMS)

> [!INFO]
> **IMPORTANTE SABER**
> A tela Governança > Status UH é só para unidades **sem** integração direta com o PMS (Opera ou similar), nessas unidades o status de ocupação não muda sozinho, então é preciso subir manualmente uma planilha (Arquivo/Planilha/Lista de Apartamentos) com os status vindos do PMS. O sistema lê a planilha e disponibiliza a informação na tela de Apontamento (subseção 4.1) para o planejamento da governança. Unidades com integração PMS não precisam usar esta tela.


1. Acesse Governança > Status UH.
1. Faça o upload da planilha no formato esperado (Arquivo/Planilha/Lista de Apartamentos).
1. Confira a Lista de Erros exibida após o processamento e corrija divergências antes de reenviar, se necessário.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Status Quarto | Estado de limpeza: Sujo, Arrumação, Limpo, Inspeção, Manutenção | --- | Inspeção |
| Status Front Office | Status de ocupação vindo do PMS do hóspede | --- | Ocupado / Livre |
| Camareira | Responsável pela limpeza atribuída | --- | Maria Silva |
| Tipo de Limpeza | Saída, Permanência ou Retoque | --- | Saída |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Revise o Planejamento no início de cada turno e distribua as limpezas antes das camareiras iniciarem.
> Use o filtro por Status ''Manutenção'' diariamente, nenhum quarto deve continuar bloqueado por uma OS já encerrada.
> Configure alertas para quartos em Status ''Sujo'' há mais de 2 horas sem camareira atribuída.


> [!DANGER]
> Nunca libere manualmente um bloqueio de manutenção sem confirmação do PCM de que a OS foi realmente concluída, o quarto pode voltar para venda com o problema técnico ainda ativo.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Quarto aparece disponível mas tem problema técnico | OS foi aberta mas o status de manutenção não atualizou | Verifique a OS vinculada ao quarto. Se estiver aberta, atualize o status manualmente pelo Apontamento |
| Camareira não vê o quarto no app | O quarto não foi atribuído a ela no Planejamento do turno | Acesse o Planejamento, selecione o quarto e atribua a camareira correta |
| Status de ocupação não bate com o PMS | Unidade sem integração e planilha de Status UH não foi importada no dia | Suba a planilha atualizada em Governança > Status UH (subseção 4.4) |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > U.H. e Tipo de U.H. | PCM > OS — bloqueio automático por reparo | Recepção (Opera): disponibilidade para check-in (unidades integradas) |
| Checklist de vistoria vinculado ao Tipo de U.H. | Relatório Camareira x UH e Camareira x NC (seção 5.3) | Planilha de Status UH atualiza o Apontamento (unidades sem integração) |', NULL, NULL, NULL, NULL, 1);

-- Gestão de Ordens de Serviço (OS) [OrdemServico/OrdemServicoIndex2]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'OrdemServico', N'OrdemServicoIndex2', N'Gestão de Ordens de Serviço (OS)', N'Supervisores e Técnicos', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:tecnico') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, técnicos de manutenção, recepcionistas e qualquer colaborador autorizado a abrir chamados. | Menu lateral > Ordem de Serviço pcmbysim.com.br/OrdemServico/OrdemServicoIndex2 |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção, você será capaz de registrar qualquer problema ou solicitação técnica da unidade em menos de dois minutos, acompanhar o andamento de cada atendimento em tempo real e garantir que nenhum chamado seja esquecido ou resolvido sem registro. Isso impacta diretamente na satisfação do hóspede, no controle do SLA (prazo de atendimento) e na geração de histórico de manutenção dos ativos, informação essencial para decisões de Capex e Opex.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> Você precisa estar logado com um perfil que tenha permissão para abrir OS (Técnico, Gestor de PCM ou Administrador).
> Para vincular a OS a um equipamento, ele deve estar previamente cadastrado em: Cadastro Básico > Máquinas/Equipamentos.
> Para definir a prioridade, verifique se as prioridades estão configuradas em: Cadastro Básico > Prioridade.
> Se você for vincular um colaborador executor, ele precisa estar ativo no sistema: Cadastro Básico > Colaborador.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'Uma OS percorre as seguintes etapas desde a abertura até o encerramento:

| 1️⃣ Abertura PENDENTE | → | 2️⃣ Atribuição VINCULADA | → | 3️⃣ Execução EM ANDAMENTO | → | 4️⃣ Apontamento — | → | 5️⃣ Encerramento CONCLUÍDA | → | 📊 Indicadores Dashboard |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |


> [!INFO]
> **STATUS REAIS DA OS:**
> O fluxo acima mostra as etapas principais, mas o sistema tem 7 status reais no total: PENDENTE, VINCULADA, EM ANDAMENTO, ATRASADO, CONCLUÍDO, BACKLOG e CANCELADA. ATRASADO é o que mais aparece no dia a dia (OS que passou do Prazo de Execução), a listagem tem inclusive botões dedicados Enviar para Backlog / Retornar do Backlog para OS que ainda não têm previsão de execução.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Abrir uma nova Ordem de Serviço

![Tela de listagem de OS — botão ''+ Nova OS'' no canto superior direito](/screenshots/pcm-os-listagem.png)

1. No dashboard de Manutenção, clique em "NOVA Ordem de Serviços ou acesse o menu lateral, clique em Ordem de Serviço e depois no botão azul + Nova O.S.
1. Unidade (obrigatório): selecione o hotel ou unidade onde o problema ocorreu.
1. Prioridade: selecione Crítica, Alta, Média ou Baixa. Se a prioridade for Crítica, o sistema disparará e-mails automáticos para os responsáveis cadastrados.
1. Setor / Local / U.H.: informe a localização exata do problema. Quanto mais preciso, mais rápido o técnico chegará ao local.
1. Máquina / Equipamento (obrigatório): vincule o equipamento envolvido. Isso permite ao sistema construir o histórico de manutenção do ativo.
1. Descrição: descreva o problema. Seja objetivo. Use frases como: ''Ar condicionado do quarto 302 não refrigera'' em vez de ''problema no quarto''.
1. Prazo de Execução: o campo já aparece editável nesta mesma tela, pré-preenchido com a data atual, ajuste se o prazo real for diferente do padrão.
1. Anexar Arquivo (opcional): se tiver uma foto do problema, anexe. Imagens aceleram o diagnóstico do técnico antes mesmo de ele chegar ao local.
1. Clique em Salvar.

> [!INFO]
> **RESULTADO ESPERADO**
> A OS é criada com status PENDENTE e aparece imediatamente na listagem geral.
> Se a prioridade for Crítica, o sistema dispara o e-mail de alerta para os gestores cadastrados em até 30 segundos.
> A OS recém-criada aparece no Dashboard no card ''O.S. em Aberto''.

### 4.2  Atribuir um colaborador executor

![Modal Colaborador aberto pelo botão Vincular na listagem de OS](/screenshots/pcm-os-executor.png)

1. Na listagem de OS, localize a OS desejada e clique no botão Vincular (ícone de telefone) da linha correspondente.
1. No modal Colaborador, busque e selecione quem realizará o serviço. O campo aceita mais de um colaborador, funciona como uma busca com tags, não uma seleção única.
1. Confirme para vincular.

> [!INFO]
> **RESULTADO ESPERADO**
> O status da OS muda para VINCULADA.
> O(s) técnico(s) selecionado(s) recebem a OS no aplicativo mobile e podem iniciar o atendimento. *IMPORTANTE*: Vincular uma os não faz com que ela desapareça da tela dos demais técnicos, possibilitando também que eles façam o apontamento de OS vinculadas a outros colaboradores.

> [!DANGER]
> O botão de lápis (Editar) da linha da OS NÃO atribui executor, ele edita os campos originais da abertura (Setor, Prioridade, Descrição etc.). Esta opção de edição é utilizada quando há a necessidade de coreções de informações lançadas erradas pelo solicitante. Para atribuir ou trocar o executor, use sempre o botão Vincular.

### 4.3  Apontar a execução (concluir o atendimento)

O apontamento é o registro do que foi feito. Sem ele, a OS permanece em aberto e não alimenta os indicadores de produtividade.

1. Na listagem, clique no botão Apontamento (ícone de mira) da OS que foi executada, isso abre a tela dedicada Apontamento - OS, diferente da tela de edição.
1. Categoria - Serviço: classifique o tipo de trabalho (ex.: Elétrica, Hidráulica, Climatização, 19 opções no total).
1. Tipo de Serviço: Interno ou Terceiros.
1. Tipo - Ordem de Serviço: Corretiva, Melhora ou Preventiva.
1. Colaborador: confirme quem executou (e Fornecedor, se o serviço foi terceirizado).
1. Solução: descreva com clareza o que foi realizado (ex.: ''Substituído filtro do ar condicionado modelo XYZ''). Esse é o nome real do campo, não existe campo chamado Descrição da Execução.
1. Data Início / Hora Início / Data Término / Hora Término: preencha os 4 campos de data e hora (as datas já vêm pré-preenchidas com o dia atual; as horas são digitadas com um seletor de relógio). Não existe um campo único de Tempo Gasto, o sistema calcula a duração a partir desses 4 campos.
1. Concluído?: ative para marcar o atendimento como finalizado. Se deixar desligado, preencha também a Justificativa - Apontamento (ex.: Aguardando Material, Aguardando Liberação do Local).
1. Anexar Arquivo (opcional).
1. Clique em Salvar.

> [!INFO]
> **RESULTADO ESPERADO**
> A OS recebe o status CONCLUÍDA e sai dos cards de pendências no Dashboard.
> O histórico do equipamento vinculado é atualizado com esta intervenção.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Hotel ou unidade onde o serviço ocorrerá | Sim | PCM by SIM |
| Prioridade | Nível de urgência: define o prazo automático de SLA | Sim | Crítica / Alta / Média / Baixa |
| Setor / Local / U.H. | Localização exata dentro da unidade | Sim | Quarto 302 / Lobby / Cozinha |
| Máquina/Equipamento | Ativo envolvido: vincula a OS ao histórico do equipamento | Sim | AC Split - TAG AC-302 |
| Descrição do Problema | Texto livre descrevendo o que precisa ser feito | Sim | Ar não refrigera, temperatura acima de 28°C |
| Prazo de Execução | Data/hora limite: já editável na própria tela de abertura, pré-preenchida com a data atual | Não | Ajustável na criação ou depois |
| Anexar Arquivo | Foto ou documento evidenciando o problema | Não | foto_problema.jpg |
| Executor (Colaborador) | Atribuído via botão Vincular na listagem: aceita múltiplos colaboradores | Não | João Silva - Técnico |
| Categoria - Serviço | Classificação do tipo de trabalho, usada no Apontamento | Sim | Elétrica, Hidráulica, Climatização (19 opções) |
| Tipo de Serviço | Execução interna ou por terceiros | Sim | Interno / Terceiros |
| Tipo - Ordem de Serviço | Natureza da OS | Sim | Corretiva / Melhora / Preventiva |
| Solução | Descrição de como o problema foi resolvido: nome real do campo, no Apontamento | Sim | Substituído filtro do ar condicionado |
| Data/Hora Início e Término | 4 campos que substituem o antigo Tempo Gasto: o sistema calcula a duração a partir deles | Sim | 04/07/2026 08:00 até 04/07/2026 09:30 |
| Justificativa - Apontamento | Motivo de a OS não estar concluída ainda: só aparece se Concluído? estiver desligado | Condicional | Aguardando Material |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Sempre vincule a OS a um equipamento quando o problema envolver um ativo específico. Isso constrói o ''prontuário'' do equipamento, essencial para decisões de substituição (Capex vs. Opex).
> Prefira descrições técnicas e objetivas. ''Trocado resistor do quadro elétrico QD-03'' é mais útil que ''consertado o problema''.
> Utilize o campo de anexo sempre que possível. Uma foto do ''antes'' e do ''depois'' serve como prova do serviço em auditorias.
> Mantenha o status da OS atualizado em tempo real pelo aplicativo. O Dashboard se atualiza a cada 60 segundos, a gestão depende desses dados.
> A listagem de OS também tem filtros por Departamento Responsável, Origem (Auditoria, PMOC, Preventiva, Rotina, U.H., Tarefa, Input Manual) e Justificativa-Apontamento, use-os para localizar rapidamente uma OS específica.


> [!DANGER]
> OS''s que permanecerem abertas por mais tempo que o prazo definido no momento da abertura, aparecerão em vermelho no Dashboard e nos relatórios de desempenho, impactando a nota da unidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O equipamento não aparece no campo de busca ao abrir a OS | O ativo não está cadastrado ou está inativo no sistema | Acesse Cadastro Básico > Máquinas/Equipamentos, verifique se o equipamento existe e se o switch ''Ativo'' está habilitado |
| O colaborador não aparece na lista de executores | O colaborador está inativo ou não está vinculado à unidade correta | Verifique em Cadastro Básico > Colaborador se o perfil está ativo e associado à unidade da OS |
| A OS foi criada mas não aparece no Dashboard | O filtro de unidade ou módulo do Dashboard está selecionando outra unidade | Verifique os filtros no topo do Dashboard: selecione a unidade correta ou ''Todas as Unidades'' |
| O botão ''Salvar'' não responde ao concluir o apontamento | Campo obrigatório vazio ou perda momentânea de conexão | Verifique se ''Unidade'', ''Prioridade'' e ''Descrição'' estão preenchidos. Se o problema persistir, recarregue a página (F5) e repita |
| A baixa de material não foi registrada no estoque | O campo ''Materiais Utilizados'' foi preenchido após o fechamento da OS | Abra a OS novamente em modo de edição, adicione os materiais e salve. Entre em contato com o gestor de estoque para confirmar o ajuste |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta a OS | O que a OS alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro de Equipamentos (TAG) | Dashboard (cards de status em tempo real) | Disparo de e-mail em OS Crítica |
| Cadastro de Colaboradores | Histórico do ativo (prontuário) | Atualização do SLA no Dashboard |
| Cadastro de Prioridades | Relatórios de Desempenho e BI | --- |
| Cadastro de Setores e U.H. | Módulo Excel (extração de dados) | Estoque > Saída de Material — vínculo manual pelo nº da OS, sem baixa automática nem cálculo de custo |', NULL, NULL, NULL, NULL, 1);

-- Gestão de Perfil e Troca de Senha [Administracao/PerfilIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Administracao', N'PerfilIndex', N'Gestão de Perfil e Troca de Senha', N'Comece por aqui', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:universal') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Todos os usuários ativos do sistema: técnicos, camareiras, gestores e administradores. | Barra superior direita > ícone de Perfil (ao lado do botão SAIR) pcmbysim.com.br, painel lateral deslizante, disponível em qualquer tela |', NULL, NULL, NULL, N'https://drive.google.com/file/d/1GBl3fTl0yIy030S79iU3I734m65Yipn_/preview', 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao finalizar esta seção você saberá atualizar seu nome de exibição e redefinir sua senha de acesso de forma segura, sem precisar sair do que está fazendo no sistema. Manter a senha atualizada é essencial porque todo apontamento de OS, preventiva, vistoria ou auditoria fica registrado com a sua identificação. Proteger o seu acesso é proteger a integridade dos registros operacionais da unidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> Você precisa estar logado no sistema com seu usuário e senha atuais.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Perfil ABERTO | → | 2️⃣ Nome ATUALIZADO | → | 3️⃣ Nova Senha DIGITADA | → | 4️⃣ Senha CONFIRMADA | → | 📊 Alterações Salvas |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Abrir o painel de perfil

> [!INFO]
> **Barra superior do sistema no lado direito, ícone de Perfil ao lado do botão SAIR**
> 


1. Em qualquer tela do sistema, localize a barra de ferramentas no canto superior direito.
1. Clique no ícone de Painel de Perfil, botão circular cinza com ícone de lista, ao lado do botão SAIR.
1. Uma aba lateral deslizará da direita para a esquerda exibindo seus dados cadastrais e os campos de segurança.


> [!INFO]
> **RESULTADO ESPERADO**
> O painel abre sobre a tela atual, você não perde o trabalho em andamento.


### 4.2  Atualizar nome e redefinir senha

![Painel lateral de Perfil — campos Nome, ID-Usuário (bloqueado), Senha e Confirmar Senha](/screenshots/painel-perfil-real.png)


1. No campo Nome, corrija ou atualize seu nome de exibição se necessário. Este nome aparece nas OS impressas e nos relatórios.
1. O campo ID-Usuário aparece em cinza e não pode ser editado. É o seu login oficial no sistema.
1. No campo Senha, digite sua nova chave de acesso respeitando os critérios abaixo:


Mínimo de 8 caracteres.

Combine letras e números obrigatoriamente.

Evite sequências óbvias como ''12345678'', ''senha123'' ou sua data de nascimento.

1. No campo Confirmar Senha, repita exatamente a mesma senha do passo anterior.
1. Clique em Salvar na base do painel lateral.


> [!INFO]
> **RESULTADO ESPERADO**
> O sistema exibe um pop-up de confirmação, clique em OK para fechar.
> A nova senha já está ativa imediatamente para o próximo login.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Nome | Nome de exibição na plataforma, nos documentos impressos e relatórios | Não | João Silva |
| ID-Usuário | Login oficial, gerado pelo Administrador no cadastro | — | joao.silva@pcmbysim (somente leitura) |
| Senha | Nova chave de acesso: mínimo 8 caracteres, letras + números | Sim | Exemplo: Ab123456 |
| Confirmar Senha | Repetição da nova senha para válidação pelo sistema | Sim | Deve ser idêntica ao campo Senha |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Atualize sua senha a cada 90 dias. O sistema não exige, mas é a frequência recomendada para evitar acessos não autorizados.
> Nunca compartilhe suas credenciais. Cada apontamento de OS, preventiva ou vistoria fica registrado com o seu ID-Usuário. O compartilhamento compromete a rastreabilidade e a responsabilidade legal dos registros.
> Use uma senha diferente para cada sistema. Reutilizar senhas aumenta o risco caso outro serviço sofra um vazamento.


> [!DANGER]
> Se você esquecer sua senha e não conseguir fazer login, contate o Administrador do sistema. Não há link de recuperação automática na tela de login.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Pop-up de confirmação não aparece após clicar em Salvar | Senhas não coincidem ou não atendem ao critério mínimo | Verifique se as duas senhas são idênticas e têm ao menos 8 caracteres com letras e números. Confirme se o Caps Lock está desativado |
| Botão Salvar não responde | Perda momentânea de conexão com o servidor | Verifique sua conexão, pressione F5 para recarregar a página e repita o processo desde o início |
| Painel lateral fecha sozinho antes de salvar | Atualização automática do Dashboard (60 segundos) | Abra o painel novamente e salve imediatamente após preencher os campos, não deixe o painel aberto sem ação |
| Não consigo alterar o ID-Usuário (login) | Campo de somente leitura por motivos de segurança e auditoria | Solicite a alteração ao Administrador do sistema, este é o único perfil com acesso a este campo |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Administração > Usuários (cadastro inicial do login) | Rastreabilidade em OS, Preventivas, Rotinas e Auditorias | Toda ação no sistema é gravada com o ID-Usuário ativo no momento |
| Perfil e Hierarquia (seção 1.4) | Nível de visibilidade de dados e fluxo de aprovações | Senha incorreta bloqueia acesso a todos os módulos vinculados ao perfil |', NULL, NULL, NULL, NULL, 1);

-- Novidades em Agosto/26 [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Novidades em Agosto/26', N'Novidades na Plataforma', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:novidades') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta página? | Atualizado em |
| :--- | :--- |
| Todos os usuários do PCM by SIM. | Agosto/2026 |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que mudou este mês', N'> [!NOVIDADE Discrepâncias — Apontamento Avançado]
> Mais uma melhoria que busca eliminar a dependência do preenchimento em papel por camareiras, supervisoras e governantas, agora direto no sistema, em tempo real.


| Módulo | Perfil afetado | Onde acessar |
| :--- | :--- | :--- |
| Governança | Camareiras, Supervisoras, Governantas, Gestores | Governança > Discrepâncias |


![Governança > Discrepâncias — grade cruzando planejamento e execução por U.H.](/screenshots/governanca-discrepancias.png)


**O que mudou:** Nova tela de apontamento cruza em uma única grade o planejamento de limpeza (Front Office) com a execução real, mostrando de cara divergências entre o que estava programado e o que aconteceu: por U.H., por camareira e por bloco/andar.

**Por que isso importa:** Esse controle era feito historicamente em planilhas e relatórios de papel preenchidos manualmente por camareiras, supervisoras e governantas, um processo lento e sujeito a erro de transcrição. Agora o apontamento e o cruzamento de divergências acontecem direto no sistema, com 9 indicadores calculados automaticamente (Total Planejado, Total Arrumado, % Vistorias etc.) e filtro por Bloco/Andar/Status/Tipo de limpeza.

**Como usar:**
1. Acesse Governança > Discrepâncias.
1. Filtre por Data, Bloco, Andar, Status ou Tipo de limpeza.
1. Use Apontar Limpeza Realizada ou Apontar Divergência para registrar diretamente na grade.

📖 Manual completo: seção 5.10 (e 5.11 para o relatório histórico)


---


> [!NOVIDADE Tudo em Dia — Checklist de Conformidade de Locais]
> É a evolução do U.H. em Dia, agora todos os locais da edificação que não são U.H. (áreas comuns, salas técnicas, academia, SPA, restaurantes) também ganham um checklist de conformidade com periodicidade própria.


| Módulo | Perfil afetado | Onde acessar |
| :--- | :--- | :--- |
| Qualidade / Manutenção | Técnicos de manutenção, supervisores de PCM | Tudo em Dia > Checklist |


![Tudo em Dia > Checklist — KPIs de status e locais agrupados por setor/andar](/screenshots/tudoemdia-checklist-locais.png)


**O que mudou:** O U.H. em Dia sempre controlou a conformidade dos apartamentos. Agora o Tudo em Dia estende essa mesma lógica para o resto do prédio: cada local (SPA, Academia, Restaurante, salas de evento etc.) tem sua própria periodicidade de vistoria, com 5 status automáticos (Atrasado, Pendente, Em Manutenção, Nova Vistoria, Em Dia).

**Por que isso importa:** Antes, garantir que áreas comuns e técnicas estivessem sempre em ordem dependia de controle manual ou memória da equipe. Agora, quando a periodicidade programada vence, o sistema sinaliza automaticamente que é hora de vistoriar aquele local. Se algo for encontrado fora de conformidade, já é possível abrir a Ordem de Serviço de correção direto dali.

**Como usar:**
1. Acesse Tudo em Dia > Checklist e confira os locais Atrasados/Pendentes.
1. Clique no local para abrir a vistoria e responder o checklist.
1. Itens Não Conformes podem gerar OS automaticamente, conforme configurado no checklist.

📖 Manual completo: seção 5.13 (e seção 2.13 para como configurar o checklist)


---


> [!NOVIDADE Ativo Patrimonial — Novo Módulo]
> Deixou de ser só uma melhoria e virou um módulo inteiro novo: controle completo do patrimônio da rede, do cadastro à baixa, incluindo registro fotográfico para acompanhar o estado de conservação de cada bem ao longo do tempo.


| Módulo | Perfil afetado | Onde acessar |
| :--- | :--- | :--- |
| Ativo Patrimonial (menu real: Ativo Fixo) | Administradores, gestores de patrimônio, equipe de campo | Ativo Fixo > Ativo Fixo / Movimentação / Gestão de Inventário / Inventário |


![Ativo Fixo > Ativo Fixo — listagem com filtros de Unidade, Código, Descrição, Status e Localização](/screenshots/ativo-fixo-listagem.png)


**O que mudou:** Nasceu para atender um cliente que precisava controlar os ativos patrimoniais de toda a rede de hotéis, hoje é um módulo completo com 5 capacidades: listagem de todos os ativos, inventário com movimentação automática, controle de baixas (arquivando o histórico do bem), cadastro de ativos direto durante o inventário, e registro fotográfico Hand-on/Hand-off a cada contagem.

**Por que isso importa:** Antes, cada bem do hotel (móveis, eletrodomésticos, equipamentos) não tinha um controle patrimonial centralizado nem uma forma objetiva de acompanhar seu estado de conservação. Agora cada ativo tem etiqueta, localização, responsável e histórico de fotos, dá para saber não só onde cada bem está, mas se ele está se deteriorando, direto pela comparação entre a foto do primeiro inventário e das contagens seguintes.

**Como usar:**
1. Cadastre os ativos em Ativo Fixo > Ativo Fixo (etiqueta, localização, responsável).
1. Abra uma campanha em Gestão de Inventário e conte os ativos em Inventário. Ativos sem cadastro podem ser cadastrados na hora.
1. A cada contagem, registre a foto do ativo para acompanhar sua conservação ao longo do tempo.


📖 Manual completo: seções 8.1 a 8.5


---


> [!NOVIDADE Metas e Parâmetros de Governança]
> Agora é possível parametrizar, unidade por unidade se necessário, os mesmos pesos que definem o Score da unidade e a Nota 10 da camareira, antes esses critérios não tinham uma tela própria de configuração.


| Módulo | Perfil afetado | Onde acessar |
| :--- | :--- | :--- |
| Administração / Governança | Administradores e gestores de Governança | Administração > Metas de Governança |


![Administração > Metas e Parâmetros de Governança — pesos, metas padrão e tabela por unidade](/screenshots/metas-parametros-governanca.png)


**O que mudou:** Nova tela central para configurar as réguas do Desempenho de Governança: meta de atendimento padrão, meta de produtividade por camareira, meta de % de vistoria, e os pesos que compõem tanto o Score da unidade (NC, Vistoria, Produtividade, Retrabalho) quanto a Nota da camareira no ranking Governança Nota 10 (Produtividade, NC, Retrabalho). Cada unidade pode ter uma meta própria que sobrescreve o padrão da rede.

**Por que isso importa:** Antes, os critérios que definem se uma unidade ou uma camareira está indo bem no ranking de Governança não tinham um lugar único e visível para ajuste, agora dá para calibrar o peso de cada indicador conforme a realidade da rede (ou de uma unidade específica) e ver imediatamente o reflexo no Score e na Nota (seção 7.10).

**Como usar:**
1. Acesse Administração > Metas de Governança.
1. Ajuste os Parâmetros Gerais da Empresa (metas e pesos padrão) e salve.
1. Se alguma unidade precisar de meta diferente do padrão, preencha a linha dela em Metas por Unidade.

📖 Manual completo: seção 1.6 (configuração) e seção 7.10 (como o Score e a Nota são calculados e exibidos)', NULL, NULL, NULL, NULL, 1);

-- Discrepâncias - Apontamento Avançado [Governanca/Apontamento]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Governanca', N'Apontamento', N'Discrepâncias - Apontamento Avançado', N'Governança & Camareira', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:camareira') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Governantas, supervisoras e gestores de governança. | Menu lateral > Governança > Discrepâncias pcmbysim.com.br/Governanca/Discrepancias |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de acompanhar o apontamento de limpeza com muito mais detalhe do que a tela clássica de Apontamento (seção 5.1): aqui cada U.H. tem um roteiro rico de status, um campo dedicado para registrar divergências entre o planejado e o executado, e o controle de itens de bagagem deixados no quarto. É a tela mais completa do sistema para a governança do dia a dia.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> As U.H.s precisam estar cadastradas (seção 2.3) e o Planejamento do turno feito (seção 5.1, subseção 4.2).


> [!INFO]
> **DIFERENÇA EM RELAÇÃO AO APONTAMENTO CLÁSSICO**
> Esta tela é mais rica que o Apontamento clássico (seção 5.1): lá o Status Quarto tem 5 valores; aqui o campo equivalente (Stat. UH) tem 11 valores, incluindo situações de hospedagem (Não Perturbe, N.Q.A, Não Dormiu, Mau Cheiro, Late Check-out) que o Apontamento clássico não cobre. Ambas as telas parecem coexistir no sistema, use esta seção quando precisar do nível de detalhe completo.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Planejamento do TURNO | → | 2️⃣ Limpeza EXECUTADA | → | 3️⃣ Apontamento DETALHADO | → | 4️⃣ Divergência IDENTIFICADA | → | 📊 Ação Corretiva |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Governança > Discrepâncias — painel de indicadores e grade detalhada por U.H.](/screenshots/governanca-discrepancias.png)


### 4.1  Consultar o painel de indicadores

1. Acesse Governança > Discrepâncias.
1. O topo mostra 9 indicadores do dia: Total Planejado, Total Arrumado, Total Permanência, Total Saída, Não Perturbe / N.Q.A, Saídas por Camareira, Permanências por Camareira, Saídas Vistoriadas e % Vistorias Saídas.
1. Filtre por Data, Bloco, Andar, U.H., Status Front Office, Status Quarto ou Tipo de Limpeza para focar a visualização.


### 4.2  Apontar limpeza realizada ou uma divergência

1. Na grade, localize a U.H. desejada.
1. Use os botões Apontar Limpeza Realizada ou Apontar Divergência conforme o caso; Excluir Apontamento remove um registro feito por engano.
1. Preencha o Stat. UH da linha, as 11 opções reais são: Limpo, N.Q.A, Não Perturbe, Manutenção, Não Dormiu, Sujo, Arrumação, Inspeção, Mau Cheiro, LP Mais Tarde, Late Check-out.
1. Confira o Stat. Gov (OK / N/OK), indica se a governança considera aquele apontamento conforme.
1. Se houver divergência, preencha o campo numérico AD/CR1/CR2 e a Observação com o detalhe do ocorrido.
1. Se houver bagagem do hóspede deixada no quarto, marque o tamanho em Bagagem (P/M/G).


> [!INFO]
> **CAMPO AD/CR1/CR2**
> Este campo aceita 3 números separados (ex.: ''2 / 3 / 4''), prováveis códigos de consumo de frigobar ou itens de amenities.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Status Front Office | Ocupação vinda do PMS | Auto | OCUPADO / VAGO |
| Stat. UH | Status detalhado da limpeza/hospedagem | Sim | 11 valores: ver passo 4.2 |
| Stat. Gov | Avaliação da governança sobre o apontamento | Sim | OK / N/OK |
| Divergência (AD/CR1/CR2) | Códigos numéricos: significado a confirmar | Não | 2 / 3 / 4 |
| Bagagem | Tamanho da bagagem deixada no quarto | Não | P / M / G |
| Observação | Texto livre sobre o ocorrido | Não | ''Cheiro de cigarro'' |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Use o Stat. UH mais específico possível (ex.: ''Mau Cheiro'' em vez de só ''Sujo''), quanto mais preciso o apontamento, mais acionável fica o Relatório de Discrepâncias (seção 5.11).
> Sempre preencha a Observação quando marcar Stat. Gov como N/OK, o texto livre é o que dá contexto para quem for revisar depois.


> [!DANGER]
> Não confunda esta tela com o Apontamento clássico (seção 5.1), os dois têm propósitos parecidos, mas conjuntos de status diferentes. Alinhe com a equipe qual das duas telas é o padrão oficial da sua unidade para evitar apontamento duplicado.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Os indicadores do topo não batem com o esperado | Filtro de Data não está no dia correto | Confirme o filtro Data no topo da tela |
| Não sei se devo usar esta tela ou o Apontamento clássico (5.1) | As duas coexistem no sistema | Confirme com o gestor de governança qual é o fluxo padrão adotado pela unidade |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Governança > Planejamento (seção 5.1) / Integração PMS — Status Front Office | Relatório de Discrepâncias (seção 5.11) | Indicadores do topo recalculados a cada apontamento |', NULL, NULL, NULL, NULL, 1);

-- Rotinas e Rondas [CadastroBasico/RotinaIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'RotinaIndex', N'Rotinas e Rondas', N'Supervisores e Técnicos', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:tecnico') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Técnicos de manutenção, supervisores de turno e gestores de PCM. | Menu lateral > PCM > Rotina pcmbysim.com.br/PCM/ManutencaoRotina |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao finalizar esta seção você saberá programar, executar e registrar as verificações rápidas e rondas operacionais da unidade. As rotinas são o ''pulso'' da manutenção: enquanto a preventiva cuida dos equipamentos em intervalos maiores, a rotina garante que alguém passa os olhos nos pontos críticos todo dia. Isso permite detectar problemas antes que se tornem falhas graves e comprova documentalmente para auditorias e seguros que a unidade realiza inspeção contínua.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> O plano de rotina precisa estar cadastrado: Cadastro Básico > Rotina.
> O checklist de verificação rápida precisa estar criado e vinculado ao plano: Cadastro Básico > Checklist.
> O técnico ou supervisor executor precisa estar ativo: Cadastro Básico > Colaborador.
> Para rotinas de turno funcionarem corretamente, os horários de execução precisam estar configurados no plano, o sistema só gera o alerta no momento correto.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'Ciclo de vida de uma rotina no sistema:

| 1️⃣ Plano CADASTRADO | → | 2️⃣ Horário CONFIGURADO | → | 3️⃣ Execução EM ANDAMENTO | → | 4️⃣ Checklist CONCLUÍDO | → | 5️⃣ Ocorrência? OS | → | 📊 Histórico Dashboard |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |

Entendendo a diferença entre Preventiva e Rotina antes de executar:

| Dimensão | Preventiva | Rotina / Ronda |
| :--- | :--- | :--- |
| Foco | No ativo (máquina, equipamento) | Na operação (ronda, verificação de estado) |
| Periodicidade | Semanal, mensal, trimestral, semestral | Diária, por turno, a cada 4 ou 8 horas |
| Duração | 30 minutos a várias horas | Varia muito: de 5 minutos a mais de 1 hora, dependendo do plano |
| Checklist | Técnico profundo: medições, trocas de peças | Também pode ser técnico e extenso: um plano real testado tinha 50+ itens em 11 categorias, com leituras numéricas (pH, temperatura, cloro) |
| Exemplo | Troca de filtros do ar condicionado (mensal) | Conferir se o gerador está em modo automático (diário) |
| Impacto legal | PMOC, NR-13, laudos técnicos | Auditoria de qualidade, seguros, registro de ronda |

> [!WARNING]
> **A DIFERENÇA REAL ESTÁ MAIS NA FREQUÊNCIA DO QUE NA PROFUNDIDADE**
> A tabela acima é uma referência conceitual. Na prática, uma Rotina pode ser tão ou mais extensa que uma Preventiva, o que diferencia mesmo os dois módulos é a frequência (diária/por turno na Rotina vs. semanal ou mais espaçada na Preventiva), não necessariamente a duração ou profundidade do checklist.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Visualizar as rotinas do dia e do turno

![Tela PCM > Rotina — cards de status e listagem das rotinas do dia](/screenshots/rotina-listagem.png)


1. Acesse o menu lateral, clique em PCM e depois em Rotina.
1. Leia os cards de status no topo: PENDENTE, EM ANDAMENTO, ATRASADO e CONCLUÍDO. O card ATRASADO exige atenção imediata.
1. Use os filtros Unidade, Tipo de Rotina e Responsável para visualizar apenas as rondas do seu turno.
1. Para planejar a semana, clique em Visualizar por Mês e verifique os dias e turnos com maior concentração de verificações.


> [!INFO]
> **RESULTADO ESPERADO**
> Você tem a visão das rondas do seu turno e sabe quais estão atrasadas antes de iniciar o trabalho.


### 4.2  Executar o apontamento de uma rotina

![Tela de apontamento de rotina — checklist de verificação rápida com itens sim/não](/screenshots/rotina-apontamento.png)


1. Na listagem, localize a rotina a ser executada e clique no ícone de Iniciar / Apontar.
1. O sistema abre o checklist de verificação rápida. Percorra cada item e selecione:


SIM, o ponto está em ordem.

NÃO, o ponto apresenta desvio, falha ou anomalia.

N/A, o item não é relevante neste turno ou área. Nomenclatura real confirmada em teste: SIM/NÃO/N/A, não Conforme/Não Conforme/Não Aplicável.

1. Para itens Não Conformes, descreva a ocorrência no campo de Observações com o máximo de detalhes possível.
1. Se encontrar algo que exige reparo imediato, use a opção Gerar OS diretamente do apontamento.
1. Ao responder todos os itens, clique em Concluir.


> [!INFO]
> **RESULTADO ESPERADO**
> A rotina recebe o status CONCLUÍDO com o horário exato de encerramento registrado.
> A nota de ''Rotina'' na tabela de Métricas por Atividade do Dashboard (seção 7.1) é atualizada imediatamente.
> Se uma OS foi gerada, ela já aparece na fila de pendências do gestor.


### 4.3  Registrar uma ocorrência e gerar OS a partir da ronda

Durante a ronda, o técnico encontra algo fora do padrão, uma luminária apagada, água empoçada no corredor, um ruído incomum no gerador. O fluxo correto é:

1. Marque o item como Não Conforme e descreva a ocorrência nas Observações.
1. Decida se a ocorrência pode ser resolvida na hora pelo próprio técnico de ronda ou se exige outro recurso:


Resolução imediata: registre o que foi feito nas observações e marque como Concluído.

Exige recurso adicional: clique em Gerar OS, defina a prioridade e salve.

1. Conclua a rotina normalmente, mesmo com itens Não Conformes, a ronda é registrada como executada.


> [!DANGER]
> Nunca deixe de registrar uma ocorrência por achar que é ''pequena demais''. Um vazamento lento não registrado pode se tornar um dano estrutural semanas depois, e sem registro, não há prova de que a equipe estava fazendo as rondas.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Rotina | Nome do plano de rotina: herdado do Cadastro Básico | Auto | Ronda Noturna: Bloco A |
| Data / Turno | Data e turno de execução | Auto | 27/05/2025: Turno Noite |
| Técnico | Colaborador que realiza a ronda | Sim | Carlos Oliveira |
| Categoria - Serviço | Classificação do tipo de trabalho, presente também no apontamento de rotina | Sim | Climatização, Elétrica... |
| Tipo de Serviço | Execução interna ou por terceiros | Sim | Interno / Terceiros |
| Tipo - Ordem de Serviço | Natureza do apontamento | Sim | Rotina |
| Item do Checklist | Ponto de verificação da ronda | Sim | SIM / NÃO / N/A |
| Observação | Descrição detalhada de qualquer desvio encontrado | Cond. | Lâmpada queimada no corredor do 3° andar |
| Gerar OS | Atalho para abrir OS corretiva vinculada à ocorrência encontrada | Não | Acionar quando o problema exige reparo |
| Foto / Anexo | Evidência visual da ocorrência: essencial para OS geradas na ronda | Não | foto_lampada_corredor.jpg |


Tipos de rotina mais comuns em unidades hoteleiras:

| Tipo de rotina | Frequência típica | O que verificar |
| :--- | :--- | :--- |
| Ronda de Turno | Por turno (3x/dia) | Iluminação, temperatura de ambientes, equipamentos ligados/desligados |
| Ronda Noturna | Diária (madrugada) | Segurança patrimonial, vazamentos, gerador em standby, alarmes |
| Verificação de Medidores | Diária (manhã) | Leituras de hidrômetro, medidor de energia, manômetros |
| Inspeção de Áreas Comuns | Diária | Estado de conservação, limpeza, sinalização, extintores |
| Teste de Emergência | Semanal | Iluminação de emergência, hidrantes, detectores de fumaça |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Execute as rondas sempre no mesmo percurso e ordem. Isso cria um padrão mental que facilita a detecção de anomalias, quando algo está diferente do habitual, o técnico nota mais rápido.
> Use o aplicativo mobile para fazer o apontamento enquanto caminha, não aguarde chegar à sala para registrar. Detalhes de uma ocorrência se perdem com o tempo.
> Ao gerar uma OS pela ronda, atribua já no momento um técnico responsável. Uma OS sem executor fica invisível na fila de gestão e pode ser esquecida.
> Revise o card ATRASADO no início de cada turno. Rondas atrasadas de turnos anteriores precisam de explicação, ou foram executadas e não apontadas, ou não foram feitas.


> [!DANGER]
> Rondas não executadas ficam com status ATRASADO e impactam a nota de ''Rotina'' na tabela de Métricas por Atividade do Dashboard. Em auditorias de qualidade, a ausência de registros de ronda é tratada como falha grave de processo.
> Não registre uma ronda como Concluída sem tê-la feito fisicamente, o registro falso pode comprometer a validade de laudos técnicos e seguros em caso de sinistro.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A rotina não aparece no horário esperado na listagem | O horário de execução não foi configurado no plano, ou o fuso horário do servidor está diferente | Acesse Cadastro Básico > Rotina, abra o plano e verifique o campo de horário/turno. Contate o suporte se o fuso horário do sistema estiver incorreto |
| O técnico não consegue iniciar o apontamento pelo app mobile | O usuário não está vinculado ao módulo PCM, ou o switch ''Acesso via Aplicativo'' está inativo | Verifique em Administração > Usuários se o módulo PCM está habilitado e se o switch de app está ativo para o colaborador |
| O card ATRASADO mostra rotinas que já foram executadas | O apontamento foi feito no papel ou verbalmente, mas não foi registrado no sistema | Registre o apontamento retroativamente informando a data e hora real da execução. Oriente a equipe sobre a importância do registro em tempo real |
| A OS gerada pela rotina não aparece vinculada ao histórico da ronda | A OS foi criada manualmente depois, sem usar o atalho ''Gerar OS'' dentro do apontamento | Para futuras ocorrências, sempre use o botão ''Gerar OS'' dentro da tela de apontamento da rotina para manter o vínculo automático |
| A nota de Rotina não atualiza após concluir o apontamento | O Dashboard tem cache de 60 segundos: a atualização é automática | Aguarde até 60 segundos para a tela do Dashboard se atualizar. Não é necessário recarregar manualmente |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Rotina (plano)Cadastro Básico > ChecklistCadastro Básico > Colaborador | Dashboard: nota de ''Rotina'' na tabela de pontuação (seção 7.1)Histórico de Rotina (PCM > Histórico) | Geração automática de OS ao registrar ocorrênciaAtualização imediata da nota no Dashboard |
| Cadastro Básico > Setores e UnidadesRegistro de Faltas (seção 3.9) | Relatório Mensal PCM: cumprimento de rondasMódulo de Laudos: evidência de inspeção contínua | Se técnico registrado em falta, a rotina é redistribuída para outro executor disponível |', NULL, NULL, NULL, NULL, 1);

-- Treinamento — Biblioteca de Materiais [CadastroBasico/TreinamentoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'TreinamentoIndex', N'Treinamento — Biblioteca de Materiais', N'Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Cadastro: Administradores e gestores de cada módulo. Consulta: todo colaborador com acesso ao sistema. | Cadastro: Cadastro Básico > Treinamento — Consulta: Menu lateral > Treinamento pcmbysim.com.br/Treinamento/TreinamentoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de disponibilizar materiais de treinamento (manuais, POPs, vídeos) para a equipe consultar dentro do próprio sistema, organizados por módulo. É uma funcionalidade simples e direta: um repositório de documentos, sem controle de leitura obrigatória ou avaliação, o valor está em manter o material sempre acessível, sem depender de e-mail ou pasta compartilhada externa.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!INFO]
> **DUAS TELAS, DOIS PÚBLICOS**
> Cadastro Básico > Treinamento é onde o material é enviado (Administrador/gestor). Menu > Treinamento é onde qualquer colaborador consulta o que foi publicado. Sem nenhum material cadastrado na primeira, a segunda fica vazia.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Material PREPARADO | → | 2️⃣ Módulo DEFINIDO | → | 3️⃣ Arquivo ENVIADO | → | 📊 Consulta por Unidade/Módulo |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Cadastro Básico > Treinamento — formulário com Unidade, Módulo, Descrição, Comentários e Arquivo](/screenshots/treinamento-cadastro.png)


### 4.1  Cadastrar um novo material de treinamento

1. Acesse Cadastro Básico > Treinamento e clique em Novo.
1. Selecione a Unidade e o Módulo: GOVERNANÇA, MANUTENÇÃO ou QUALIDADE.
1. Preencha a Descrição e, se necessário, Comentários adicionais sobre o material.
1. Mantenha Ativo habilitado para que o material fique disponível para consulta.
1. Faça o upload do Arquivo (documento, POP ou material de apoio).
1. Clique em Salvar.


### 4.2  Consultar materiais de treinamento disponíveis

1. Acesse o menu lateral > Treinamento.
1. Selecione a Unidade e o Módulo desejado: GOVERNANÇA, MANUTENÇÃO ou QUALIDADE.
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Lista dos materiais ativos cadastrados para aquele Módulo e Unidade, disponíveis para abertura/download.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Módulo | Área a que o material se refere | Sim | GOVERNANÇA / MANUTENÇÃO / QUALIDADE |
| Descrição | Título/identificação do material | Sim | POP: Limpeza de Piscina |
| Comentários | Notas adicionais sobre o conteúdo | Não | — |
| Ativo | Define se o material aparece na consulta | Sim | Ativo |
| Arquivo | Documento enviado | Sim | PDF, DOCX ou similar |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Use a Descrição de forma padronizada (ex.: ''POP, [nome do processo]'') para facilitar a busca visual na lista.
> Revise periodicamente os materiais ativos e inative os desatualizados, um POP antigo disponível junto com o atual gera confusão.


> [!DANGER]
> Esta tela não registra se o colaborador efetivamente abriu ou leu o material, não é uma ferramenta de controle de treinamento obrigatório. Para exigência formal de leitura com registro, mantenha um controle complementar fora do sistema.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Um material não aparece na consulta do colaborador | Material inativo, ou Unidade/Módulo filtrados não correspondem ao cadastro | Verifique o cadastro em Cadastro Básico > Treinamento e confirme Unidade, Módulo e o switch Ativo |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| — | Consulta de material por qualquer colaborador com acesso ao sistema | Material fica disponível na consulta imediatamente após o cadastro (se Ativo) |', NULL, NULL, NULL, NULL, 1);

-- Lavanderia - Apontamento e Controle de Lavagem [Lavanderia/RelatorioControleGeral]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Lavanderia', N'RelatorioControleGeral', N'Lavanderia - Apontamento e Controle de Lavagem', N'Governança & Camareira', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:camareira') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Governantas, supervisoras de lavanderia e gestores de custos operacionais. | Menu lateral > Lavanderia > Apontamento pcmbysim.com.br/Lavanderia/Apontamento |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar a operação física de lavagem do dia, quanto peso foi lavado, em quais máquinas, com qual equipe, e acompanhar a produtividade da lavanderia ao longo do tempo. Este módulo é diferente da Gestão de Enxoval (seção 5.2): lá você controla **quantas peças** existem, entram e saem do estoque; aqui você controla **o processo de lavagem em si**, peso processado, uso de máquinas e mão de obra, a base para calcular custo e eficiência da lavanderia.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'| Tela | URL | Função principal |
| :--- | :--- | :--- |
| Apontamento | Lavanderia/Apontamento | Registro diário da lavagem: peso, peso relave, máquinas usadas e equipe |
| Histórico | Lavanderia/Historico | Lista de todos os apontamentos já registrados, com filtros |
| Controle de Lavagem | Lavanderia/RelatorioControleGeral | Relatório mensal de produtividade, agrupável por Cliente/Máquina/Funcionário |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Carga PESADA | → | 2️⃣ Lavagem REALIZADA | → | 3️⃣ Apontamento REGISTRADO | → | 4️⃣ Peso Relave CONFERIDO | → | 📊 Histórico Atualizado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Lavanderia > Apontamento — campos de peso, peso relave, máquinas e grade de enxoval](/screenshots/lavanderia-apontamento.png)


### 4.1  Registrar o apontamento diário de lavagem

1. Acesse Lavanderia > Apontamento.
1. Selecione a Unidade e, se aplicável, o Cliente.
1. Informe a Data, o Peso total lavado (kg) e o Peso Relave (kg de roupa que precisou de uma segunda lavagem por não ter ficado limpa na primeira).
1. Na grade Máquina de Lavar, informe a Qtde. Maquinadas (quantidade de ciclos/cargas) de cada máquina usada no dia.
1. Na grade Enxoval, informe a Quantidade e a Quantidade Relave de cada tipo de peça processada.
1. Na grade Tipo de Funcionário, informe a Quantidade de colaboradores de cada função que trabalharam na lavanderia naquele turno.
1. Clique em Salvar.


> [!INFO]
> **IMPORTANTE SABER**
> O Peso Relave é o principal indicador de qualidade do processo, quanto maior o percentual de relave sobre o total lavado, pior a eficiência da lavagem (produto de limpeza incorreto, máquina com defeito, ou sobrecarga de peso por ciclo). Acompanhe a coluna % Relave no relatório de Controle de Lavagem (subseção 4.3).


### 4.2  Consultar o histórico de apontamentos

1. Acesse Lavanderia > Histórico.
1. Filtre por Unidade, Cliente, Colaborador ou Período (Data Início e Fim).
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Lista com Data, Cliente, Colaborador, Hora Início, Hora Término, Tempo Gasto, Peso, Peso Relave e Peso Total de cada apontamento registrado.


> [!WARNING]
> **ATENÇÃO, filtro compartilhado**
> O filtro ''Máquina / Equipamento / Itens Gerais'' desta tela reutiliza a mesma lista combinada de equipamentos com TAG e Itens Gerais já reportada como bug em outras telas do sistema (ver seção 2.17), não é uma lista específica de máquinas de lavar. Ignore esse filtro se a lista não fizer sentido para o seu caso de uso.


### 4.3  Consultar o relatório de Controle de Lavagem

1. Acesse Lavanderia > Controle de Lavagem (dentro do menu Relatório).
1. Selecione a Unidade, o Cliente (opcional), o Mês de análise e o Colaborador (opcional).
1. Escolha como Agrupar Por: Cliente, Família de Máquinas, Funcionário, Máquina ou Total.
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Tabela por dia do mês com Kg Lavagem, Kg Relave, Total, % Relave, Kgs/H.H. (produtividade por hora trabalhada) e Maquinadas (número de ciclos), os dois indicadores-chave de gestão da lavanderia: eficiência (% Relave, quanto menor melhor) e produtividade (Kgs/H.H., quanto maior melhor).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Peso | Peso total lavado no dia, em kg | Sim | 42,5 kg |
| Peso Relave | Peso que precisou de segunda lavagem | Sim | 3,2 kg |
| Qtde. Maquinadas | Número de ciclos/cargas por máquina | Sim | 8 |
| % Relave | Percentual do peso total que foi relavado: indicador de qualidade | Auto (calculado) | Abaixo de 5% é saudável |
| Kgs/H.H. | Produtividade: kg lavados por hora trabalhada | Auto (calculado) | Uso para benchmarking entre unidades |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Registre o apontamento no mesmo dia da lavagem, acumular vários dias para lançar de uma vez introduz erro de memória no peso e nas máquinas usadas.
> Acompanhe o % Relave semanalmente. Um salto repentino costuma indicar produto químico errado ou máquina precisando de manutenção, não espere o fechamento do mês para investigar.
> Cruze o Kgs/H.H. com a escala de trabalho: quedas de produtividade no mesmo turno, repetidas, podem indicar necessidade de treinamento ou de mais uma pessoa na equipe.


> [!DANGER]
> Não confunda esta seção com a Gestão de Enxoval (seção 5.2). Lá você controla quantas peças de enxoval existem e se movimentam (estoque); aqui você controla o processo físico de lavagem (peso, máquinas, mão de obra). Uma peça pode estar corretamente registrada no estoque (5.2) mesmo em dias sem nenhum apontamento de lavagem aqui, e vice-versa, são registros independentes.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O % Relave está anormalmente alto | Produto de limpeza incorreto, máquina com defeito ou sobrecarga de peso por ciclo | Revise o procedimento de lavagem do dia e verifique a manutenção das máquinas apontadas |
| O Kgs/H.H. caiu de repente | Menos colaboradores no turno ou máquina parada | Confira a grade Tipo de Funcionário do apontamento e o número de Qtde. Maquinadas do período |
| Um apontamento não aparece no Histórico | Filtro de Período não cobre a data do lançamento | Amplie o intervalo de Data Início/Fim no filtro do Histórico |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Enxoval (tipos de peça processados na grade) | Relatório Controle de Lavagem: base para custo de lavanderia por kg | Peso e Qtde. Maquinadas consolidados automaticamente no relatório mensal |
| Registro de escala de colaboradores da lavanderia | Módulo Financeiro: custo de mão de obra da lavanderia | — |', NULL, NULL, NULL, NULL, 1);

-- Manutenção Preventiva [CadastroBasico/PreventivaIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'PreventivaIndex', N'Manutenção Preventiva', N'Supervisores e Técnicos', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:tecnico') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| 👤  Para quem é esta seção? | 📍  Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, planejadores de manutenção e técnicos executores. | Menu lateral > PCM > Preventiva pcmbysim.com.br/PCM/ManutencaoPreventiva |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você saberá planejar, executar e registrar manutenções preventivas em equipamentos e instalações da unidade. A preventiva é a estratégia que evita quebras inesperadas: ao invés de esperar um ativo falhar, o sistema agenda automaticamente as intervenções com base na periodicidade configurada. Isso reduz o custo total de manutenção, aumenta a vida útil dos equipamentos e garante a conformidade com normas legais como o PMOC (climatização) e a NR-13 (vasos de pressão).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> O plano de preventiva precisa estar cadastrado antes da execução: Cadastro Básico > Preventiva.
> O equipamento precisa existir no sistema: Cadastro Básico > Máquinas/Equipamentos ou Ar Condicionado.
> O checklist da atividade precisa estar criado e vinculado ao plano: Cadastro Básico > Checklist.
> O técnico executor precisa estar ativo com o switch ''Colaborador'' habilitado: Cadastro Básico > Colaborador.
> Para a visão mensal do cronograma funcionar corretamente, os planos precisam ter a periodicidade definida com precisão, este é o dado que o sistema usa para gerar as próximas datas automaticamente.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'Uma preventiva percorre o seguinte ciclo de vida no sistema:

| 1️⃣ Plano CADASTRADO | → | 2️⃣ Geração PENDENTE | → | 3️⃣ Início EM ANDAMENTO | → | 4️⃣ Checklist CONCLUÍDO | → | 5️⃣ Falha? OS CORRETIVA | → | 📊 Histórico e BI |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |


> [!INFO]
> A geração automática significa que o sistema cria a próxima preventiva com base na data de conclusão da última + o intervalo configurado no plano. Você não precisa criar manualmente a cada ciclo.
> Se uma preventiva não for executada na data prevista, o sistema a marca como ATRASADA e impacta a nota de ''Preventiva'' na tabela de Métricas por Atividade do Dashboard (seção 7.1).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Visualizar o cronograma de preventivas

![Tela principal PCM > Preventiva — cards de status e lista de preventivas do mês](/screenshots/pcm-preventiva-listagem.png)


1. Acesse o menu lateral, clique em PCM e depois em Preventiva.
1. Na tela principal, leia os cards de status no topo: PENDENTE, EM ANDAMENTO, ATRASADO e CONCLUÍDO.
1. Clique em Visualizar por Mês para ver o calendário completo de preventivas planejadas para o período.
1. Use os filtros Unidade, Equipamento, Responsável e Status para focar na visualização que precisa.


> [!INFO]
> **RESULTADO ESPERADO**
> Você tem a visão completa do que está programado, em andamento e atrasado, base para distribuir a carga de trabalho da equipe.


### 4.2  Iniciar o apontamento de uma preventiva

![Tela de apontamento — checklist com itens Conforme / Não Conforme e campo de observações](/screenshots/pcm-preventiva-apontamento.png)


1. Na listagem, localize a preventiva a ser executada e clique no ícone de Iniciar / Apontar.
1. Preencha os campos comuns da tela real de apontamento: Categoria - Serviço, Tipo de Serviço (Interno/Terceiros), Tipo - Ordem de Serviço, Solução, Valor e Quantidade Equipamento, nem todo plano usa todos, mas eles existem na tela.
1. Se o plano tiver checklist vinculado, o sistema abre os itens configurados. Testado em 2 planos reais: um não tinha checklist nenhum (apontamento só com os campos acima); outro tinha checklist com opções SIM / NÃO por item (não Conforme/Não Conforme/Não Aplicável) e um campo de Observação ao lado de cada item, confirme com o plano específico antes de assumir que o checklist é universal ou tem 3 opções.
1. Se o sistema exigir leituras numéricas (ex: pressão em bar, temperatura em °C), insira os valores medidos.
1. Tire fotos de evidência usando o campo de anexo, especialmente para itens marcados como NÃO.
1. Ao concluir todos os itens, clique em Salvar / Concluir.


> [!INFO]
> **RESULTADO ESPERADO**
> A preventiva recebe o status CONCLUÍDO e o sistema projeta automaticamente a data da próxima execução com base na periodicidade.
> O histórico do equipamento é atualizado com esta intervenção.
> A nota de Preventiva na tabela de Métricas por Atividade do Dashboard é recalculada.


### 4.3  Abrir uma OS corretiva vinculada à preventiva

Quando o técnico identifica durante a preventiva um problema que não pode ser resolvido no momento (ex: uma peça que precisa ser comprada), o sistema permite abrir uma OS corretiva diretamente da tela de apontamento, mantendo o vínculo entre os dois registros.

1. No apontamento, ao marcar um item como Não Conforme, o sistema oferece a opção Gerar OS Corretiva.
1. Clique em Gerar OS Corretiva e preencha a descrição do problema identificado.
1. Defina a Prioridade da OS, o sistema herda automaticamente o equipamento e o setor da preventiva.
1. Salve a OS e conclua normalmente a preventiva. Os dois registros ficam vinculados para rastreabilidade.


> [!INFO]
> **RESULTADO ESPERADO**
> A OS corretiva aparece na fila de pendências do gestor de PCM com referência à preventiva de origem.
> O equipamento fica com dois registros vinculados: a preventiva executada e a OS corretiva pendente.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'Campos do apontamento de preventiva:

| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Plano de Preventiva | Nome do plano configurado no Cadastro Básico | Auto | Preventiva Mensal: Ar Condicionado |
| Equipamento | Ativo vinculado ao plano: herdado automaticamente | Auto | AC Split TAG AC-302 |
| Data de Execução | Data em que a preventiva está sendo realizada | Sim | Preenchida automaticamente com a data atual |
| Técnico Responsável | Colaborador que está executando a tarefa | Sim | João Silva |
| Categoria - Serviço | Classificação do tipo de trabalho | Sim | Climatização, Elétrica... |
| Tipo de Serviço | Execução interna ou por terceiros | Sim | Interno / Terceiros |
| Tipo - Ordem de Serviço | Natureza do apontamento | Sim | Preventiva |
| Valor | Custo associado ao apontamento, quando aplicável | Não | R$ 150,00 |
| Quantidade Equipamento | Quantos equipamentos foram cobertos neste apontamento | Não | 3 |
| Item do Checklist (se o plano tiver checklist) | Pergunta ou verificação a ser respondida: nem todo plano tem checklist | Cond. | SIM / NÃO |
| Observação | Texto livre descrevendo o desvio encontrado em item Não Conforme | Cond. | Filtro com acúmulo excessivo de sujeira |
| Leitura Numérica | Valor medido quando o checklist exige (pressão, temperatura, etc.) | Cond. | 2,5 bar / 18 °C |
| Anexo / Foto | Evidência fotográfica: obrigatória para itens críticos | Não | foto_filtro_antes.jpg |


Periodicidades disponíveis no cadastro de plano:

> [!WARNING]
> **A CONFIRMAR**
> Os planos reais testados mostraram a periodicidade no formato ''N - MÊS'' (ex.: 1 - MÊS), sugere um sistema de número + unidade mais flexível do que uma lista fixa de 7 nomes. A tabela abaixo mantém os nomes como referência conceitual; antes de treinar o time com ela, vale confirmar em Cadastro Básico > Preventiva quais unidades o campo realmente aceita.


| Periodicidade | Intervalo típico | Quando usar |
| :--- | :--- | :--- |
| Diária | 1 dia | Verificações de gerador, bombas críticas, medidores de pressão |
| Semanal | 7 dias | Inspeção visual de quadros elétricos, testes de iluminação de emergência |
| Quinzenal | 15 dias | Limpeza de filtros de ar condicionado em áreas de alta ocupação |
| Mensal | 30 dias | Lubrificação de motores, inspeção de coberturas e calhas |
| Trimestral | 90 dias | Revisão de equipamentos de climatização, elevadores e sistemas hidráulicos |
| Semestral | 180 dias | Testes de vasos de pressão, revisão de SPDA, calibração de instrumentos |
| Anual | 365 dias | Substituição de filtros absolutos, revisão geral de geradores |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Sempre use o campo de Observações para itens Não Conformes, uma descrição técnica precisa (ex: ''Correia do compressor com desgaste de 30%'') é muito mais útil que apenas marcar ''Não Conforme'' sem explicação.
> Tire fotos do ''antes'' de qualquer intervenção, não apenas quando há problema. Isso cria um histórico visual do estado do equipamento ao longo do tempo.
> Quando gerar uma OS corretiva a partir da preventiva, atribua imediatamente um técnico e prazo, OS sem executor ficam invisíveis na gestão do dia a dia.
> Revise semanalmente as preventivas com status ATRASADO. Um atraso não resolvido em 7 dias geralmente indica problema de recursos ou planejamento, intervenha antes que vire crítico.


> [!DANGER]
> Não encerre a preventiva sem responder todos os itens do checklist. Itens em branco são tratados como ''não verificados'' nas auditorias e comprometem a conformidade legal do PMOC.
> Preventivas atrasadas impactam diretamente a nota de Preventiva na tabela de Métricas por Atividade do Dashboard (seção 7.1). Cada dia de atraso é registrado.
> Se o equipamento tiver uma manutenção em andamento (OS aberta), verifique com o gestor antes de executar a preventiva, pode haver interação entre os dois processos.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A preventiva não aparece na listagem mesmo estando no prazo | O plano está inativo ou o filtro de unidade está errado | Verifique em Cadastro Básico > Preventiva se o plano está ativo. Confira também se o filtro de Unidade na tela de listagem está correto |
| O sistema não gera automaticamente a próxima preventiva após a conclusão | A periodicidade não foi configurada no plano, ou foi configurada como zero | Acesse Cadastro Básico > Preventiva, abra o plano e confirme se Periodicidade e Intervalo estão preenchidos corretamente |
| O checklist abre sem nenhum item para responder | O checklist vinculado ao plano está vazio ou foi desvinculado | Acesse Cadastro Básico > Preventiva, abra o plano e verifique o campo Checklist. Se necessário, recrie o vínculo com o checklist correto |
| Não consigo anexar fotos durante o apontamento no tablet | O aplicativo mobile está desatualizado ou a permissão de câmera foi negada | Atualize o app para a versão mais recente. Verifique as permissões de câmera nas configurações do dispositivo. Se persistir, registre a foto manualmente e anexe depois pelo desktop |
| A OS corretiva gerada pela preventiva não aparece na fila do gestor | A OS foi criada sem executor atribuído e o filtro do gestor está limitado a OS com responsável | Acesse OS > Listagem, filtre por status ''Pendente'' e sem filtro de executor. Localize a OS pela descrição e atribua um técnico |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Preventiva (plano)Cadastro Básico > ChecklistCadastro Básico > Equipamentos | Dashboard: nota de Preventiva na tabela de pontuação (seção 7.1)Histórico de Ativos (prontuário técnico) | Geração automática de próxima preventiva após conclusãoAbertura de OS Corretiva vinculada |
| Cadastro Básico > Colaborador (técnico)Cadastro Básico > Unidades e Setores | Módulo Financeiro: custo de mão de obra por preventivaMódulo PMOC: cumprimento do cronograma legal | Atualização do SLA no Dashboard |', NULL, NULL, NULL, NULL, 1);

-- Gestão de Enxoval [CadastroBasico/EnxovalIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'EnxovalIndex', N'Gestão de Enxoval', N'Governança & Camareira', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:camareira') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Governantas, supervisoras de rouparia e gestores de custos operacionais. | Menu lateral > Governança > Lavanderia / Inventário de Enxoval pcmbysim.com.br/Governanca/InventarioEnxoval |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de controlar o ciclo completo do enxoval da unidade, tudo que é enviado para lavanderia externa ou controlado internamente: registrar envio e retorno de lavanderia, registrar perdas e aquisições, realizar inventários periódicos e acompanhar a movimentação de cada item. O enxoval é um dos ativos mais caros e mais vulneráveis da operação hoteleira, sem rastreabilidade, peças desaparecem ou se deterioram sem controle.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!INFO]
> **NÃO CONFUNDIR COM A SEÇÃO ''LAVANDERIA'' (5.7)**
> Esta seção cobre o **estoque** de enxoval: quantas peças existem, entram, saem ou se perdem. A operação física de lavagem em si, peso lavado, máquinas usadas, produtividade por funcionário, é um módulo separado do sistema, documentado na seção 5.7.


| Tela | URL | Função principal |
| :--- | :--- | :--- |
| Apontamento (Lavanderia) | Governanca/ApontamentoLavanderia | Registra os 5 tipos de movimento de enxoval: Uso, Saída para Lavanderia, Retorno da Lavanderia, Perda e Aquisições |
| Inventário de Enxoval | Governanca/InventarioEnxoval | Contagem periódica de todas as peças: gera o índice de acuracidade do estoque, com fluxo de aprovação |
| Movimentação de Enxoval | Governanca/MovimentacaoEnxoval | Fluxo completo de cada item ao longo de um período: saídas, entradas, perdas, aquisições e evasão |
| Uso de Enxoval | Governanca/RelatorioConsumoEnxovalDia | Consumo diário por item, com 3 formas de cálculo (Uso / Uso por Hóspede / Uso por U.H. Ocupada) |
| Tipo de Perda de Enxoval | Governanca/TipoPerda | Cadastro dos motivos de perda usados no Apontamento (Governança, não Cadastro Básico) |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Enxoval SEPARADO | → | 2️⃣ Movimento REGISTRADO | → | 3️⃣ Comprovante IMPRESSO | → | 4️⃣ Inventário PERIÓDICO | → | 📊 Movimentação Analisada |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Registrar envio para lavanderia

![Governança > Apontamento — campos de tipo, data, peso e grade de itens](/screenshots/governanca-lavanderia.png)


1. Acesse Governança > Lavanderia > Apontamento e clique em Novo.
1. Selecione a Unidade e defina o Tipo: Saída para Lavanderia.
1. Confirme a Data e informe o Peso total da carga em kg.
1. Na grade, selecione cada item de enxoval e informe a Quantidade de peças.
1. Clique em Salvar e em Imprimir para gerar o comprovante que acompanha a carga.


### 4.2  Registrar retorno da lavanderia

1. Quando o enxoval retornar lavado, acesse novamente Apontamento e clique em Novo.
1. Selecione o Tipo: Retorno da Lavanderia.
1. Confira as quantidades recebidas com o comprovante de envio, registre apenas o que efetivamente retornou.
1. Clique em Salvar.


> [!INFO]
> **IMPORTANTE SABER**
> A diferença entre o que foi enviado e o que retornou é a perda do ciclo. Acompanhe este indicador mensalmente, taxas acima de 2% indicam problema no processo de lavanderia ou furto.


### 4.3  Registrar uma perda de enxoval

1. Acesse Governança > Lavanderia > Apontamento e clique em Novo.
1. Selecione a Unidade e defina o Tipo: Perda.
1. Confirme a Data, selecione o item de Enxoval e informe a Quantidade perdida.
1. Selecione o Tipo de Perda (cadastrado previamente, ver subseção 4.7).
1. Clique em Salvar.


### 4.4  Registrar aquisição de enxoval novo

1. Acesse Governança > Lavanderia > Apontamento e clique em Novo.
1. Selecione a Unidade e defina o Tipo: Aquisições.
1. Confirme a Data, selecione o item de Enxoval e informe a Quantidade adquirida.
1. Clique em Salvar.


> [!INFO]
> **IMPORTANTE SABER**
> Existe ainda um 5º tipo de movimento, **Uso**, para consumo diário fora do ciclo de lavanderia, a natureza exata desse tipo ainda está sendo confirmada; se sua unidade usa esse tipo no dia a dia, documente aqui o cenário real de uso.


### 4.5  Realizar inventário de enxoval

1. Acesse Governança > Inventário de Enxoval e clique em Novo.
1. Selecione a Unidade e a Data do inventário.
1. Para cada item, informe a **Qtde. Rouparia** (peças contadas fisicamente na rouparia/estoque) e a **Qtde. Em Uso** (peças em circulação nos quartos ou na lavanderia) separadamente, as duas contagens juntas evitam que peças em uso sejam descontadas indevidamente do saldo.
1. Salve o inventário, ele entra com status **Aguardando Aprovação**.
1. Um responsável revisa e aprova ou reprova o inventário na listagem principal; só depois de **Aprovado** o Índice de Acuracidade conta oficialmente.


> [!INFO]
> **RESULTADO ESPERADO**
> Índice de acuracidade calculado após aprovação, meta recomendada: acima de 95%.


### 4.6  Consultar movimentação de enxoval

1. Acesse Governança > Movimentação de Enxoval.
1. Selecione a Unidade, o Período (Data Início e Fim) e, opcionalmente, o item de Enxoval.
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Tabela com o fluxo completo de cada item, quantidade em estoque, saídas e entradas na lavanderia, saldo, perdas declaradas, aquisições e o percentual de evasão (peças sumidas sem declaração). Diferente do Inventário (subseção 4.5), que registra uma contagem física pontual, a Movimentação mostra o fluxo ao longo do período.


### 4.7  Cadastrar Tipos de Perda de Enxoval

1. Acesse Governança > Tipo de Perda de Enxoval e clique em Novo.
1. Selecione a Unidade e preencha a Descrição do tipo de perda (ex.: ''Desgaste'', ''Mal Uso'').
1. Mantenha Ativo habilitado e clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> O tipo fica disponível para seleção ao registrar uma perda de enxoval no Apontamento (subseção 4.3).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Tipo | Uso, Saída para Lavanderia, Retorno da Lavanderia, Perda ou Aquisições | Sim | Saída para Lavanderia |
| Data | Data do movimento | Sim | 01/06/2025 |
| Peso (kg) | Peso total da carga: base para custo de lavanderia | Sim (envio/retorno) | 42,5 kg |
| Enxoval | Tipo de peça (Edredom, Fronha, Lençol, Toalha...) | Sim | Toalha |
| Quantidade | Número de peças deste tipo no movimento | Sim | 85 peças |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Pese a carga de enxoval sempre no mesmo equipamento e no mesmo ponto do dia, consistência na medição é fundamental para o cálculo de custo de lavanderia por kg.
> Realize inventários mensais no início do mês, antes do pico de ocupação. Um inventário durante alta temporada fica impreciso por causa do alto giro.
> Separe o enxoval por estado: Bom, Danificado (para descarte) e Em Lavagem. Peças danificadas não devem ir para a lavanderia, pesam no custo sem retorno.


> [!DANGER]
> Nunca registre um retorno de lavanderia com a quantidade do envio sem conferir fisicamente, a diferença real pode estar mascarada há meses gerando uma acuracidade falsa.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Um tipo de enxoval não aparece na grade de apontamento | O item não foi cadastrado ou está inativo em Cadastro Básico > Enxoval | Acesse Cadastro Básico > Enxoval, crie ou reative o item e reabra o apontamento |
| O índice de acuracidade está muito baixo após inventário | Peças em uso nos quartos ou na lavanderia foram lançadas junto com a Qtde. Rouparia | Revise o inventário e confirme que Qtde. Rouparia e Qtde. Em Uso foram preenchidas separadamente |
| O inventário não conta para o índice de acuracidade | Ainda está com status Aguardando Aprovação | Peça para o responsável aprovar o inventário na listagem |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Enxoval (catálogo de peças, com peso padrão por item) | Relatório Uso de Enxoval e Movimentação de Enxoval (seção 7.4) | Saldo atualizado a cada apontamento de envio, retorno, perda ou aquisição |
| Governança > Tipo de Perda de Enxoval | Análise de perdas e necessidade de reposição | Índice de acuracidade recalculado após aprovação do inventário |', NULL, NULL, NULL, NULL, 1);

-- U.H. em Dia — Checklist de Conformidade Técnica [CadastroBasico/ChecklistIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'ChecklistIndex', N'U.H. em Dia — Checklist de Conformidade Técnica', N'Supervisores e Técnicos', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:tecnico') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Técnicos de manutenção e supervisores de PCM. | Menu lateral > U.H. em Dia > Checklist pcmbysim.com.br/UH/ChecklistUH |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de acompanhar e executar o Checklist U.H. em Dia, o roteiro de inspeção técnica preventiva do apartamento, com periodicidade geralmente semestral, que confirma se o quarto está em conformidade técnica ou não. Este é um checklist predefinido, respondido item a item: diferente do Mapa de Manutenção (seção 5.8), que trata de atividades únicas, e da Dedetização (seção 5.9), que é recorrente mas sem roteiro de perguntas, aqui existe sempre um roteiro fixo de itens a responder.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> As U.H.s precisam estar cadastradas: seção 2.3.
> O Checklist de "UHemDia" (roteiro técnico preventivo) precisa estar vinculado ao Tipo de U.H.: seção 2.4, é ele quem distingue a inspeção técnica de manutenção dos checklists de limpeza da Governança (Permanência, Saída, Manutenção pós-vistoria).


> [!INFO]
> **TRÊS MÓDULOS DIFERENTES DEBAIXO DE ''U.H. EM DIA'', NÃO CONFUNDIR**
> O menu ''U.H. em Dia'' reúne 3 telas com propósitos bem diferentes, cada uma com sua própria seção neste manual: **Checklist** (esta seção, roteiro predefinido SIM/NÃO/N/A, periodicidade geralmente semestral), **Mapa de Manutenção** (seção 5.8, atividades únicas por U.H., como trocar um colchão, sem roteiro de perguntas) e **Dedetização** (seção 5.9, como o Mapa de Manutenção, mas recorrente: volta para pendente sozinha a cada ciclo, geralmente mensal).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Periodicidade VENCIDA | → | 2️⃣ U.H. PENDENTE | → | 3️⃣ Vistoria REALIZADA | → | 4️⃣ Checklist RESPONDIDO | → | 5️⃣ Apontamento SALVO | → | 📊 Status Atualizado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Consultar o painel de status do Checklist

1. Acesse U.H. em Dia > Checklist.
1. A tela abre com 5 cards de contagem no topo: ATRASADO, PENDENTE, EM MANUTENÇÃO, NOVA VISTORIA e CONCLUÍDO.
1. Abaixo, as U.H.s aparecem agrupadas visualmente por Bloco e Andar, cada uma como um card colorido mostrando a data da próxima vistoria prevista.
1. Clique em um dos cards de contagem do topo para filtrar a lista só por aquele status.


### 4.2  Realizar ou consultar o apontamento do checklist

![U.H. em Dia > Checklist — Apontamento com cabeçalho de responsáveis e tabela de itens SIM/NÃO/N/A](/screenshots/uh-checklist-apontamento.png)


1. Clique no card de uma U.H. para abrir o Apontamento dela.
1. Confira o cabeçalho: Responsável - Unidade, Responsável - Vistoria, Data Início e Data Término.
1. O Checklist aparece organizado por categoria (ex.: ''01 - QUARTO''), com cada item mostrando Código, Descrição e três colunas de resposta: SIM, NÃO e N/A.
1. Preencha a coluna Observação quando necessário, e marque Nova Vistoria num item específico se ele precisar ser reavaliado numa próxima passagem, em vez de forçar uma resposta SIM/NÃO.
1. Use Imprimir para gerar uma via física do checklist respondido, se necessário.


> [!INFO]
> **RESULTADO ESPERADO**
> Com todos os itens respondidos, a U.H. sai do status PENDENTE/ATRASADO e vai para CONCLUÍDO. A próxima data de vistoria é recalculada automaticamente conforme a periodicidade configurada.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Status | Situação da U.H. no ciclo do checklist | Auto | ATRASADO / PENDENTE / EM MANUTENÇÃO / NOVA VISTORIA / CONCLUÍDO |
| Responsável - Unidade / Vistoria | Colaboradores vinculados ao apontamento | Auto | José Araújo |
| Data Início / Término | Janela de execução da vistoria | Auto | 14/08/2026 11:00 – 11:40 |
| Código / Descrição do item | Identificação do ponto de checklist | Sim | 01.01.001: Armários estão em boa condição? |
| SIM / NÃO / N/A | Resposta do item | Sim | SIM |
| Nova Vistoria | Marca o item para reavaliação numa próxima passagem | Não | — |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!WARNING]
> Ao sinalizar que foi encontrada uma não conformidade durante a vistoria do "UHemDIA" uma Ordem de serviço será aberta e ficará vinculada ao apartamento. Este apartamento ficará em "Manutenção" até a conclusão desta ordem de serviço. Quando ela for concluída, o gestor deverá realizar uma "Nova vistoria" na UH para garantir que o serviço foi realizado corretamente e assim fazer a liberação do apartamento.

> [!BOAPRATICA]
> Revise o card ATRASADO diariamente, nenhuma U.H. deveria acumular mais de um ciclo de checklist vencido sem justificativa.
> Use Nova Vistoria com critério: é para itens que precisam de reavaliação (ex.: aguardando peça), não como forma de adiar uma resposta.
> Cruze o card EM MANUTENÇÃO com as OS abertas da unidade, um checklist parado nesse status por muito tempo pode indicar uma OS esquecida.


> [!DANGER]
> Não confunda este checklist técnico com o checklist de arrumação usado pela Governança (camareira, seção 5.1), são roteiros diferentes, para públicos e objetivos diferentes, mesmo estando associados à mesma U.H.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A U.H. não aparece na lista do Checklist | O Tipo de U.H. não tem o Checklist U.H. em Dia vinculado | Acesse Cadastro Básico > Tipo de U.H. (seção 2.4) e vincule o checklist correto |
| O status não muda mesmo depois de responder tudo | Ainda restam itens sem resposta em alguma categoria do checklist | Role a tabela completa e confirme que todos os itens de todas as categorias foram respondidos |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Tipo de U.H. — vínculo do Checklist U.H. em Dia (seção 2.4) | Dashboard: indicador U.H. em Dia (Painel de Controle, seção 7.1) | Próxima data de vistoria recalculada automaticamente ao concluir o apontamento |', NULL, NULL, NULL, NULL, 1);

-- Desempenho de Governança — KPIs, Score e Rankings [Home/DesempenhoGovernanca]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Home', N'DesempenhoGovernanca', N'Desempenho de Governança — KPIs, Score e Rankings', N'Governança & Camareira', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:camareira') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de Governança, diretores e coordenadores de qualidade. | Menu lateral > Desempenho - Governança pcmbysim.com.br/Home/DesempenhoGovernanca |', NULL, NULL, NULL, N'https://drive.google.com/file/d/15wwC2MWRaKv5lxjnAnppzYsGEJpZMXcU/preview', 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de ler o painel de desempenho da governança da sua unidade: quantas U.H.s foram arrumadas e vistoriadas, a nota (Score) da unidade, e o ranking de cada camareira por produtividade e qualidade. É a tela que transforma os apontamentos do dia a dia (seções 5.1, 5.10 e 5.12) em indicadores de gestão.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Os pesos e metas usados nos cálculos desta tela são configurados em Administração > Metas e Parâmetros de Governança (seção 1.6), configure-os antes de cobrar metas da equipe.


> [!INFO]
> **DUAS VISÕES NESTA TELA**
> Um seletor no cabeçalho do sistema alterna entre **Unidade Individual** (detalha uma unidade específica, com ranking de camareiras) e **Todas as Unidades** (compara todas as unidades da rede). O texto abaixo cobre as duas.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Arrumação APONTADA | → | 2️⃣ Vistoria do SUPERVISOR | → | 3️⃣ NC/Retrabalho REGISTRADOS | → | 4️⃣ Score CALCULADO | → | 📊 Ranking Atualizado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Desempenho de Governança — KPIs, gauge de score e gráficos de evolução](/screenshots/desempenho-governanca-individual.png)


### 4.1  Visão Unidade Individual, Indicadores e Score

1. Acesse Desempenho - Governança com uma unidade específica selecionada no cabeçalho.
1. No topo, confira os 8 KPIs do mês: UHs Arrumadas, UHs Vistoriadas, % Vistoria, Total NC, Total Retrabalho, Índice NC, Índice Retrabalho e OS Manutenção.
1. O gauge ''Score de Desempenho da Unidade'' mostra a nota de 0 a 100 da unidade no mês, a Meta Mensal (85 por padrão), o Ranking da unidade na rede e os Dias Restantes do mês.


> [!INFO]
> **FÓRMULA DO SCORE DA UNIDADE (0–100)**
> Score = NC×30% + Vistoria×30% + Produtividade×30% + Retrabalho×10%, pesos configuráveis na seção 1.6. Unidades com menos de 100 UHs no mês (configurável) ficam como ''Sem dados''. Faixas: vermelho 0–54 (crítico), âmbar 55–84 (abaixo da meta), verde 85–100 (na meta).


### 4.2  Gráficos de evolução

1. Role até ''Evolução Diária — NC e Retrabalho'': barras empilhadas por dia do mês, retrabalho na base e NC acima.
1. Em ''Arrumado × Vistoriado'': barras empilhadas (Saída/Permanência/Manutenção) com uma linha de UHs vistoriadas sobreposta e uma linha tracejada de meta diária.


### 4.3  Rankings de camareiras

1. Ranking Geral: Nota de 1 a 10 por camareira, elegível a partir de 20 UHs no mês.
1. Ranking por Produtividade: ordena por UH/Dia (volume de trabalho).
1. Ranking por Qualidade: ordena pelos menores índices de NC e Retrabalho.
1. Top 10, Itens com Mais Não Conformidades: lista os itens do checklist de vistoria que mais geraram NC no mês, com % do total, o relatório mais acionável da tela para planejar treinamento.


> [!INFO]
> **FÓRMULA DA NOTA DA CAMAREIRA (1–10)**
> Nota = (Produtividade×60% + NC×30% + Retrabalho×10%) ÷ 10, pesos configuráveis na seção 1.6. Elegível a partir de 20 UHs no mês. A camareira NÃO é avaliada pela % de vistoria (isso é responsabilidade do supervisor), apenas pelos resultados (NC e Retrabalho) encontrados nos quartos que ela arrumou.


### 4.4  Visão Todas as Unidades

1. Selecione ''TODAS AS UNIDADES'' no filtro do cabeçalho (disponível em contas com visão de rede).
1. A visão traz 7 KPIs consolidados da rede, tabela de dados por unidade, ranking de unidades por Score com linha de meta tracejada, gráficos de NC e % Vistoria por unidade, gráfico Arrumado × Vistoriado por unidade e gráfico de produtividade (UHs/camareira) por unidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Índice NC | NC ÷ UHs vistoriadas × 100 | Auto | Métrica mais justa que o total absoluto |
| Índice Retrabalho | Retrabalho ÷ UHs vistoriadas × 100 | Auto | — |
| Score da Unidade | Nota composta 0–100 | Auto | Meta mínima: 85 |
| Nota da Camareira | Nota composta 1,0–10,0 | Auto | Elegível ≥ 20 UHs |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Ataque sempre o item nº 1 do Top 10 de NC antes dos demais, o princípio de Pareto se aplica: poucos itens costumam concentrar a maioria das não conformidades.
> Cruze o Ranking de Produtividade com o de Qualidade, uma camareira no topo de produtividade com nota baixa em qualidade pode estar priorizando velocidade.


> [!DANGER]
> Compare sempre os Índices (NC% e Retrabalho%), nunca os totais absolutos, ao comparar unidades ou camareiras entre si, o total absoluto sobe só porque há mais volume vistoriado, não necessariamente mais problemas.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O Score aparece como ''Sem dados'' | A unidade tem menos UHs no mês do que a Qtd. Mínima Elegível configurada (seção 1.6) | Aguarde o volume do mês crescer, ou revise a Qtd. Mínima Elegível se ela estiver alta demais para o porte da unidade |
| A Nota da camareira aparece como ''—'' | Ela ainda não atingiu 20 UHs no mês | Aguarde o fechamento do mês ou o acúmulo de mais apontamentos |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Governança > Apontamento / Discrepâncias / Checklist (seções 5.1, 5.10, 5.12) | Reuniões de gestão de Governança: plano de ação por item de NC | Score e Notas recalculados em tempo real conforme novos apontamentos |
| Administração > Metas e Parâmetros de Governança (seção 1.6) | Pesos e metas usados em todos os cálculos desta tela | — |', NULL, NULL, NULL, NULL, 1);

-- Mapa de Manutenção da U.H. — Atividades Únicas [CadastroBasico/AtividadeIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'AtividadeIndex', N'Mapa de Manutenção da U.H. — Atividades Únicas', N'Supervisores e Técnicos', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:tecnico') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Técnicos de manutenção e supervisores de PCM. | Menu lateral > U.H. em Dia > Mapa de Manutenção pcmbysim.com.br/UH/UHAtividade |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar e acompanhar atividades únicas de manutenção por U.H.. Atividades como virar os colchões ou instalar um equipamento novo em cada quarto deverão ser controlados através desta ferramenta. Diferente do Checklist de "UHemDia" (seção 5.1), aqui não existe um roteiro de perguntas SIM/NÃO: cada atividade é simplesmente marcada como concluída (ou não), com a data da conclusão e tempo decorrido da atividade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> A atividade em si precisa estar cadastrada no Mapa de Manutenção: Cadastro Básico > Mapa de Manutenção (seção 2.17), é lá que se define o que é ''Virada de Colchões'' ou ''Instalar Varal Retrátil''. Esta seção trata apenas do acompanhamento por U.H., não do cadastro da atividade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Atividade CADASTRADA | → | 2️⃣ Aplicação nas U.H.s | → | 3️⃣ Execução pelo TÉCNICO | → | 4️⃣ Conclusão REGISTRADA | → | 📊 Status Atualizado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Consultar e filtrar atividades por U.H.

![U.H. em Dia > Mapa de Manutenção — filtro por atividade e cards de status](/screenshots/uh-mapa-manutencao-filtro.png)


1. Acesse U.H. em Dia > Mapa de Manutenção.
1. Selecione a Unidade e a atividade específica no filtro Mapa de Manutenção (ex.: VIRADA DE COLCHÕES, INSTALAR VARAL RETRÁTIL, a lista vem do cadastro da seção 2.17).
1. Opcionalmente, filtre por Local / U.H., Status (ATRASADO/PENDENTE/CONCLUÍDO) ou Status UH (NÃO APLICÁVEL/NÃO OK/OK).
1. Clique em Filtrar.


> [!INFO]
> **DUAS COLUNAS DE STATUS DIFERENTES, NÃO CONFUNDIR**
> Status (ATRASADO/PENDENTE/CONCLUÍDO) indica se a atividade foi executada naquela U.H. Status UH (NÃO APLICÁVEL/NÃO OK/OK) é uma segunda informação, independente, por exemplo, marcar como NÃO APLICÁVEL uma U.H. que não tem varal retrátil para instalar. São duas perguntas diferentes sobre a mesma linha.


### 4.2  Consultar o histórico de atividades concluídas

1. Acesse U.H. em Dia > Mapa de Manutenção - Histórico.
1. Use os mesmos filtros da tela de consulta (Unidade, atividade, Local/U.H., período).
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Lista das execuções já concluídas, com a data de conclusão de cada U.H., diferente da Dedetização (seção 5.9), uma atividade concluída aqui **não volta** automaticamente para pendente; é uma conclusão definitiva, a menos que a atividade seja reaplicada manualmente.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Mapa de Manutenção | Atividade sendo acompanhada: vem do cadastro da seção 2.17 | Sim | VIRADA DE COLCHÕES |
| Local / U.H. | Apartamento ou área ao qual a atividade se aplica | Sim | 0105 |
| Status | Situação da execução da atividade nesta U.H. | Auto | ATRASADO / PENDENTE / CONCLUÍDO |
| Status UH | Avaliação complementar, independente do Status | --- | OK / NÃO OK / NÃO APLICÁVEL |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Use o Mapa de Manutenção para campanhas pontuais (troca de peças, reforma parcial, instalação de item novo), não para nada que precise se repetir sozinho no tempo, isso é papel da Dedetização (seção 5.9) ou de uma Preventiva (seção 3.2).
> Marque Status UH como NÃO APLICÁVEL assim que perceber que uma U.H. não se aplica à campanha, em vez de deixá-la pendente indefinidamente distorcendo os indicadores.


> [!DANGER]
> Não confunda esta tela com o cadastro da atividade em si (Cadastro Básico > Mapa de Manutenção, seção 2.17), lá se cria/edita a atividade (o que ela é); aqui se acompanha e conclui a execução dela, U.H. por U.H.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A atividade não aparece no filtro Mapa de Manutenção | A atividade não foi cadastrada ou está inativa em Cadastro Básico > Mapa de Manutenção | Acesse a seção 2.17, cadastre ou reative a atividade |
| Uma U.H. que não deveria ter a atividade aparece como pendente | A atividade foi aplicada a todas as U.H.s sem exceção | Marque o Status UH dela como NÃO APLICÁVEL |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Mapa de Manutenção (seção 2.17) | Histórico de atividades por U.H.: evidência de campanhas de manutenção concluídas | — |', NULL, NULL, NULL, NULL, 1);

-- Metas e Parâmetros de Governança [Administracao/MetasParametrosGovernanca]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Administracao', N'MetasParametrosGovernanca', N'Metas e Parâmetros de Governança', N'Governança & Camareira', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:camareira') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Administradores e gestores de Governança. | Menu lateral > Administração > Metas de Governança pcmbysim.com.br/Administracao/MetasParametrosGovernanca |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de configurar as réguas que alimentam todos os cálculos do módulo Desempenho de Governança (seção 7.10): os pesos de cada pilar no Score da unidade e na Nota da camareira, as metas padrão de atendimento, produtividade e vistoria, e as metas individuais por unidade quando ela precisa de um valor diferente do padrão da rede.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> **ANTES DE COMEÇAR**
> Esta tela é de uso administrativo, mudanças aqui afetam imediatamente o cálculo de Score e Nota de todas as unidades. Só altere com autorização do gestor de rede.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Parâmetros GERAIS | → | 2️⃣ Metas por UNIDADE | → | 3️⃣ Config. SALVA | → | 📊 Desempenho Recalculado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Administração > Metas e Parâmetros de Governança — pesos, metas padrão e tabela por unidade](/screenshots/metas-parametros-governanca.png)


### 4.1  Configurar os Parâmetros Gerais da Empresa

1. Acesse Administração > Metas de Governança.
1. Em ''Meta de Atendimento — Padrão'', preencha: % Meta de Atendimento (padrão real observado: 90%, aplicado sobre os Aptos Totais do PMS), Meta Aptos Dia / Camareira (padrão: 18 UHs/dia) e Meta, % Vistoria (padrão: 95%).
1. Em ''Pesos — Ranking de Todas as Unidades'', preencha NC, Vistoria, Produtividade e Retrabalho, a soma precisa fechar 100% (padrão real: 30/30/30/10), e a Qtd. Mínima Elegível (padrão: 100 UHs).
1. Em ''Pesos — Ranking Individual (Governança Nota 10)'', preencha Produtividade, NC e Retrabalho, soma 100% (padrão real: 60/30/10) —, a Qtd. Mínima Elegível (padrão: 20 UHs) e a Base NC/Retrabalho, % Vistoriados Estimado (padrão: 80%).
1. Clique em Salvar Parâmetros Gerais.


> [!INFO]
> **IMPORTANTE SABER**
> A Base NC/Retrabalho existe porque nem toda UH arrumada é vistoriada, o sistema estima a base de cálculo como UHs arrumadas × este percentual, para não penalizar camareiras de unidades com baixa cobertura de vistoria.


### 4.2  Definir Metas por Unidade (sobrescrever o padrão)

1. Role até a tabela ''Metas por Unidade''.
1. Cada linha é uma unidade da rede, com o Aptos Totais PMS (vem do PMS, não do cadastro de U.H. do PCM by SIM) e o % Meta daquela unidade.
1. Preencha % Meta, Meta Aptos Dia / Camareira e Meta % Vistoria apenas para as unidades que precisam de um valor diferente do padrão global, deixe em branco para manter o padrão.
1. A coluna = Meta Calculada mostra automaticamente Aptos Totais PMS × % Meta.
1. A coluna Status mostra ''Personalizado'' (algum valor diferente do padrão) ou ''Padrão'' (segue o padrão global).
1. Clique em Salvar Metas por Unidade.


> [!INFO]
> **RESULTADO ESPERADO**
> As metas e pesos configurados aqui passam a valer imediatamente nos cálculos de Score e Nota do Desempenho de Governança (seção 7.10).', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| % Meta de Atendimento | % dos Aptos Totais PMS que a unidade deve arrumar no mês | Sim | 90% |
| Meta Aptos Dia / Camareira | Benchmark de produtividade diária por camareira | Sim | 18 UHs/dia |
| Meta: % Vistoria | % mínimo das saídas que deve ser vistoriado | Sim | 95% |
| Pesos Ranking Unidades (NC/Vistoria/Produtividade/Retrabalho) | Compõem o Score da unidade: soma deve ser 100% | Sim | 30/30/30/10 |
| Pesos Ranking Individual (Produtividade/NC/Retrabalho) | Compõem a Nota da camareira: soma deve ser 100% | Sim | 60/30/10 |
| Aptos Totais PMS | Total de UHs disponíveis segundo o PMS: não vem do cadastro de U.H. do PCM by SIM | Auto (PMS) | 4.210 |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Confira sempre se a soma dos pesos fecha 100% antes de salvar, o sistema sinaliza visualmente, mas o botão Salvar não bloqueia uma soma incorreta.
> Atualize o Aptos Totais PMS mensalmente, ele reflete a oferta real do hotel (reformas, sazonalidade) e, se ficar desatualizado, distorce a Meta Calculada.
> Use metas personalizadas por unidade só quando houver diferença estrutural real (ex.: uma unidade com equipe de supervisão menor pode ter Meta % Vistoria mais baixa), personalizar sem critério dificulta a comparação justa no ranking de rede.


> [!DANGER]
> Alterar os pesos gerais recalcula o Score e a Nota de **todas** as unidades e camareiras da rede, não é uma mudança isolada. Avise a rede antes de alterar.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A soma dos pesos não fecha 100% | Um dos campos foi digitado errado | Revise os 4 (ou 3) campos de peso do bloco correspondente antes de salvar |
| A Meta Calculada de uma unidade parece errada | O Aptos Totais PMS está desatualizado | Confirme o valor real do PMS para aquela unidade no mês |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| PMS: Aptos Totais por unidade e mês | Desempenho de Governança: Score e Rankings (seção 7.10) | Recalculo em tempo real dos indicadores ao salvar |', NULL, NULL, NULL, NULL, 1);

-- Dedetização — Controle Recorrente por U.H. [CadastroBasico/Dedetizacao]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'Dedetizacao', N'Dedetização — Controle Recorrente por U.H.', N'Supervisores e Técnicos', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:tecnico') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Técnicos de manutenção e supervisores de PCM. | Menu lateral > U.H. em Dia > Dedetização pcmbysim.com.br/UH/Dedetizacao |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de configurar o ciclo de dedetização das U.H.s e registrar a execução de cada aplicação, entendendo por que este módulo se comporta diferente do Mapa de Manutenção (seção 5.8): a Dedetização é **recorrente**, de acordo com a regra de vigilância sanitária local (geralmente mensal), a U.H. volta sozinha para o status PENDENTE assim que o prazo da periodicidade vence de novo. Não é uma conclusão definitiva como uma atividade única.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> A Unidade e as U.H.s precisam já estar cadastradas (seção 2.3) antes de configurar o agendamento.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Agendamento CONFIGURADO | → | 2️⃣ Prazo VENCIDO | → | 3️⃣ U.H. PENDENTE | → | 4️⃣ Dedetização EXECUTADA | → | 5️⃣ Registro SALVO | → | 📊 Ciclo Reinicia |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Configurar o agendamento em massa

1. Acesse Cadastro Básico > Dedetização.
1. Esta tela **não é** um formulário de um certificado por vez com Fornecedor/Data da Aplicação/Data de Validade/Upload de PDF. É uma ferramenta de agendamento em massa: preencha Unidade, Periodicidade (Dia/Mês/Semanas), Nº dias Alerta, Tipo de Serviço (Interno/Terceiros) e Data Início.
1. Selecione, numa grade com todas as U.H.s da unidade (Local/U.H., Tipo de U.H., Setor, Bloco, Andar), quais U.H.s recebem o agendamento.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> As U.H.s selecionadas passam a ter dedetização agendada conforme a Periodicidade definida. Na prática, a tela funciona como um mapa: a pessoa seleciona o quarto e informa quando a dedetização foi realizada, e o sistema calcula automaticamente a próxima data de vencimento com base na Periodicidade configurada.


### 4.2  Registrar dedetização por U.H.

![U.H. em Dia > Dedetização — grade de U.H.s agrupadas por bloco e andar, com data da última aplicação](/screenshots/uh-dedetizacao-listagem.png)


1. Acesse U.H. em Dia > Dedetização.
1. A tela abre com 3 cards de contagem no topo (ATRASADO, PENDENTE, CONCLUÍDO) e, abaixo, as U.H.s agrupadas visualmente por Bloco e Andar, cada uma como um card pequeno mostrando a data da última aplicação. O status (atrasado/pendente/concluído) é calculado automaticamente a partir da data, não escolhido manualmente.
1. Para atualizar uma única U.H., clique no card dela para abrir o apontamento individual. Para atualizar várias de uma vez, use o botão Apontamento Múltiplos no topo da tela.


> [!INFO]
> **CAMPOS REAIS DO APONTAMENTO INDIVIDUAL**
> Ao clicar numa U.H., os campos são: Unidade, Descrição (fixa, sempre DEDETIZAÇÃO), Fornecedor, Observação e Data. Não existe um campo Status para editar diretamente, ele é sempre calculado pela Data informada e pela Periodicidade configurada no passo 4.1.


> [!DANGER]
> A grade carrega todas as U.H.s da unidade, separando as informações por blocos e andares para facilitar que o usuário encontre as U.Hs que o usuário deseja apontar.


### 4.3  Consultar histórico de dedetização por U.H.

1. Acesse U.H. em Dia > Dedetização - Histórico.
1. Filtre por Unidade, Período (Data Início e Fim) e, opcionalmente, a U.H. específica.
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Lista completa de quando cada U.H. recebeu dedetização, com o fornecedor responsável e observações registradas. O histórico mostra várias aplicações ao longo do tempo para a mesma U.H., já que o ciclo se repete e permite demostrar o controle geral de dedetização dos ambientes', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Periodicidade | Intervalo do ciclo de agendamento em massa (Dia/Mês/Semanas) | Sim | Mensal |
| Nº dias Alerta | Antecedência para sinalizar vencimento próximo | Sim | 5 dias |
| Tipo de Serviço | Interno ou Terceiros | Sim | Terceiros |
| Data Início | Data de referência do primeiro ciclo | Sim | 01/07/2026 |
| Status (ATRASADO/PENDENTE/CONCLUÍDO) | Calculado automaticamente a partir da Data e da Periodicidade: volta a PENDENTE sozinho a cada ciclo | Auto | Card colorido no topo e por U.H. |
| Descrição | Fixa, sempre DEDETIZAÇÃO: não editável | Auto | DEDETIZAÇÃO |
| Fornecedor | Empresa responsável pela aplicação | Sim | Dedetizadora XYZ |
| Observação | Texto livre sobre a aplicação | Não | Aplicação preventiva mensal |
| Data | Data da última aplicação de dedetização | Sim | 04/07/2026 |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Mantenha a grade de Dedetização atualizada assim que o serviço for executado. O histórico é a evidência usada em auditorias sanitárias.
> Revise a Periodicidade configurada no passo 4.1 sempre que a exigência da vigilância sanitária local mudar.


> [!DANGER]
> Não deixe o certificado de dedetização vencer sem renovação agendada, em auditorias sanitárias, é um dos primeiros documentos solicitados.
> Não confunda com o Mapa de Manutenção (seção 5.8): lá a conclusão é definitiva; aqui, por ser recorrente, a U.H. concluída hoje vai automaticamente para PENDENTE de novo quando o próximo ciclo vencer.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Uma U.H. voltou para PENDENTE mesmo já tendo sido dedetizada | Comportamento esperado: o ciclo da Periodicidade venceu de novo | Registre a nova aplicação normalmente; não é um erro do sistema |
| A data de vencimento não bate com a regra local de vigilância sanitária | A Periodicidade configurada no passo 4.1 está desatualizada | Ajuste a Periodicidade em Cadastro Básico > Dedetização |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro de U.H.s (seção 2.3) | Auditorias sanitárias (A&B, Qualidade): evidência de conformidade | Status recalculado automaticamente a cada novo ciclo da Periodicidade |', NULL, NULL, NULL, NULL, 1);

-- Relatório de Produtividade e Qualidade de Camareiras [Governanca/RelatorioCamareiraNC]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Governanca', N'RelatorioCamareiraNC', N'Relatório de Produtividade e Qualidade de Camareiras', N'Governança & Camareira', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:camareira') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de Governança e supervisoras. | Menu lateral > Governança > Relatório > Camareira x UH / NC / Horas pcmbysim.com.br/Governanca/RelatorioCamareiraUH |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de medir e analisar a produtividade da equipe de governança: quantos quartos cada camareira limpou por dia, quantas não conformidades teve nas vistorias e quantas horas trabalhou no período. Esses dados transformam a gestão de governança de subjetiva para objetiva.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'| Relatório | URL | O que mostra |
| :--- | :--- | :--- |
| Camareira x UH | Governanca/RelatorioCamareiraUH | Matriz Camareira × dias do mês, com a quantidade de U.H.s apontadas em cada dia |
| Camareira x NC | Governanca/RelatorioCamareiraNC | Matriz Camareira → item de checklist × dias do mês, com a contagem de não conformidades |
| Horas Trabalhadas | Governanca/FuncionarioHorasTrabalhadasGovernanca | Total de horas registradas por colaboradora por mês ao longo do ano |


> [!INFO]
> **IMPORTANTE SABER**
> Os campos com **horário de início e fim por quarto, tempo gasto, checklist, não conformidade, quantidade de OS e vistoria**, que dão o detalhe operação a operação, não ficam nestes 3 relatórios. Eles estão na tela **Governança > Histórico** (seção 7.4), que lista cada limpeza individualmente. Use os relatórios desta seção para visão agregada por dia/mês e o Histórico para o detalhe de uma limpeza específica.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Limpeza EXECUTADA | → | 2️⃣ Apontamento SALVO | → | 3️⃣ Vistoria REALIZADA | → | 4️⃣ Dados CONSOLIDADOS | → | 📊 Relatório em Matriz |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Governança > Relatório Camareira x UH — matriz de camareira por dia do mês](/screenshots/governanca-relatorio-camareira-uh.png)


1. Acesse Governança > Relatório > Camareira x UH.
1. Selecione a Unidade e o Mês de análise (seletor de mês único, ex.: AGO/2026).
1. Opcionalmente filtre por Camareira específica para análise individual.
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Uma matriz: cada linha é uma Camareira, cada coluna é um dia do mês (1 a 31), cada célula é a quantidade de U.H.s apontadas naquele dia, com uma coluna Total ao final.


> [!INFO]
> **IMPORTANTE SABER**
> Este relatório está com essa estrutura hoje; é uma tela candidata a melhoria futura para ficar mais rica, fique de olho em próximas atualizações do manual.


### 4.1 Relatório Camareira x NC

1. Acesse Governança > Relatório > Camareira x NC.
1. Selecione Unidade, Mês e, opcionalmente, a Camareira.
1. Escolha a Forma de Visualização: **Peso** ou **Quantidade**.
1. Opcionalmente filtre pelo Tipo de NC: Retrabalho, NC ou Todos.
1. Clique em Filtrar.


> [!INFO]
> **RESULTADO ESPERADO**
> Matriz Camareira → item do checklist × dias do mês, com a contagem (ou peso) de não conformidades em cada célula.


> [!INFO]
> **IMPORTANTE SABER**
> Como usar o relatório de NC para melhoria contínua:
> Se UMA camareira tem muitas NCs no mesmo item: direcione treinamento prático específico para ela.
> Se TODAS as camareiras têm NC no mesmo item: o problema é do processo ou do estoque (ex.: falta de insumo específico), não da camareira.
> Meta recomendada: menos de 5% de NCs por camareira por semana.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Camareira x UH | Matriz de U.H.s apontadas por camareira e dia do mês | --- | Uso semanal |
| Camareira x NC | Matriz de não conformidades por camareira, item e dia do mês | --- | Uso semanal |
| Horas Trabalhadas Gov. | Matriz de horas registradas por colaboradora e mês do ano | --- | Uso mensal para RH |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Use os dados de produtividade nas reuniões semanais de governança, mostre os números para a equipe. Transparência gera responsabilidade.
> Combine os três relatórios: uma camareira com muitas UHs, poucas NCs e horas dentro do esperado é candidata a supervisora.
> Nunca use o relatório para punição isolada, use como base para conversa e treinamento focado.


> [!DANGER]
> Para o detalhe de horário de uma limpeza específica, use o Histórico (seção 7.4), os relatórios desta seção são matrizes agregadas por dia, não mostram horário início/fim.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O relatório está em branco mesmo com limpezas realizadas | As limpezas foram realizadas mas os apontamentos não foram salvos no sistema | Verifique com a supervisora se as camareiras estão concluindo o apontamento no app. Apontamentos em rascunho não aparecem nos relatórios |
| Preciso do horário de uma limpeza e não encontro neste relatório | Este relatório é uma matriz por dia, não uma lista com horário | Consulte a tela Governança > Histórico (seção 7.4) para o detalhe individual |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Governança > Apontamento (arrumações das camareiras) | Gestão de RH: dados para avaliação de desempenho | Dados alimentados automaticamente a cada apontamento concluído |
| Horas trabalhadas por colaboradora | Módulo Financeiro: custo de mão de obra de Governança | Filtro de Colaborador específico, Ativo e tipo Faltas/Horas/Horas-Faltas disponível em Horas Trabalhadas |', NULL, NULL, NULL, NULL, 1);

-- Green Planet — Lançamento de Medições [GreenPlanet/LancamentoIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'GreenPlanet', N'LancamentoIndex', N'Green Planet — Lançamento de Medições', N'Supervisores e Técnicos', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:tecnico') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de operação, sustentabilidade e supervisores de manutenção. | Menu lateral > Green Planet > Medição pcmbysim.com.br/GreenPlanet/LancamentoIndex |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar o consumo diário de água, energia e gás da unidade e interpretar os KPIs automáticos gerados pelo sistema, consumo per capita, consumo por UH ocupada e desvios em relação à média histórica, para identificar desperdícios e vazamentos antes que virem fatura alta.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> Os grupos e itens de medição precisam estar cadastrados com meta de consumo definida: seção 6.1.
> Tenha os dados de ocupação do dia (quantidade de hóspedes e quartos ocupados) disponíveis, eles entram no cálculo dos KPIs.
> Defina um horário fixo do dia para o lançamento (recomendado: início do turno da manhã), consistência de horário é o que torna o consumo comparável entre os dias.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Ocupação INFORMADA | → | 2️⃣ Leitura REGISTRADA | → | 3️⃣ KPI CALCULADO | → | 4️⃣ Desvio IDENTIFICADO | → | 📊 OS de Inspeção Aberta |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Green Planet > Medição — grade de medidores com campos de leitura e dados de ocupação](/screenshots/green-planet-medicao.png)


1. Acesse Green Planet > Medição.
1. Selecione a Unidade e confirme a Data da leitura.
1. Preencha os dados de ocupação: Qtde. Hóspedes e Quartos Ocupados.
1. Para cada medidor listado, insira a Leitura Atual conforme o mostrador físico.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> A leitura é salva e o registro fica marcado com asterisco (**) para evitar duplicidade, isso foi confirmado em teste real.
> Os KPIs (consumo per capita, por UH ocupada e desvio) NÃO aparecem nesta tela depois de salvar, testado com um lançamento real (300 hóspedes, 145 quartos, medidor Combustível Carro) e nenhum indicador apareceu na tela de Medição. Eles provavelmente ficam disponíveis em Relatório > Green Planet, uma tela separada, consulte os KPIs por lá, não espere vê-los aqui.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Data da Leitura | Dia ao qual o lançamento se refere: no formulário real, é um filtro no painel esquerdo, não um campo dentro do card Medição | Sim | 04/07/2026 |
| Qtde. Hóspedes | Ocupação do dia: usada no cálculo per capita | Sim | 320 |
| Quartos Ocupados | Ocupação do dia: usada no cálculo por UH | Sim | 145 |
| Leitura Atual | Valor lido no mostrador físico do medidor | Sim | 1248 (m³) |


Indicadores calculados automaticamente a partir do lançamento:

| KPI | Fórmula | Como usar |
| :--- | :--- | :--- |
| Consumo per Capita | Consumo do dia / Qtde. Hóspedes | Benchmarking entre unidades: quanto cada hóspede consome em média |
| Consumo por UH Ocupada | Consumo do dia / Quartos Ocupados | Eficiência por quarto: detecta desperdicio em áreas comuns |
| Variacao em relacao a média | (Consumo atual - Média historica) / Média historica | Desvio acima de 20% pode indicar vazamento ou equipamento com defeito |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Registre as medições sempre no mesmo horário, preferencialmente no início do turno da manhã.
> Quando o consumo de água subir mais de 15% em relação à média sem aumento de ocupação, abra imediatamente uma OS de inspeção de vazamentos, uma tubulação rompida pode gerar prejuízos de dezenas de milhares de reais por semana.
> Use os dados de Green Planet para negociar com as concessionárias, histórico de consumo detalhado pode embasar pedidos de redução de tarifa.


> [!DANGER]
> A meta de consumo zerada no cadastro do item de medição (seção 6.1) faz com que qualquer leitura seja considerada ''dentro do normal'', confirme que as metas foram configuradas antes de confiar nos alertas de desvio.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O cálculo per capita está distorcido | Os dados de ocupação (hóspedes e quartos) foram preenchidos incorretamente ou deixados zerados | Edite o lançamento do dia e corrija os campos de ocupação |
| O sistema sinaliza desvio mas o consumo parece normal | A meta ou média histórica está configurada incorretamente no item de medição | Revise o campo Meta de Consumo no cadastro do medidor (seção 6.1) |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Green Planet > Configuração de Medições (seção 6.1) — grupos, medidores e metas | PCM > OS — alerta de desvio pode gerar chamado de inspeção de vazamento | Alerta automático de desvio brusco no consumo diário |', NULL, NULL, NULL, NULL, 1);

-- Laudos — Apontamento de Horas [Governanca/Apontamento]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'Governanca', N'Apontamento', N'Laudos — Apontamento de Horas', N'Supervisores e Técnicos', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:tecnico') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Técnicos e supervisores que executam vistorias técnicas para laudos. | Menu lateral > PCM > Laudo / Documentação |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar as horas efetivamente gastas por um técnico numa vistoria ou inspeção vinculada a um laudo técnico, separando esse tempo do apontamento de horas de uma OS comum (seção 3.1). Isso garante que o custo de mão de obra de conformidade (laudos, ARTs, certificações) apareça corretamente nos relatórios, sem ficar misturado ao custo de manutenção corretiva.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> O laudo precisa estar cadastrado e com uma renovação em andamento: seção 3.5.
> O técnico responsável pela vistoria precisa estar ativo no sistema: seção 2.9.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Vistoria AGENDADA | → | 2️⃣ Apontamento INICIADO | → | 3️⃣ Horas REGISTRADAS | → | 4️⃣ Laudo ANEXADO | → | 📊 Custo Lançado |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'> [!INFO]
> **PRINT: PCM > Laudo, apontamento de horas do técnico responsável pela vistoria ]**
> Inserir print da tela aqui


1. Acesse o laudo em renovação (seção 3.5) e localize a vistoria em andamento.
1. Registre o Técnico Responsável pela vistoria.
1. Informe o Tempo Gasto na inspeção (horas e minutos).
1. Descreva brevemente o que foi verificado durante a vistoria.
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> As horas ficam vinculadas ao laudo e ao técnico responsável, compondo o custo de mão de obra de conformidade no Módulo Financeiro.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Técnico Responsável | Colaborador que executou a vistoria | Sim | João Silva |
| Tempo Gasto | Horas/minutos dedicados à vistoria | Sim | 1h 30min |
| Descrição da Vistoria | O que foi verificado durante a inspeção | Não | Verificação de pressão e vedação: vasos de pressão |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Registre o apontamento no mesmo dia da vistoria, atraso no lançamento é a principal causa de custo de conformidade subestimado nos relatórios.


> [!DANGER]
> Não confunda este apontamento com o apontamento de horas de uma OS comum (seção 3.1), são custos de naturezas diferentes (conformidade legal vs. manutenção corretiva) e precisam ficar separados nos relatórios.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O apontamento de horas não aparece vinculado ao laudo correto | O laudo não estava com uma renovação em andamento no momento do apontamento | Confirme o status do laudo na seção 3.5 antes de registrar as horas |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Laudo e Documentação Técnica (seção 3.5)Cadastro de Colaboradores (seção 2.9) | Módulo Financeiro: custo de mão de obra de conformidade | Vínculo automático entre horas apontadas e o laudo em renovação |', NULL, NULL, NULL, NULL, 1);

-- Tudo em Dia — Checklist de Conformidade de Locais [CadastroBasico/ChecklistIndex]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'CadastroBasico', N'ChecklistIndex', N'Tudo em Dia — Checklist de Conformidade de Locais', N'Supervisores e Técnicos', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:tecnico') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Técnicos de manutenção e supervisores de PCM: equipe que realiza a vistoria dos locais. | Menu lateral > Tudo em Dia > Checklist pcmbysim.com.br/Tudo/ChecklistTudo (acessível com o módulo Qualidade selecionado no topo da tela). |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de acompanhar e executar a vistoria de conformidade de TODOS os locais da edificação que não são U.H. (áreas comuns, salas técnicas, academia, spa, restaurantes, playground etc.). É a evolução do U.H. em Dia (seção 5.1): mesmo conceito de checklist com periodicidade, mas em vez de vistoriar apartamentos, vistoria os demais espaços do empreendimento, garantindo que tudo permaneça novo e em perfeitas condições. Quando a periodicidade programada vence, é hora da equipe ir ao local, confirmar que está tudo em ordem e, se necessário, abrir uma Ordem de Serviço para corrigir uma não conformidade.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> O checklist com Tipo de Checklist = Tudo em Dia precisa estar criado em Cadastro Básico > Checklist (seção 2.13) e vinculado ao local. Sem isso, a tela de execução mostra ''Este local não tem itens de checklist configurados''.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Local CADASTRADO | → | 2️⃣ Periodicidade VENCIDA | → | 3️⃣ Status PENDENTE/ATRASADO | → | 4️⃣ Vistoria REALIZADA | → | 5️⃣ Não Conforme? OS ABERTA | → | 📊 Local em Dia |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'### 4.1  Visão geral, KPIs e locais por setor

![Tudo em Dia > Checklist — KPIs de status e locais agrupados por setor/andar](/screenshots/tudoemdia-checklist-locais.png)


1. Acesse Tudo em Dia > Checklist.
1. Selecione a Unidade e, se quiser, filtre por Setor — o agrupamento real de ''Setor'' usa os andares/áreas do prédio (ex.: ''01º Andar'', ''Área Externa'', ''Cobertura'', ''Mezanino'', ''Térreo/Cozinha''), não departamentos como Cozinha/Lavanderia isoladamente.
1. Confira os 5 indicadores no topo, Atrasado, Pendente, Em Manutenção, Nova Vistoria e Em Dia, cada um mostrando a quantidade de locais naquele status.
1. Na lista ''Locais por Setor'', use os ícones no canto superior direito para alternar entre visão em grade (cards) e visão em lista.


> [!INFO]
> **IMPORTANTE SABER**
> Cada card de local mostra a data da última vistoria (ou ''Sem data'' se nunca foi vistoriado), a Periodicidade configurada (ex.: ''MÊS'') e o Status atual. O Status é calculado automaticamente pelo sistema a partir da data e da periodicidade, o mesmo princípio já visto em Dedetização (seção 5.9).


### 4.2  Executar a vistoria de um local

1. Na lista de locais, clique no card do local que deseja vistoriar.
1. A tela de Execução abre com: Local, Setor, Responsável, Data Início, Hora Início, Data Término, Hora Término e a Periodicidade configurada (ex.: ''MÊS · 1'').
1. Responda os itens do checklist vinculado ao local (formato de resposta segue a mesma estrutura de Rotina/Preventiva/Governança documentada na seção 2.13).
1. Para itens Não Conformes, o checklist pode abrir uma Ordem de Serviço automaticamente, se o item estiver configurado com Gera Ordem de Serviço = SIM (seção 2.13), comportamento herdado do cadastro do checklist.
1. Ao concluir, salve o apontamento.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade/marca à qual o local pertence | Sim | Hotel by SIM Services |
| Setor | Andar ou área do prédio onde o local está | Sim | 01º Andar |
| Local | Nome do espaço vistoriado | Sim | SPA, Academia, Restaurante Verde |
| Status | ATRASADO / PENDENTE / EM MANUTENÇÃO / NOVA VISTORIA / EM DIA: calculado automaticamente | Auto | PENDENTE |
| Periodicidade | Ciclo de retorno da vistoria daquele local | Sim | MÊS · 1 |
| Responsável | Quem executou a vistoria | Sim | Técnico designado |
| Data/Hora Início e Término | Janela de execução da vistoria | Sim | — |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Cadastre o checklist Tudo em Dia (seção 2.13) e vincule os locais antes de anunciar a ferramenta para a equipe, um local sem itens configurados não pode ser vistoriado de fato.
> Priorize a vistoria dos locais Atrasados antes dos Pendentes, eles já passaram do prazo da periodicidade configurada.


> [!DANGER]
> Assim como em Dedetização (seção 5.9), um local Em Dia hoje pode voltar automaticamente para Pendente quando o próximo ciclo da Periodicidade vencer, isso é esperado, não um bug.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| A tela de execução mostra ''Este local não tem itens de checklist configurados'' | O checklist Tipo = Tudo em Dia não foi criado ou não foi vinculado a este local | Acesse Cadastro Básico > Checklist (seção 2.13), crie o checklist com Tipo = Tudo em Dia e vincule os itens |
| Um local não aparece na listagem | Local de outra Unidade selecionada, ou filtro de Setor ativo escondendo o resultado | Confira a Unidade selecionada no topo e limpe o filtro de Setor |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Cadastro Básico > Checklist, Tipo = Tudo em Dia (seção 2.13) | Ordens de Serviço: abertura automática para itens Não Conformes, quando configurado | Status recalculado automaticamente a cada novo ciclo da Periodicidade |', NULL, NULL, NULL, NULL, 1);

-- LogBook — Livro de Ocorrencias [LogBook/LogBook]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'S', N'LogBook', N'LogBook', N'LogBook — Livro de Ocorrencias', N'Supervisores e Técnicos', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:tecnico') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Todos os colaboradores com acesso ao sistema. Registro diário de ocorrencias operacionais. | Menu lateral > Log Book pcmbysim.com.br/LogBook/LogBook |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de registrar e consultar ocorrencias operacionais no livro digital de passagem de turno. O LogBook e o substituto digital do caderno de ocorrencias físico: nele são registrados eventos que não geram OS mas que precisam ser comunicados entre turnos, visita tecnica, comunicado de fornecedor, problema resolvido informalmente, observacao sobre equipamento. Um LogBook bem preenchido garante que informações criticas não se percam na transicao entre equipes.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> Qualquer colaborador com acesso ao sistema pode registrar ocorrências no LogBook.
> Defina com a equipe o critério claro do que vai para o LogBook e o que gera uma OS.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Evento IDENTIFICADO | → | 2️⃣ Registro ABERTO | → | 3️⃣ Ocorrência DESCRITA | → | 4️⃣ Registro SALVO | → | 📊 Próximo Turno Lê |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![LogBook > Nova Entrada — campo de data, turno, descricao e setor](/screenshots/logbook-nova-entrada.png)


1. Acesse Log Book no menu lateral.
1. Clique em Inserir
1. Preencha só 2 campos: Unidade e o texto do registro no campo único Log. Não se preocupe em inserir data e hota, essas informações são preenchidas automaticamente junto com o autor que é o usuário logado, também inserido automático pelo sistema.
1. Dentro do texto do Log, inclua o que aconteceu de forma clara e objetiva: Quem, o que, quando, onde e qual foi o desfecho do dia. Como não há campos de Turno e Setor, escreva-os no início do texto se forem relevantes (ex.: Turno Noite, Cozinha Industrial: ...).
1. Clique em Salvar.


> [!INFO]
> **RESULTADO ESPERADO**
> Ocorrencia registrada e disponível para consulta pelo próximo turno e pela gestão. Todos que efetuarem login no sistema após a gravação do formulário do Logbook verão a informação na tela principal.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Unidade | Unidade onde a ocorrência foi registrada | Sim | PCM by SIM |
| Log | Campo único de texto livre: é aqui que entra tudo: o que aconteceu, e também data/turno/setor se você quiser registrar, já que não existem campos separados para eles | Sim | Texto livre |
| Data | Preenchida automaticamente pelo sistema: não é um campo editável | Auto | 04/07/2026 |
| Autor | Preenchido automaticamente com o usuário logado: não é um campo editável | Auto | João Silva |


| Registrar no LogBook | Não registrar no LogBook (use OS) |
| :--- | :--- |
| Visita tecnica de fornecedor: resultado da visita | Reparo realizado em equipamento: use OS |
| Comunicado da recepcao sobre quarto específico | Defeito identificado em equipamento: use OS |
| Problema resolvido informalmente durante o turno | Manutenção preventiva realizada: use Preventiva |
| Observacao sobre comportamento de equipamento | Requisição de compra: use módulo Requisição |
| Registro de passagem de turno com pendencias | Auditoria de qualidade: use módulo Auditoria |
| Evento extraordinary: falta de energia, chuva forte, alagamento |  |


### 4.1 Consultar o histórico

1. Na tela de LogBook, use os filtros reais: Unidade e Período (De/Até), não existem filtros de Turno ou Setor, já que esses dados não são campos separados (ver nota acima).
1. Clique em Filtrar para ver os registros do período.
1. Clique em qualquer registro para ver o detalhe completo.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Inicie todo turno lendo os registros do turno anterior, e um habito que evita retrabalho e surpresas operacionais.
> Seja objetivo e factual: ''Bomba de recalque B2 apresentou ruido anormal por 3 minutos as 14h32, parou sozinha sem intervencao. Monitorando.'' e muito melhor do que ''Bomba com ruido''.
> Para eventos criticos, registre tambem a hora exata, o nome de quem percebeu e as acoes tomadas, o LogBook pode ser exigido em pericias tecnicas.


> [!DANGER]
> O LogBook não substitui a OS para intervencoes tecnicas. Um reparo registrado apenas no LogBook não entra nos custos, não consta no histórico do equipamento e não compoe os indicadores de manutenção.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| O registro do turno anterior não aparece no LogBook | O filtro de turno ou data está restringindo a visualização | Remova todos os filtros e aplique apenas o filtro de Unidade para ver todos os registros recentes |
| Não consigo editar um registro já salvo | O LogBook e por design imutavel após o salvamento: garante integridade do histórico | Se houver erro de informação, crie um novo registro corretivo indicando que cancela o anterior |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Qualquer colaborador com acesso ao sistemaEventos operacionais do turno | Gestão de continuidade: histórico de eventos por unidade Investigacoes tecnicas: linha do tempo de ocorrencias Reunião de passagem de turno | Registro imutavel: histórico permanente para auditoria |', NULL, NULL, NULL, NULL, 1);

-- Módulo Excel — Extração e Análise Avançada [processo]
INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)
VALUES (NULL, N'P', NULL, NULL, N'Módulo Excel — Extração e Análise Avançada', N'Gestor de PCM', 1, N'supabase', GETDATE());
SET @codigo = SCOPE_IDENTITY();
UPDATE tb_manual SET codigo_manual_processo = (SELECT codigo FROM @trilha WHERE chave = N'trilha:gestor') WHERE codigo = @codigo;
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 1, N'Visão geral', N'| Para quem é esta seção? | Onde está no sistema? |
| :--- | :--- |
| Gestores de PCM, analistas de operações e diretores. | Menu lateral > Relatorio > Excel / BI pcmbysim.com.br/Relatorio/Excel |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 2, N'🎯 O que você vai conquistar', N'Ao dominar esta seção você será capaz de exportar bases de dados estruturadas do sistema diretamente para Excel, permitindo análises avancadas que vao alem dos relatorios padrão, tabelas dinamicas, gráficos personalizados, cruza de dados entre módulos e construcao de paineis de BI personalizados. O Módulo Excel e a ponte entre o PCM by SIM e as ferramentas de análise que o gestor já conhece.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 3, N'📋 Antes de começar', N'> [!WARNING]
> Com esta ferramenta você será capaz de realizar a estração de Ordem de Serviço e Planos de Ação diretamente compativeis para o excel.


| Exportação | O que contem | Principal uso analitico |
| :--- | :--- | :--- |
| OS Completo | Todas as OS do período: data, categoria, setor, executor, equipamento, custo e status | Tabela dinâmica de custo por categoria, por setor e por equipamento |
| Plano de Acao | Todos os Planos de Acao: descricao, responsável, prazo, % execução e status | Acompanhamento de fechamento de não conformidades em reunião de diretoria |
| Preventiva x Equipamento | Histórico de execucoes de preventiva por equipamento no período | Análise de cumprimento do plano e vida util dos ativos |
| Estoque e Movimentacao | Saldo atual, entradas, saidas e custo médio de cada produto | Análise de giro de estoque e custo de material por OS |
| Colaboradores x Horas | Horas apontadas e custo por colaborador no período | Cálculo de custo de mao de obra total e por especialidade |
| Green Planet | Medicoes diarias de todos os medidores com KPIs calculados | Gráficos de tendencia de consumo e análise de sazonalidade |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 4, N'🧭 Visão do Fluxo', N'| 1️⃣ Módulo SELECIONADO | → | 2️⃣ Filtros APLICADOS | → | 3️⃣ Exportação .XLSX | → | 4️⃣ Abertura no EXCEL | → | 📊 Análise e Gráficos |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 5, N'🚀 Passo a Passo', N'![Relatorio > Excel — seleção de módulo e filtros para exportação](/screenshots/excel-modulo.png)


1. Não existe uma tela única Relatório > Excel com seletor de módulo. O menu real Excel tem só 2 telas separadas: Excel > Ordem de Serviço (/Excel/OrdemServico) e Excel > Plano de Ação (/Excel/PlanoAcaoQA) — não existem exportações Excel dedicadas para Preventiva, Estoque, Colaboradores ou Green Planet neste menu.
1. Acesse a tela específica do que deseja exportar (Ordem de Serviço ou Plano de Ação) e aplique os Filtros disponíveis ali.
1. Clique em Exportar.
1. O arquivo .xlsx e gerado e baixado automaticamente.
1. Separadamente, existe um menu Upload (Upload > Cadastro Básico e Upload > PMOC) para importar dados via planilha, direção oposta à exportação.


> [!INFO]
> **RESULTADO ESPERADO**
> Base estruturada pronta para análise em Excel, cada linha e um registro, cada coluna e um campo do sistema.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 6, N'🔍 Tabela de Campos para Consulta Rápida', N'| Campo | Descrição | Obrigatório? | Exemplo / Regra |
| :--- | :--- | :--- | :--- |
| Módulo | Base de dados a ser exportada | Sim | OS, Preventiva, Estoque |
| Unidade | Filtro de uma ou todas as unidades | Sim | Todas as unidades |
| Período | Intervalo de datas da exportação | Sim | 01/06/2025 a 30/06/2025 |
| Categoria | Filtro adicional conforme o módulo exportado | Não | Elétrica |


| Análise | Módulo exportado | Como montar no Excel |
| :--- | :--- | :--- |
| Top 10 equipamentos com mais OS | OS Completo | Tabela dinâmica: equipamento na linha, contagem de OS no valor |
| Custo de manutenção por mes | OS Completo | Tabela dinâmica: mes na coluna, soma de custo no valor: gráfico de linha |
| Cumprimento do plano preventivo | Preventiva | CONT.SE de status=''Concluído'' / CONT.SE de status=''Pendente'' |
| Giro de estoque por produto | Estoque | Saidas / Saldo médio: identifica itens parados em estoque |
| Consumo energetico por hóspede | Green Planet | Consumo kwh / Qtde Hóspedes: gráfico de tendencia mensal |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 7, N'💡 Boas Práticas e Pontos de Atenção', N'> [!BOAPRATICA]
> Não modifique os dados exportados diretamente, crie uma aba de ''Análise'' separada e use fórmulas que referenciem a aba de dados brutos. Assim você pode atualizar a base mensalmente sem perder as fórmulas.
> Salve as exportacoes com nome padronizado: ''OS_[Unidade]_[AAAAMM].xlsx''. Facilita a localização e a construcao de relatorios anuais.
> Para apresentacoes de diretoria, use o Power BI conectado ao Excel exportado, os gráficos ficam mais profissionais e a atualizacao mensal e simples.


> [!DANGER]
> Não compartilhe a planilha exportada como fonte única de verdade, ela reflete o momento da exportação. Para decisões recorrentes, sempre reexporte antes de apresentar os números.', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 8, N'⚠️ Problemas Comuns e Soluções', N'| Sintoma / Problema | Causa provável | O que fazer |
| :--- | :--- | :--- |
| Arquivo exportado vazio ou com poucos registros | Filtro muito restrito ou período sem registros | Remova todos os filtros, aplique apenas Unidade e o período desejado |
| Fórmulas do Excel param após atualizar os dados | Estrutura de colunas mudou entre exportações | Mantenha aba de dados brutos separada da aba de análise: nunca modifique a aba de dados diretamente |', NULL, NULL, NULL, NULL, 1);
INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
VALUES (@codigo, 9, N'🔗 Conexões com Outros Módulos', N'| O que alimenta este módulo | O que este módulo alimenta | Integrações automáticas |
| :--- | :--- | :--- |
| Todos os módulos operacionais do período selecionado | Ferramentas externas de BI (Power BI, Looker, Google Data Studio)Apresentacoes de diretoriaRelatorio Mensal personalizado | Exportação sob demanda: sempre reflete os dados mais recentes do sistema |', NULL, NULL, NULL, NULL, 1);

COMMIT TRANSACTION;

SELECT manuais = (SELECT COUNT(*) FROM tb_manual WHERE usuario = N'supabase'),
       secoes  = (SELECT COUNT(*) FROM tb_manual_item i
                  JOIN tb_manual m ON m.codigo = i.codigo_manual WHERE m.usuario = N'supabase');