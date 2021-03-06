using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo12
{

    public class Producto
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }

        public Producto(int codigo , string descripcion, double precio){
                this.Codigo=codigo;
                this.Descripcion=descripcion;
                this.Precio=precio;
        }
    }


    public class Catalogo 
    {   
        private List<Producto> productos = new List<Producto>();
          // agregar
        public void Agregar(Producto producto)
        {
               productos.Add(producto);
        }
         public static Catalogo  Cargar(){

                var cat1 = new Catalogo();
            // cat1.Agregar(new Producto () Codigo = 10, Descripcion = "coca cola 2,5L", Precio = 123 });
                cat1.Agregar(new Producto (10, "coca cola 2,5L", 100 ) );
                cat1.Agregar(new Producto (11, "Sprite 2,5L",    115 ) );
                cat1.Agregar(new Producto (12, "Pepsi Cola 2,5L",150 ) );
                cat1.Agregar(new Producto (13, "Arroz Gallo Oro 1Kg", 85 ));
                cat1.Agregar(new Producto (14, "Arroz Gallo Doble Carolina",  145 ));

            //   Console.WriteLine(cat1.productos.Count.ToString());
                return cat1;
        }

        //  BUSQUEDA OPCION 2
        public Producto Buscar(int codigo) => productos.Where( p => p.Codigo == codigo).First();
        
        //  BUSQUEDA OPCION 1 
        public Producto Busqueda(int codigo)
        {
            foreach (var p in productos)
            {
                if (p.Codigo == codigo)  return p;
            }
            return null;
        }
    }

    public class Factura
    {
         class Item
        {
                public int cantidad;
                public Producto producto;
                public double importe=> producto.Precio*cantidad;
        }
        Catalogo Catalogo;
        List<Item> items;
        int numeroFactura;
   
        public Factura(Catalogo catalogo){   //      Inyeccion de dependencia 
            numeroFactura = ProxFac++;
            items = new();
            Catalogo = catalogo;            //      Inyeccion de dependencia 
            
        }

        public void Vender(int codigo, int cantidad)
        {          
           // var producto = Catalogo.Buscar(codigo);
            var producto = Catalogo.Busqueda(codigo);

            if(producto == null) return;

                if( items.Count() >= 1){
                              
                    bool bandera=false;
                        foreach(var Li in items)
                        {
                                if( Li.producto == producto){            
                                    Li.cantidad += cantidad;
                                    bandera=true;
                                    break;
                                }
                                else
                                {
                                    bandera = false;
                                }
                        }
                        if(bandera == false){
                             items.Add(new Item() {cantidad=cantidad,producto=producto});
                        }
                }
                else
                {
                     items.Add(new Item() {cantidad=cantidad,producto=producto});      //  Primer Producto
                }
        }

        public void Listar(){
            int acum =0;
                Console.WriteLine("Numero de Factura {0}",numeroFactura);
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Cantidad   -    Descripcion     -     Precio/u   -  Subtotal" );
                foreach(var i in items){
                
                    Console.WriteLine($" {i.cantidad, -9} - {i.producto.Descripcion, -28} -   {i.producto.Precio, -6} -  {i.importe} " );
                        acum += i.cantidad;
                }

                Console.WriteLine("-----------------------------");
                Console.WriteLine($"Total General: {Total()}");
                Console.WriteLine("Nro de Items: {0}",acum);
                Console.WriteLine("-----------------------------");
        }


      public  double Total () {
         return items.Sum( j => j.importe);
      } 
        static int ProxFac = 1;
    }

    public class Caja{
        Catalogo Catalogo;
        List<Factura> Facturas1;
        public Caja(Catalogo catalogo){
            Catalogo = catalogo;
            Facturas1 = new();
        }

        public Factura Generar(){
            var fact = new Factura(Catalogo);
            Facturas1.Add(fact);
            return fact;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {          
                    Catalogo cat = Catalogo.Cargar();
                    Caja caja1 = new(cat);

                    var venta1= caja1.Generar();
                    venta1.Vender(10,2);
                    venta1.Vender(10,3);
                    venta1.Vender(11,2);
                    venta1.Vender(13,4);
                    venta1.Vender(13,-3);
                    venta1.Vender(13,-1);
                   
                    venta1.Vender(11,2);
                    venta1.Vender(14,3);

                     venta1.Listar();

                    //   VENTA   Nro 2 (FACTURA 2)
                    var venta2 = caja1.Generar();

                    venta2.Vender(11,5);
                    venta2.Vender(13,2);
                    venta2.Vender(11,3);

                    venta2.Listar();
        }
    }
}
