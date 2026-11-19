# ProgramaCEDESP

Sistema desktop em C# (.NET Framework 4.7.2) com servidor local, desenvolvido por iniciativa própria em 2024 e em uso contínuo desde fevereiro de 2025 para organizar a rotina das turmas e o acompanhamento dos conviventes sem custos adicionais de hospedagem ou manutenção.

## Funcionalidades

- Registro e acompanhamento de faltas, notas e atividades ao longo do semestre.
- Consulta individual de desempenho pelos conviventes.
- Acesso direto à pasta pessoal / de materiais de cada convivente em servidor local.
- Acesso rápido à transmissão ao vivo das aulas, permitindo acompanhamento tanto na tela projetada quanto nos computadores.
- Área administrativa para geração de relatório final (média e faltas) e limpeza dos registros ao encerrar o semestre, com regra para cadastrar novo administrador apenas quando não houver administrador ativo.

## Estrutura e execução

1. Abra o arquivo `ProgramaCEDESP.sln` no Visual Studio.
2. Restaure os pacotes NuGet caso necessário (Newtonsoft.Json, iTextSharp, BouncyCastle) e compile/execute o projeto.
3. Os dados reais (dadosAlunos.json), relatórios gerados e pastas de compilação (in/, obj/, .vs/, packages/) ficam fora do versionamento via .gitignore.
