using System;
using System.Collections.Generic;
using System.Windows.Forms;

public class Pokemon
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Ataque { get; set; }
    public int Daño { get; set; }
    public int Energia { get; set; }
    public string DescripcionAtaque { get; set; }
    public int Vida { get; set; }
    public string Elemento { get; set; }
    public string Debilidad { get; set; }
    public string Fortaleza { get; set; }
    public string Fase { get; set; }
    public string Descripcion { get; set; }
    public int Numero { get; set; }

    // Lista para guardar todos los ataques de un Pokémon
    public List<(string Nombre, int Daño)> Ataques { get; set; }

    public static List<Pokemon> ParsearDatos(string datos, List<Pokemon> lista)
    {
        string[] registros = datos.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string registro in registros)
        {
            string[] campos = registro.Split('/');

            string ataque = "Desconocido";
            int daño = 0;
            int energia = 0;
            string descripcionAtaque = "Sin descripción";

            // Si tiene menos de 12 campos, agregamos valores vacíos
            if (campos.Length < 10)
            {
                return lista;

            }
            else
            {
                string[] ataqueDatos = campos[3].Split('*');
                ataque = ataqueDatos.Length > 0 ? ataqueDatos[0] : "Desconocido";
                daño = ataqueDatos.Length > 1 ? int.TryParse(ataqueDatos[1], out int d) ? d : 0 : 0;
                energia = ataqueDatos.Length > 2 ? int.TryParse(ataqueDatos[2], out int e) ? e : 0 : 0;
                descripcionAtaque = ataqueDatos.Length > 3 ? ataqueDatos[3] : "Sin descripción";
            }
             

            

            int.TryParse(campos[1], out int id);
            int.TryParse(campos[4], out int vida);
            int.TryParse(campos[5], out int elemento);
            int.TryParse(campos[6], out int debilidad);
            int.TryParse(campos[7], out int fortaleza);
            int.TryParse(campos[10], out int numero);

            Pokemon pokemon = new Pokemon
            {
                Id = id,
                Nombre = campos[2],
                Ataque = ataque,
                Daño = daño,
                Energia = energia,
                DescripcionAtaque = descripcionAtaque,
                Vida = vida,
                Elemento = ObtenerElemento(elemento),
                Debilidad = ObtenerElemento(debilidad),
                Fortaleza = ObtenerElemento(fortaleza),
                Fase = campos[8],
                Descripcion = campos[9],
                Numero = numero,
                Ataques = new List<(string, int)>()
            };

            //Añadimos el ataque que hemos parseado a la lista
            if (!string.IsNullOrEmpty(ataque) && ataque != "Desconocido")
            {
                pokemon.Ataques.Add((ataque, daño));
                
            }
            pokemon.Ataques.Add(("Placaje", 10));
            
            lista.Add(pokemon);
        }
        return lista;
    }
    private static string ObtenerElemento(int id)
    {
        switch (id)
        {
            case 1:
                return "fuego";
            case 2:
                return "planta";
            case 3:
                return "agua";
            case 4:
                return "rayo";
            default:
                return "desconocido";
        }
    }


}


