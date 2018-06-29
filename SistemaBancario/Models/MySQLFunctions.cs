﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Dapper;

namespace SistemaBancario.Models
{
    class MySQLFunctions
    {
        //Conexa com o banco de dados remoto online
        static private MySqlConnection connection = new MySqlConnection("SERVER=db4free.net;PORT=3306;DATABASE=sistemabancario;UID=bancario;PWD=sb100001;");
        static string treatment = "Sem alteração";

        //Criar novo usuario no banco de dados
        static public Boolean InserirUsuario(string primeiroNome, string sobrenome, string cpf, string rg)
        {
            Boolean sucesso;

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlCommand inserirUsuario = new MySqlCommand("INSERT INTO Usuario(primeiroNome, sobrenome, cpf, rg) VALUES(@primeiroNome, @sobrenome, @cpf, @rg)", connection);
                inserirUsuario.Parameters.AddWithValue("@primeiroNome", primeiroNome);
                inserirUsuario.Parameters.AddWithValue("@sobrenome", sobrenome);
                inserirUsuario.Parameters.AddWithValue("@cpf", cpf);
                inserirUsuario.Parameters.AddWithValue("@rg", rg);

                inserirUsuario.ExecuteNonQuery();
                inserirUsuario.Parameters.Clear();

                sucesso = true;
            }
            catch (MySqlException exception)
            {
                sucesso = false;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }

            return sucesso;
        }

        //Criar novo endereco no banco de dados
        static public Boolean InserirEndereco(string tipo, string logradouro, int numero, string bairro, string complemento, string cep, string cidade, string estado)
        {
            Boolean sucesso;

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlCommand inserirEndereco = new MySqlCommand(
                    "INSERT INTO Endereco(tipo, logradouro, numero, bairro, complemento, cep, cidade, estado_id) VALUES(@tipo, @logradouro, @numero, @bairro, @complemento, @cep, @cidade, @estado)", connection);
                inserirEndereco.Parameters.AddWithValue("@tipo", tipo);
                inserirEndereco.Parameters.AddWithValue("@logradouro", logradouro);
                inserirEndereco.Parameters.AddWithValue("@numero", numero);
                inserirEndereco.Parameters.AddWithValue("@bairro", bairro);
                inserirEndereco.Parameters.AddWithValue("@complemento", complemento);
                inserirEndereco.Parameters.AddWithValue("@cep", cep);
                inserirEndereco.Parameters.AddWithValue("@cidade", cidade);
                inserirEndereco.Parameters.AddWithValue("@estado", estado);

                inserirEndereco.ExecuteNonQuery();
                inserirEndereco.Parameters.Clear();

                sucesso = true;
            }
            catch (MySqlException exception)
            {
                sucesso = false;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }

