using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.ValueObjects;

namespace Fixon.Tests.MockRepository
{
    static class TicketsMock
    {
        public static List<Ticket> Tickets { get; set; } = new List<Ticket>
            {
                new Ticket(
                    title: "Erro ao acessar painel administrativo",
                    description: "Usuário não consegue acessar o painel administrativo. Apresenta erro 500.",
                    category: "Acesso",
                    departament: "TI",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "João Souza" },
                    priority: EPriority.High.ToString()),

                new Ticket(
                    title: "Impressora não funciona",
                    description: "Impressora do setor financeiro não imprime relatórios.",
                    category: "Hardware",
                    departament: "Financeiro",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Carlos Alberto" },
                    priority: EPriority.Medium.ToString()),

                new Ticket(
                    title: "Sistema de estoque lento",
                    description: "O sistema de estoque está demorando mais de 30 segundos para abrir relatórios.",
                    category: "Performance",
                    departament: "Logística",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Fernanda Ribeiro" },
                    priority: EPriority.High.ToString()),

                new Ticket(
                    title: "Falha na autenticação de e-mails",
                    description: "E-mails corporativos não estão autenticando corretamente.",
                    category: "Rede",
                    departament: "Suporte",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Lucas Ferreira" },
                    priority: EPriority.High.ToString()),

                new Ticket(
                    title: "Solicitação de acesso ao sistema de relatórios",
                    description: "Novo colaborador precisa de acesso ao sistema de relatórios.",
                    category: "Acesso",
                    departament: "RH",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Patrícia Gomes" },
                    priority: EPriority.Low.ToString()),

                new Ticket(
                    title: "Solicitação de novo equipamento",
                    description: "Pedido de um novo notebook para o gerente de vendas.",
                    category: "Hardware",
                    departament: "Vendas",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Marcos Pereira" },
                    priority: EPriority.Medium.ToString()),

                new Ticket(
                    title: "Erro ao gerar relatório financeiro",
                    description: "Sistema apresenta erro 404 ao tentar gerar relatórios financeiros mensais.",
                    category: "Relatórios",
                    departament: "Financeiro",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Rafael Martins" },
                    priority: EPriority.High.ToString()),

                new Ticket(
                    title: "Acesso negado ao portal de RH",
                    description: "Colaboradores não conseguem acessar documentos de folha de pagamento.",
                    category: "Acesso",
                    departament: "RH",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Juliana Alves" },
                    priority: EPriority.Medium.ToString()),

                new Ticket(
                    title: "Sistema de ponto fora do ar",
                    description: "Sistema de registro de ponto não está respondendo.",
                    category: "Sistema",
                    departament: "RH",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Diego Lima" },
                    priority: EPriority.High.ToString()),

                new Ticket(
                    title: "Computador com tela azul",
                    description: "Estação de trabalho do setor jurídico apresenta tela azul constantemente.",
                    category: "Hardware",
                    departament: "Jurídico",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Larissa Ferreira" },
                    priority: EPriority.High.ToString()),

                new Ticket(
                    title: "Solicitação de instalação de software de design",
                    description: "Equipe de marketing solicitou instalação do Photoshop.",
                    category: "Software",
                    departament: "Marketing",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Caio Souza" },
                    priority: EPriority.Low.ToString()),

                new Ticket(
                    title: "Telefone corporativo sem sinal",
                    description: "Telefone IP do gerente comercial está sem sinal.",
                    category: "Rede",
                    departament: "Comercial",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Priscila Duarte" },
                    priority: EPriority.Medium.ToString()),

                new Ticket(
                    title: "Problema ao enviar e-mails externos",
                    description: "E-mails para domínios externos estão retornando com erro de autenticação.",
                    category: "E-mail",
                    departament: "TI",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "André Torres" },
                    priority: EPriority.High.ToString()),

                new Ticket(
                    title: "Lentidão na rede interna",
                    description: "Rede interna muito lenta durante o expediente.",
                    category: "Rede",
                    departament: "Operações",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Renata Barros" },
                    priority: EPriority.High.ToString()),

                new Ticket(
                    title: "Impressora sem driver atualizado",
                    description: "Impressora do setor de contratos não imprime documentos grandes.",
                    category: "Hardware",
                    departament: "Jurídico",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Eduardo Pires" },
                    priority: EPriority.Medium.ToString()),

