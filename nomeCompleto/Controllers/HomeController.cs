using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nomeCompleto;

namespace nomeCompleto.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = "Banco de Dados";

        private readonly List<string> nomesCompostos = new List<string>
        {
            "João Pedro", "Pedro Henrique", "Lucas Gabriel", "Miguel Ângelo", "Enzo Gabriel", "Guilherme Augusto", "Felipe André", "Arthur Miguel", "Leonardo Antônio", "Matheus Eduardo", "Rafael Alexandre", "Gustavo Henrique", "Samuel Joaquim", "André Luiz", "Marcos Vinícius", "Daniel Roberto", "Bruno Rafael", "Lucas Emanuel", "Marcelo Ricardo", "Victor Hugo",
            "Maria Eduarda", "Ana Clara", "Beatriz Sofia", "Laura Vitória", "Vitória Maria", "Maria Clara", "Maria Cecília", "Ana Júlia", "Sofia Gabriela", "Ana Luiza", "Maria Valentina", "Maria Alice", "Maria Fernanda", "Maria Helena", "Lara Beatriz", "Clara Eduarda", "Laura Beatriz", "Júlia Vitória", "Sofia Valentina", "Luiza Gabriela"

        };


        //Regra Nome Composto
        private void ManipularNomesCompostos(tbl_nomeCompleto model)
        {
            if (model.Nome_completo == null)
            {
                model.Nome_completo = model.Nome;
            }
            
            if(model.Nome_completo == "")
            {
                model.Nome_completo = model.Nome;
            }

            if(nomesCompostos == null)
            {
                model.Nome_completo = "";
                model.Nome = "";
            }

            // Ve se o nome inserido está na lista de nomes compostos
            if (nomesCompostos.Any(nc => model.Nome.StartsWith(nc, StringComparison.InvariantCultureIgnoreCase)))
            {
                string nomeCompostoEncontrado = nomesCompostos
                    .FirstOrDefault(nc => model.Nome.StartsWith(nc, StringComparison.InvariantCultureIgnoreCase));

                int firstSpaceIndex = nomeCompostoEncontrado.IndexOf(' ');

                if (firstSpaceIndex >= 0)
                {
                    model.Nome = nomeCompostoEncontrado;
                    model.Nome_completo = model.Nome_completo;
                }
            }
            else // Se o nome não estiver na lista, mantém o que estiver antes do primeiro espaço no campo Nome e passa todo o restante para o campo Nome Completo
            {
                int firstSpaceIndex = model.Nome_completo.Trim().IndexOf(' ');

                if (firstSpaceIndex >= 0)
                {
                    model.Nome = model.Nome_completo.Substring(0, firstSpaceIndex);
                    model.Nome_completo = model.Nome_completo.Substring(firstSpaceIndex + 1);
                }
                else
                {
                    model.Nome_completo = model.Nome;
                }
            }
        }

        


        public ActionResult Index()
        {
            return View();
        }

      
        [HttpPost]
        public ActionResult Index(tbl_nomeCompleto model)
        {
            if (ModelState.IsValid)
            {
                // Verifica se o campo Nome está vazio ou nulo
                if (string.IsNullOrWhiteSpace(model.Nome))
                {
                    int firstSpaceIndex = model.Nome_completo.Trim().IndexOf(' ');
                    if (firstSpaceIndex >= 0)
                    {
                        model.Nome = model.Nome_completo.Substring(0, firstSpaceIndex);
                        // Mantém o que está no campo Nome_completo
                    }
                    else
                    {
                        // Caso o campo Nome_completo não tenha espaços, define o valor completo para o campo Nome
                        model.Nome = model.Nome_completo;
                        model.Nome_completo = ""; // Limpa o Nome_completo

          
                    }
                    
                    
                }
                else
                {
                    // Verifica se o valor do campo Nome está na lista de nomes compostos
                    string nomeEncontrado = nomesCompostos
                        .FirstOrDefault(nc => model.Nome.StartsWith(nc, StringComparison.InvariantCultureIgnoreCase));

                    if (nomeEncontrado != null)
                    {
                        ManipularNomesCompostos(model);
                    }
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "INSERT INTO tbl_nomeCompleto (Nome, Nome_completo) VALUES (@Nome, @Nome_completo)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Nome", model.Nome);

                        // Verifica se o campo Nome_completo está vazio ou nulo e trata o parâmetro adequadamente
                        if (string.IsNullOrWhiteSpace(model.Nome_completo))
                        {
                            command.Parameters.AddWithValue("@Nome_completo", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Nome_completo", model.Nome_completo);
                        }

                        command.ExecuteNonQuery();
                    }

                    return RedirectToAction("About");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao salvar os dados: " + ex.Message);
                }
            }

            return View(model);
        }


        public ActionResult About()
        {
            List<tbl_nomeCompleto> nomeCompletos = new List<tbl_nomeCompleto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Nome, Nome_completo FROM tbl_nomeCompleto";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        tbl_nomeCompleto nomeCompleto = new tbl_nomeCompleto
                        {
                            Nome = reader["Nome"].ToString(),
                            Nome_completo = reader["Nome_completo"].ToString(),
                           
                        };
                        nomeCompletos.Add(nomeCompleto);
                    }
                }
            }
            catch (Exception ex)
            {
                // Tratar erros de consulta, se necessário.
            }

            return View(nomeCompletos);
        }
    

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

       
     




    }
}