            return sucesso;
        }
       
        //Criar novo cliente no banco de dados
        static public Boolean InserirCliente(string dataNascimento, string email, string telefone, string celular, string dataCadastro, string estado, string estadoCivil, string cep, string cpf)
        {
            Boolean sucesso;

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlCommand inserirCliente = new MySqlCommand(
                    "INSERT INTO Cliente(data_nascimento, email, telefone, celular, data_cadastro, id_endereco, status, estado_civil, id_usuario) VALUES(STR_TO_DATE(@dataNascimento, \"%d/%m/%Y\"), @email, @telefone, @celular, STR_TO_DATE(@dataCadastro, \"%d/%m/%Y\"), (SELECT id FROM Endereco WHERE cep = @cep), @estado, @estadoCivil, (SELECT id FROM Usuario WHERE cpf = @cpf))", connection);
                inserirCliente.Parameters.AddWithValue("@dataNascimento", dataNascimento);
                inserirCliente.Parameters.AddWithValue("@email", email);
                inserirCliente.Parameters.AddWithValue("@telefone", telefone);
                inserirCliente.Parameters.AddWithValue("@celular", celular);
                inserirCliente.Parameters.AddWithValue("@dataCadastro", dataCadastro);
                inserirCliente.Parameters.AddWithValue("@estado", estado);
                inserirCliente.Parameters.AddWithValue("@estadoCivil", estadoCivil);
                inserirCliente.Parameters.AddWithValue("@cep", cep);
                inserirCliente.Parameters.AddWithValue("@cpf", cpf);

                inserirCliente.ExecuteNonQuery();
                inserirCliente.Parameters.Clear();

                sucesso = true;
            }
            catch (MySqlException exception)
            {
                sucesso = false;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }
            return sucesso;
        }
      
        //Criar novo Titular Pessoa Fisica
        static public Boolean InserirTitularPessoaFisica(string profissao, decimal rendaMensal, string email)
        {
            Boolean sucesso;
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlCommand inserirTitularPessoaFisica = new MySqlCommand(
                    "INSERT INTO PessoaFisica(profissao, rendaMensal, id_cliente) VALUES(@profissao, @rendaMensal, (SELECT id FROM Cliente WHERE email = @email))", connection);

                inserirTitularPessoaFisica.Parameters.AddWithValue("@profissao", profissao);
                inserirTitularPessoaFisica.Parameters.AddWithValue("@rendaMensal", rendaMensal);
                inserirTitularPessoaFisica.Parameters.AddWithValue("@email", email);

                inserirTitularPessoaFisica.ExecuteNonQuery();
                inserirTitularPessoaFisica.Parameters.Clear();

                sucesso = true;
            }
            catch (MySqlException exception)
            {
                sucesso = false;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }
            return sucesso;
        }

        static public bool SelecionarCliente(string agencia, string conta)
        {
            bool sucesso = false;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT Conta.id, Agencia.id FROM Conta JOIN Agencia ON Conta.id_agencia = Agencia.id AND Conta.numero= @conta AND Agencia.numero= @agencia;", connection);
                command.Parameters.AddWithValue("@conta", conta);
                command.Parameters.AddWithValue("@agencia", agencia);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        treatment = reader[0].ToString();

                    }
                    reader.Close();
                    if (treatment != "Sem alteração") sucesso = true;
                    else sucesso = false;
                }
            }
            catch (MySqlException exception)
            {
                sucesso = false;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }
            return sucesso;
        }

        //Criar novo Titular Pessoa Juridica
        static public Boolean InserirTitularPessoaJuridica(string cnpj, string razaoSocial, string tipoPessoaJuridica, string email)
        {
            Boolean sucesso;

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlCommand inserirTitularPessoaJuridica = new MySqlCommand(
                    "INSERT INTO PessoaJuridica(cnpj, razaoSocial, tipo, id_cliente) VALUES(@cnpj, @razaoSocial, @tipo, (SELECT id FROM Cliente WHERE email = @email))", connection);

                inserirTitularPessoaJuridica.Parameters.AddWithValue("@cnpj", cnpj);
                inserirTitularPessoaJuridica.Parameters.AddWithValue("@razaoSocial", razaoSocial);
                inserirTitularPessoaJuridica.Parameters.AddWithValue("@tipo", tipoPessoaJuridica);
                inserirTitularPessoaJuridica.Parameters.AddWithValue("@email", email);

                inserirTitularPessoaJuridica.ExecuteNonQuery();
                inserirTitularPessoaJuridica.Parameters.Clear();

                sucesso = true;
            }
            catch (MySqlException exception)
            {
                sucesso = false;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }

            return sucesso;
        }

        //Criar novo Dependente - VERIFICAR NO BANCO DE DADOS AS COLUNAS
        static public Boolean InserirDependente(string cpfResponsavel, string email)
        {
            Boolean sucesso;

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlCommand inserirDependente = new MySqlCommand(
                    "INSERT INTO Dependente (id_titular, id_cliente) VALUES((SELECT PessoaFisica.id FROM PessoaFisica JOIN Cliente ON PessoaFisica.id_cliente = Cliente.id JOIN Usuario ON Cliente.id_usuario = Usuario.id WHERE Usuario.cpf = @cpfResponsavel), (SELECT id FROM Cliente WHERE email = @email))", connection);

                inserirDependente.Parameters.AddWithValue("@cpfResponsavel", cpfResponsavel);
                inserirDependente.Parameters.AddWithValue("@email", email);

                inserirDependente.ExecuteNonQuery();
                inserirDependente.Parameters.Clear();

                sucesso = true;
            }
            catch (MySqlException exception)
            {
                sucesso = false;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }

            return sucesso;
        }


        //Funcao para conferir se uma dada string contem apenasdigitos
        public static bool ApenasDigitos(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }

        //Funcao para conferir se uma dada string contem apenas letras
        public static bool ApenasLetras(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsLetter(c))
                    return false;
            }
            return true;
        }

        //Exibir resultado da busca por um cliente
        static public Boolean BuscarCliente(DataGridView dataGridView, string identificador)
        {
            bool sucesso;

            string query = "";
            string parametro = "";

            if (!ApenasDigitos(identificador) && !ApenasLetras(identificador)) //Se ambas funcoes forem falsas, o identificador esta incorreto (nem nome nem cpf)
            {
                sucesso = false;
                return sucesso;
            }
            else
            {

                if (ApenasDigitos(identificador)) //Se a string apenas conter digitos, entao a busca sera feita pelo cpf
                {
                    query = "SELECT Cliente.id as 'Identificador', Usuario.primeiroNome as 'Nome', Usuario.cpf as 'CPF', data_nascimento as 'Data de Nascimento', status as 'Status' FROM Cliente, Usuario WHERE Usuario.cpf = @cpf AND Cliente.id_usuario = Usuario.id";
                    parametro = "@cpf";

                }
                else if (ApenasLetras(identificador)) //Se a string apenas conter letras, entao a busca sera feita pelo primeiro nome
                {
                    query = "SELECT Cliente.id as 'Identificador', Usuario.primeiroNome as 'Nome', Usuario.cpf as 'CPF', data_nascimento as 'Data de Nascimento', status as 'Status' FROM Cliente, Usuario WHERE Cliente.id_usuario = ANY (SELECT id FROM Usuario WHERE primeiroNome = @nome) AND Cliente.id_usuario = Usuario.id";
                    parametro = "@nome";
                }

                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    MySqlCommand buscarCliente = new MySqlCommand(query, connection);
                    buscarCliente.Parameters.AddWithValue(parametro, identificador);


                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(buscarCliente);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView.DataSource = dataTable;

                    sucesso = true;
                }
                catch (MySqlException exception)
                {
                    sucesso = false;
                    Console.WriteLine(exception.ToString());
                }
                finally
                {
                    connection.Close();
                }

                return sucesso;
            }
        }

        //Exibir todos os dados de um cliente
        static public Cliente RetornarCliente(int id)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Comando SQL refere-se a chamada de procedimento no banco para determinal qual tipo de cliente
                MySqlCommand determinarTipoCliente = new MySqlCommand("ID_TIPO_CLIENTE", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Informando o valor do parametro de entrada do procedimento (id cliente)
                determinarTipoCliente.Parameters.AddWithValue("@idCli", id);

                //Adicionado os parametros de saida do procedimento
                determinarTipoCliente.Parameters.Add("@t1", MySqlDbType.Int16).Direction = ParameterDirection.Output;
                determinarTipoCliente.Parameters.Add("@t2", MySqlDbType.Text).Direction = ParameterDirection.Output;

                //Execucao da query
                determinarTipoCliente.ExecuteNonQuery();

                //Obtendo retorno do procedimento (Tipo de cliente [Dependente, PessoaFisica ou PessoaJuridica] e seu id)
                string tipoCliente = determinarTipoCliente.Parameters["@t2"].Value.ToString();
                int idCli = int.Parse(determinarTipoCliente.Parameters["@t1"].Value.ToString());
                determinarTipoCliente.Parameters.Clear();

                //Outro comando SQL para retornar os dados do cliente
                string query = "";
                string parametro = "";

                //Determina a consulta adequada para retornar TODOS os dados de cada tipo de cliente diferente 
                if (tipoCliente == "Dependente")
                {
                    query = "SELECT Usuario.primeiroNome, Usuario.sobrenome, Usuario.cpf, Usuario.rg, Cliente.data_nascimento, Cliente.email, Cliente.telefone, Cliente.celular, Cliente.data_cadastro, Cliente.status, Cliente.estado_civil," +
                        " Dependente.id_titular FROM Dependente JOIN Cliente ON Dependente.id_cliente = Cliente.id JOIN Usuario ON Cliente.id_usuario = Usuario.id WHERE Dependente.id = @idDependente";

                    Dependente dependente = RetornarDependente(query, idCli);

                    return dependente;

                }
                else if (tipoCliente == "PessoaFisica")
                {
                    query = "SELECT Usuario.primeiroNome, Usuario.sobrenome, Usuario.cpf, Usuario.rg, Cliente.data_nascimento, Cliente.email, Cliente.telefone, Cliente.celular, Cliente.data_cadastro, Cliente.status, Cliente.estado_civil, " +
                        "PessoaFisica.profissao, PessoaFisica.rendaMensal FROM PessoaFisica JOIN Cliente ON PessoaFisica.id_cliente = Cliente.id JOIN Usuario ON Cliente.id_usuario = Usuario.id WHERE PessoaFisica.id = @idPessoaFisica;";

                    PessoaFisica pessoaFisica = RetornarPessoaFisica(query, idCli);

                    return pessoaFisica;

                }
                else if (tipoCliente == "PessoaJuridica")
                {
                    query = "SELECT Usuario.primeiroNome, Usuario.sobrenome, Usuario.cpf, Usuario.rg, Cliente.data_nascimento, Cliente.email, Cliente.telefone, Cliente.celular, Cliente.data_cadastro, Cliente.status, Cliente.estado_civil, " +
                        "PessoaJuridica.cnpj, PessoaJuridica.razaoSocial, PessoaJuridica.tipo FROM PessoaJuridica JOIN Cliente ON PessoaJuridica.id_cliente = Cliente.id JOIN Usuario ON Cliente.id_usuario = Usuario.id WHERE PessoaJuridica.id = @idPessoaJuridica;";

                    PessoaJuridica pessoaJuridica = RetornarPessoaJuridica(query, idCli);

                    return pessoaJuridica;
                }

                //Executa a consulta caso as variaveis da mesma nao sao nulas
                if (query == "" && parametro == "")
                {
                    return null; //erro
                }
            else
            {
                return null; //valor informado para identificador esta incorreto
            }
            }
            catch (MySqlException exception)
            {
                Console.Write(exception.ToString());
                return null; //erro
            }
            finally
            {
                connection.Close();
            }
        }

        //Retorna um objeto dependente de acordo com a query passada e o id deste
        static public Dependente RetornarDependente(string query, int id)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Busca pelo endereco de acordo com o id do dependente
                var queryResultFirst = connection.Query<Endereco>("SELECT Endereco.tipo, Endereco.logradouro, Endereco.numero, Endereco.bairro, Endereco.complemento, Endereco.cep, Endereco.cidade, Endereco.estado FROM Endereco " +
                    "JOIN Cliente on Cliente.id_endereco = Endereco.id JOIN Dependente on Dependente.id_cliente = Cliente.id WHERE Dependente.id = @idDependente;", new { @idDependente = id });

                Endereco endereco = queryResultFirst.First();

                var queryResult = connection.Query<Dependente>(query, new { @idDependente = id });
                Dependente dependente = queryResult.First();

                //Associa o objeto endereco buscado ao objeto dependente criado
                dependente.Endereco = endereco;

                return dependente;
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return null;
            }
            finally
            {
                connection.Close();
            }

        }

        //Retorna um objeto pessoa fisica de acordo com a query passada e o id deste
        static public PessoaFisica RetornarPessoaFisica(string query, int id)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Busca pelo endereco de acordo com o id do dependente
                var queryResultFirst = connection.Query<Endereco>("SELECT Endereco.tipo, Endereco.logradouro, Endereco.numero, Endereco.bairro, Endereco.complemento, Endereco.cep, Endereco.cidade, Endereco.estado FROM Endereco " +
                    "JOIN Cliente on Cliente.id_endereco = Endereco.id JOIN PessoaFisica on PessoaFisica.id_cliente = Cliente.id WHERE PessoaFisica.id = @idPessoaFisica;", new { @idPessoaFisica = id });

                Endereco endereco = queryResultFirst.First();


                var queryResult = connection.Query<PessoaFisica>(query, new { @idPessoaFisica = id });
                PessoaFisica pessoaFisica = queryResult.First();

                pessoaFisica.Endereco = endereco;

                return pessoaFisica;
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        //Retorna um objeto pessoa juridica de acordo com a query passada e o id deste
        static public PessoaJuridica RetornarPessoaJuridica(string query, int id)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Busca pelo endereco de acordo com o id do dependente
                var queryResultFirst = connection.Query<Endereco>("SELECT Endereco.tipo, Endereco.logradouro, Endereco.numero, Endereco.bairro, Endereco.complemento, Endereco.cep, Endereco.cidade, Endereco.estado FROM Endereco " +
                    "JOIN Cliente on Cliente.id_endereco = Endereco.id JOIN PessoaJuridica on PessoaJuridica.id_cliente = Cliente.id WHERE PessoaJuridica.id = @idPessoaJuridica;", new { @idPessoaJuridica = id });

                Endereco endereco = queryResultFirst.First();

                var queryResult = connection.Query<PessoaJuridica>(query, new { @idPessoaJuridica = id });
                PessoaJuridica pessoaJuridica = queryResult.First();

                pessoaJuridica.Endereco = endereco;

                return pessoaJuridica;
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        //Exibir todos os clientes cadastrados no banco de dados
        static public DataTable ListarCliente()
        {
            DataTable listagemCliente = new DataTable();

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter("SELECT Cliente.id, Usuario.primeiroNome, data_nascimento, status FROM Cliente, Usuario WHERE Cliente.id_usuario = Usuario.id", connection);

                //Todos os dados retornados em formato de tabela para variavel dadosCliente
                dataAdapter.Fill(listagemCliente);

            }
            catch (MySqlException exception)
            {
                listagemCliente = null;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }

            return listagemCliente;
        }

        //Inativar um determinado cliente
        static public Boolean InativarCliente(string identificador)
        {
            bool sucesso;

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlCommand removerCliente = new MySqlCommand("UPDATE Cliente JOIN Usuario on Cliente.id_usuario = Usuario.id SET Cliente.status = 'Inativo' WHERE Usuario.cpf = @identificador", connection);
                removerCliente.Parameters.AddWithValue("@identificador", identificador);

                removerCliente.ExecuteNonQuery();
                removerCliente.Parameters.Clear();

                sucesso = true;
            }
            catch (MySqlException exception)
            {
                sucesso = false;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }
            return sucesso;
        }

        //Exibir resultado da busca por uma aplicacao
        static public DataTable BuscarAplicacao(string numeroContaCorrente)
        {
            DataTable buscaAplicacao = new DataTable();

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlCommand buscarAplicacao = new MySqlCommand("SELECT Aplicacao.id, tipoAplicacao, valorInicial, vencimento FROM Aplicacao JOIN ContaCorrente ON Aplicacao.id_contacorrente = ContaCorrente.id JOIN Conta ON ContaCorrente.id_conta = Conta.id WHERE Conta.numero = @numeroCC", connection);
                buscarAplicacao.Parameters.AddWithValue("@numeroCC", numeroContaCorrente);


                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(buscarAplicacao);

                dataAdapter.Fill(buscaAplicacao);
            }
            catch (MySqlException exception)
            {
                buscaAplicacao = null;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }

            return buscaAplicacao;
        }

        //Exibir todas aplicacoes cadastradas no banco de dados
        static public DataTable ListarAplicacao()
        {
            DataTable listagemAplicacao = new DataTable();

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter("SELECT Aplicacao.id, tipoAplicacao, valorInicial, taxaRendimento, vencimento FROM Aplicacao JOIN ContaCorrente ON Aplicacao.id_contacorrente = ContaCorrente.id JOIN Conta ON ContaCorrente.id_conta = Conta.id", connection);

                //Todos os dados retornados em formato de tabela para variavel dadosCliente
                dataAdapter.Fill(listagemAplicacao);

            }
            catch (MySqlException exception)
            {
                listagemAplicacao = null;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }

            return listagemAplicacao;
        }

        //Exibir resultado da busca por uma aplicacao
        static public Aplicacao RetornarAplicacao(int id)
       {
            Aplicacao aplicacao = new Aplicacao();
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Recupera o id de conta corrente
                var idContaCorrente = connection.ExecuteScalar<int>("SELECT id_contacorrente FROM Aplicacao WHERE Aplicacao.id = @id", new { @id = id });

                ContaCorrente contaCorrente = RetornarContaCorrente(idContaCorrente);

                var queryResult = connection.Query<Aplicacao>("SELECT tipoAplicacao, status, valorMinimo, valorInicial, taxaRendimento, resgateMinimo, vencimento, valorIOF, impostoRenda FROM Aplicacao WHERE id = @id", new { @id = id });
                aplicacao = queryResult.First();

                aplicacao.contaCorrente = contaCorrente;

                return aplicacao;
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        //Cancelar uma determinada aplicacao
        static public Boolean CancelarAplicacao(string identificador)
        {
            bool sucesso;

            if (Int32.TryParse(identificador, out int idBusca)) //tenta converter a string informada em numero
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    MySqlCommand removerCliente = new MySqlCommand("UPDATE Aplicacao SET Aplicacao.status = 'Cancelada' WHERE id = @identificador", connection);
                    removerCliente.Parameters.AddWithValue("@identificador", idBusca);

                    removerCliente.ExecuteNonQuery();
                    removerCliente.Parameters.Clear();

                    sucesso = true;
                }
                catch (MySqlException exception)
                {
                    sucesso = false;
                    Console.WriteLine(exception.ToString());
                }
                finally
                {
                    connection.Close();
                }
            } else
            {
                sucesso = false;
            }

            return sucesso;
        }

        //Atualizacao de Usuario
        static public Boolean AtualizarUsuario(string primeiroNome, string sobrenome, string cpf)
        {

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                int linhasAfetadasUsuario = connection.Execute("UPDATE Usuario SET primeiroNome = @primeiroNome, sobrenome = @sobrenome WHERE cpf = @cpf", new { @primeiroNome = primeiroNome, @sobrenome = sobrenome, @cpf = cpf});

                    return true;
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }
            finally
            {
                connection.Close();
            };

        }

        //Atualizacao de Cliente
        static public Boolean AtualizarCliente(string emailAntigo, string estado_civil, string emailNovo, string celular, string telefone, string status)
        {

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                int linhasAfetadasCliente = connection.Execute("UPDATE Cliente SET estado_civil = @estado_civil, email = @email, celular = @celular, telefone = @telefone, status = @status WHERE email = @emailAntigo;", 
                    new { @estado_civil = estado_civil, @email = emailNovo, @celular = celular, @telefone = telefone, @status = status, @emailAntigo = emailAntigo});

                    return true;
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }
            finally
            {
                connection.Close();
            };
        }

        //Atualizacao de Endereco
        static public Boolean AtualizarEndereco(string cepAntigo, string cepNovo, string tipo, string logradouro, int numero, string bairro, string cidade, string estado, string complemento)
        {

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                int linhasAfetadasEndereco = connection.Execute("UPDATE Endereco SET cep = @cepNovo, tipo = @tipo, logradouro = @logradouro, numero = @numero, bairro = @bairro, cidade = @cidade, estado = @estado, complemento = @complemento WHERE cep = @cepAntigo;",
                    new { @cepNovo = cepNovo, @tipo = tipo, @logradouro = logradouro, @numero = numero, @bairro = bairro, @cidade = cidade, @estado = estado, @complemento = complemento, @cepAntigo = cepAntigo });

                    return true;
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }
            finally
            {
                connection.Close();
            };
        }

        //Atualizacao de PessoaFisica
        static public Boolean AtualizarPF(string profissaoAtual, decimal rendaMensal, string cpf)
        {

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                int linhasAfetadasEndereco = connection.Execute("UPDATE PessoaFisica JOIN Cliente ON PessoaFisica.id_cliente = Cliente.id JOIN Usuario ON Cliente.id_usuario = Usuario.id SET profissao = @profissao, rendaMensal = @rendaMensal WHERE Usuario.cpf = @cpf",
                    new { @profissao = profissaoAtual, @rendaMensal = rendaMensal, @cpf = cpf });

                return true;
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        //Verifica se o administrador esta cadastrado no banco
        static public bool SelecionarAdministrador(string login, string senha)
        {
            bool sucesso = false;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT Administrador.login FROM Administrador WHERE login = @login AND senha=@senha;", connection);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@senha", senha);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        treatment = reader[0].ToString();
                    }
                    reader.Close();
                    if (treatment != "Sem alteração") sucesso = true;
                    else sucesso = false;
                }
            }
            catch (MySqlException exception)
            {
                sucesso = false;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }
            return sucesso;
        }

        //Verifica se a conta deste cliente esta cadastrada no banco
        static public bool LoginCliente(string _numeroConta, string senha)
        {
            bool sucesso = false;
            try
            {
                if (senha.Length == 4)
                {
                    MySqlConnection connection = new MySqlConnection("SERVER=db4free.net;PORT=3306;DATABASE=sistemabancario;UID=bancario;PWD=sb100001");
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT Conta.senha FROM Conta WHERE Conta.numero = @conta;", connection);
                    command.Parameters.AddWithValue("@conta", _numeroConta);
                    MySqlDataReader reader3 = command.ExecuteReader();
                    while (reader3.Read())
                    {
                        treatment = reader3[0].ToString();

                    }
                    reader3.Close();
                    if (treatment != "Sem alteração") sucesso = true;
                    else sucesso = false;
                }
                else
                {
                    MessageBox.Show("Senha incompleta");
                }
            }

            catch (MySqlException ex)
            {
                sucesso = false;
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return sucesso;
        }

        //Atualizacao de PessoaJuridica
        static public Boolean AtualizarPJ(string razaoSocial, string cpf)
        {

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                int linhasAfetadasEndereco = connection.Execute("UPDATE PessoaJuridica JOIN Cliente ON PessoaJuridica.id_cliente = Cliente.id JOIN Usuario ON Cliente.id_usuario = Usuario.id SET razaoSocial = @razaoSocial WHERE Usuario.cpf = @cpf",
                    new { @razaoSocial = razaoSocial, @cpf = cpf });

                if (linhasAfetadasEndereco == 1) //1 linha do banco de dados, usuario desejado, foi atualizada
                {
                    return true;
                }
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }
            finally
            {
                connection.Close();
            };

            return false;
        }

        //Cria uma nova conta
        static public Boolean InserirConta(int agencia, int numero, string senha, string cpf)
        {
            Boolean sucesso;

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Recupera o id da agencia informada
                var idAgencia = connection.ExecuteScalar<int>("SELECT id FROM Agencia WHERE numero = @agencia", new { @agencia = agencia });

                //Recupera o id do cliente informado
                var idCliente = connection.ExecuteScalar<int>("SELECT Cliente.id FROM Cliente JOIN Usuario ON Cliente.id_usuario = Usuario.id WHERE Usuario.cpf = @cpf", new { cpf = cpf });

                //Cria a Conta, retornando a quantidade de linhas afetadas pelo comando
                int affectedRows = connection.Execute("INSERT INTO Conta(numero, id_agencia, senha, id_cliente) VALUES(@numero, @idAgencia, @senha, @idCliente)",
                    new { @numero = numero, @idAgencia = idAgencia, @senha = senha, @idCliente = idCliente });

                if (affectedRows == 1) //Obrigatoriamente uma linha precisa ser inserida
                {
                    sucesso = true;
                }
                else
                {
                    sucesso = false;
                }
            }
            catch (MySqlException exception)
            {
                sucesso = false;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                connection.Close();
            }

            return sucesso;
        }

        //Busca por uma conta
        static public DataTable BuscarConta(string numeroConta)
        {
            DataTable dataTable = new DataTable();

            if (!ApenasDigitos(numeroConta)) //Confere se a string digitada apenas contem numeros
            {
                dataTable = null;
            }
            else
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    MySqlCommand buscarConta = new MySqlCommand("SELECT Conta.id AS 'Identificador', Conta.numero AS 'Número da conta', Agencia.numero AS 'Número da agência', Conta.status AS 'Status', Usuario.cpf AS 'CPF cliente responsável' FROM Conta JOIN " +
                        "Agencia ON Conta.id_agencia = Agencia.id JOIN Cliente ON Conta.id_cliente = Cliente.id JOIN Usuario ON Cliente.id_usuario = Usuario.id WHERE Conta.numero = @numero", connection);
                    buscarConta.Parameters.AddWithValue("@numero", numeroConta);

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(buscarConta);
                    dataAdapter.Fill(dataTable);

                }
                catch (MySqlException exception)
                {
                    dataTable = null;
                    Console.WriteLine(exception.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }
            return dataTable;
        }

        //Retorna um objeto do tipo conta
        static public ContaCorrente RetornarContaCorrente(int id)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Recupera o id de conta
                var idConta = connection.ExecuteScalar<int>("SELECT id_conta FROM ContaCorrente WHERE ContaCorrente.id = @id", new { @id = id });

                //Recupera o id da agencia da conta
                var idAgencia = connection.ExecuteScalar<int>("SELECT Conta.id_agencia FROM Conta JOIN ContaCorrente ON Conta.id = ContaCorrente.id_conta WHERE ContaCorrente.id = @id", new { @id = id });

                //Recupera o id do cliente da conta
                var idCliente = connection.ExecuteScalar<int>("SELECT Conta.id_cliente FROM Conta JOIN ContaCorrente ON Conta.id = ContaCorrente.id_conta WHERE ContaCorrente.id = @id", new { @id = id });

                //Cria a classe conta
                Conta conta = RetornarConta(idConta, idAgencia, idCliente);
               
                //Associa o objeto endereco buscado ao objeto dependente criado
                var queryResult = connection.Query<ContaCorrente>("SELECT taxa, limite FROM ContaCorrente WHERE id = @id", new { @id = id });

                ContaCorrente contaCorrente = queryResult.First();

                contaCorrente.Agencia = conta.Agencia;
                contaCorrente.Cliente = conta.Cliente;
                contaCorrente.Numero = conta.Numero;
                contaCorrente.Saldo = conta.Saldo;
                contaCorrente.Status = conta.Status;

                contaCorrente = queryResult.First();

                return contaCorrente;

            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return null;
            }
            finally
            {
                connection.Close();
            }

        }

        //Retorna um objeto do tipo Conta
        static public Conta RetornarConta(int idConta, int idAgencia, int idCliente)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Retorna o cliente responsavel pela conta
                 Cliente cliente = RetornarCliente(idCliente);

                    //Confere se o cliente nao eh um dependente
                    if (cliente is Dependente)
                {
                    return null;

                }
                else
                 {
                    //Retorna a agencia da conta
                    Agencia agencia = RetornarAgencia(idAgencia);

                    //Retorna o objeto conta
                    var queryResult = connection.Query<Conta>("SELECT numero, saldo, senha, status FROM Conta WHERE id = @id", new { @id = idConta });

                    Conta conta = queryResult.First();

                    //Associa agencia e cliente a conta
                    conta.Cliente = cliente;
                    conta.Agencia = agencia;

                    return conta;
                }

            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        //Retorna um objeto do tipo Conta
        static public Agencia RetornarAgencia(int id)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Busca pelo endereco da agencia
                var queryResultFirst = connection.Query<Endereco>("SELECT Endereco.tipo, Endereco.logradouro, Endereco.numero, Endereco.bairro, Endereco.complemento, Endereco.cep, Endereco.cidade, Endereco.estado FROM Endereco " +
                    "JOIN Agencia on Agencia.id_endereco = Endereco.id WHERE Agencia.id = @id", new { @id = id });

                Endereco endereco = queryResultFirst.First();

                //Busca pela agencia da conta
                var queryAgencia = connection.Query<Agencia>("SELECT numero FROM Agencia WHERE id = @id", new { @id = id });

                Agencia agencia = queryAgencia.First();

                //Associa o endereco da agencia ao objeto da agencia
                agencia.Endereco = endereco;

                return agencia;

            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        static public Decimal ConsultarSaldo(string numeroConta)
        {
            decimal saldoCliente = 0;

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                saldoCliente= connection.ExecuteScalar<decimal>("SELECT saldo FROM Conta WHERE numero = @conta", new { @conta = numeroConta });
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());

            }
            finally
            {
                connection.Close();
            }
            return saldoCliente;
        }

        static public List<Agencia> ListaAgencias()
        {
            List<Agencia> agencias = new List<Agencia>();
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                var query= connection.Query<Agencia>("SELECT * FROM Agencia");
                agencias = query.ToList();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());

            }
            finally
            {
                connection.Close();
            }
            return agencias;
        }

        static public Endereco SelecionaEndereco(int id)
        {
            Endereco endereco = new Endereco();
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                var query = connection.Query<Endereco>("SELECT Endereco.tipo, Endereco.logradouro, Endereco.numero, Endereco.bairro, Endereco.complemento, Endereco.cep, Endereco.cidade, Endereco.estado FROM Endereco " +
                    "JOIN Agencia on Agencia.id_endereco = Endereco.id WHERE Agencia.id = @id", new { @id = id });
                endereco = query.First();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());

            }
            finally
            {
                connection.Close();
            }
            return endereco;


        }

        //Funcao que realiza o pagamento de um determinado boleto bancario de cobranca - Efetua a atualizacao de saldo na conta do cliente
        static public Boolean RealizarPagamento(string numBoleto, decimal valor, int numeroConta, string codBancoDestino)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Recupera o id da conta corrente envolvida
                var idContaCorrente = connection.ExecuteScalar<int>("SELECT ContaCorrente.id FROM ContaCorrente JOIN Conta ON ContaCorrente.id_conta = Conta.id WHERE Conta.numero = @numero", new { @numero = numeroConta });

                //Retornar conta 
                ContaCorrente conta = RetornarContaCorrente(idContaCorrente);

                DateTime dataAtual = DateTime.Now;

                //Insere o registro de pagamento na tabela de Pagamentos
                int linhasAfetadasPag = connection.Execute("INSERT INTO Pagamento(dataHoraTransacao, numeroBoleto, valor, id_contaOrigem, cod_bancoDestino) VALUES(@dataHoraTransacao, @numeroBoleto, @valor, @id_contaOrigem, @cod_bancoDestino)",
                        new { @dataHoraTransacao = dataAtual, @numeroBoleto = numBoleto, @valor = valor, @id_contaOrigem = idContaCorrente, @cod_bancoDestino = codBancoDestino });

                Pagamento pagamento = new Pagamento(dataAtual, numBoleto, valor, conta, codBancoDestino);

                //Calcula o novo saldo da conta
                decimal saldoAtualizado = conta.Saldo - valor;

                //Atualiza a tabela conta com o novo saldo
                int linhasAfetadasConta = connection.Execute("UPDATE Conta JOIN ContaCorrente ON ContaCorrente.id_conta = Conta.id SET saldo = @saldoAtualizado WHERE ContaCorrente.id = @idCC",
                    new { @saldoAtualizado = saldoAtualizado, @idCC = idContaCorrente });

                if (linhasAfetadasPag == 1 && linhasAfetadasConta == 1)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        /*static public Boolean RealizarTransfOB()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Recupera o id da conta corrente envolvida
                var idContaCorrente = connection.ExecuteScalar<int>("SELECT ContaCorrente.id FROM ContaCorrente JOIN Conta ON ContaCorrente.id_conta = Conta.id WHERE Conta.numero = @numero", new { @numero = numeroConta });

                //Retornar conta 
                ContaCorrente conta = RetornarContaCorrente(idContaCorrente);

                DateTime dataAtual = DateTime.Now;

                //Insere o registro de pagamento na tabela de Pagamentos
                int linhasAfetadasPag = connection.Execute("INSERT INTO Transferencia(dataHoraTransacao, tipo, valor, id_contaOrigem, num_contaDest, agencia_contaDest, cod_bancoDestino) VALUES(@dataHoraTransacao, @tipo, @valor, @id_contaOrigem, @cod_bancoDestino)",
                        new { @dataHoraTransacao = dataAtual, @numeroBoleto = numBoleto, @valor = valor, @id_contaOrigem = idContaCorrente, @cod_bancoDestino = codBancoDestino });

                Pagamento pagamento = new Pagamento(dataAtual, numBoleto, valor, conta, codBancoDestino);

                //Calcula o novo saldo da conta
                decimal saldoAtualizado = conta.Saldo - valor;

                //Atualiza a tabela conta com o novo saldo
                int linhasAfetadasConta = connection.Execute("UPDATE Conta JOIN ContaCorrente ON ContaCorrente.id_conta = Conta.id SET saldo = @saldoAtualizado WHERE ContaCorrente.id = @idCC",
                    new { @saldoAtualizado = saldoAtualizado, @idCC = idContaCorrente });

                if (linhasAfetadasPag == 1 && linhasAfetadasConta == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        static public Boolean RealizarTransfEC()
        {

        }*/
    }
}