                new Ticket(
                    title: "Falha na integração com sistema de contabilidade",
                    description: "Sistema ERP não sincroniza com o módulo contábil.",
                    category: "Sistema",
                    departament: "Financeiro",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Fernanda Lima" },
                    priority: EPriority.High.ToString()),

                new Ticket(
                    title: "Solicitação de acesso ao drive compartilhado",
                    description: "Novo colaborador precisa acessar pastas compartilhadas do projeto X.",
                    category: "Acesso",
                    departament: "Projetos",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Thiago Rezende" },
                    priority: EPriority.Low.ToString()),

                new Ticket(
                    title: "Teclado não funciona",
                    description: "Teclado da estação de trabalho não responde às teclas.",
                    category: "Hardware",
                    departament: "Atendimento",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Bruna Rocha" },
                    priority: EPriority.Medium.ToString()),

                new Ticket(
                    title: "Aplicativo mobile trava ao abrir relatórios",
                    description: "Versão mobile do sistema de vendas trava ao acessar relatórios diários.",
                    category: "Mobile",
                    departament: "Vendas",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Fábio Nascimento" },
                    priority: EPriority.High.ToString()),

                new Ticket(
                    title: "Monitor piscando constantemente",
                    description: "Monitor do setor administrativo está com falhas na exibição.",
                    category: "Hardware",
                    departament: "Administrativo",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Juliana Pereira" },
                    priority: EPriority.Medium.ToString()),

                new Ticket(
                    title: "Problema na autenticação de VPN",
                    description: "Usuários remotos não conseguem conectar via VPN.",
                    category: "Rede",
                    departament: "TI",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Marcos Vinicius" },
                    priority: EPriority.High.ToString()),

                new Ticket(
                    title: "Erro ao exportar dados em CSV",
                    description: "Sistema de relatórios não exporta dados no formato CSV.",
                    category: "Relatórios",
                    departament: "Operações",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Aline Costa" },
                    priority: EPriority.Medium.ToString()),

                new Ticket(
                    title: "Atualização de firmware pendente",
                    description: "Roteador principal com atualização de firmware atrasada.",
                    category: "Rede",
                    departament: "TI",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Pedro Moura" },
                    priority: EPriority.Medium.ToString()),

                new Ticket(
                    title: "Solicitação de licença para software de engenharia",
                    description: "Engenheiro civil solicitou licença para software AutoCAD.",
                    category: "Software",
                    departament: "Engenharia",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Camila Ribeiro" },
                    priority: EPriority.Low.ToString()),

                new Ticket(
                    title: "Falha no backup diário",
                    description: "Backup automático do servidor não está sendo executado.",
                    category: "Servidor",
                    departament: "TI",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Henrique Borges" },
                    priority: EPriority.High.ToString()),

                new Ticket(
                    title: "Configuração incorreta de proxy",
                    description: "Navegadores não acessam sites externos devido a proxy incorreto.",
                    category: "Rede",
                    departament: "TI",
                    createdByUser: new User { UserId = Guid.NewGuid().ToString(), UserName = "Beatriz Souza" },
                    priority: EPriority.High.ToString())
            };

        public static List<Analyst> Analysts = new List<Analyst>
            {
                new Analyst
                {
                    AnalystId = Guid.NewGuid().ToString(),
                    AnalystName = "Ana Carolina Souza"
                },
                new Analyst
                {
                    AnalystId = Guid.NewGuid().ToString(),
                    AnalystName = "Bruno Henrique Almeida"
                },
                new Analyst
                {
                    AnalystId = Guid.NewGuid().ToString(),
                    AnalystName = "Carla Fernanda Ribeiro"
                },
                new Analyst
                {
                    AnalystId = Guid.NewGuid().ToString(),
                    AnalystName = "Diego Luiz Martins"
                },
                new Analyst
                {
                    AnalystId = Guid.NewGuid().ToString(),
                    AnalystName = "Elaine Cristina Rocha"
                }
            };

        public static List<Ticket> GetTickets()
        {
            var random = new Random();

            foreach (var ticket in Tickets)
            {
                var analyst = Analysts[random.Next(Analysts.Count)];

                ticket.AssignTicketToAnalyst(analyst);
            }

            foreach (var ticket in Tickets)
            {
                if (random.NextDouble() > 0.5)
                {
                    ticket.ResolveTicket();
                }
            }

            return Tickets;
        }
    }
}
