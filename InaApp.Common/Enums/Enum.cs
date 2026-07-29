
using System.ComponentModel.DataAnnotations;


namespace InaApp.Common.Enums
{
    //clase static para no crear espacio en memoria para esta clase, ya que solo se va a usar
    //para definir los enums, y no se va a crear instancias de esta clase
    public static class Enumeradores { 
         
        public enum TipoCedulaEnum
        {
            [Display(Name = "Cedula Nacional")]
            Nacional = 1,

            [Display(Name = "Cedula Juridica")]
            Juridica = 2,

            Dimex = 3,
            //nite es un tipo de cedula que se le asigna a las empresas extranjeras que operan en Costa Rica,
            //es un numero unico que se utiliza para identificarlas y para realizar transacciones comerciales(tributarias),
            //formato comienza con la letra N seguida de numero de 8 digitos #, por ejemplo: N12345678
            NITE = 4,
            Pasaporte = 5
        }

        public enum TipoImpuestoAplicable
        {
            [Display(Name = " Impuesto al Valor Agregado")]
            IVA = 1,

            [Display(Name = "Impuesto Selectivo de Consumo")]
            ISC = 2,

            [Display(Name = "Impuesto Unico a los Combustivos")]
            IUC = 3,

            [Display(Name = "Impuesto Especifico de Bebidas Alcohólicas")]
            IEBA = 4,

            [Display(Name = "Impuesto Especifico Sobre las Bebidas Envasadas Sin Contenido Alcoholico y Jabones de Tocador")]
            IESBECAJT = 5,


            [Display(Name = "Impuesto a los Productos de Tabaco")]
            IPT = 6,

            [Display(Name = "IVA(cálculo especial)")]
            IVACE = 7,


            [Display(Name = "IVA Régimen de Bienes Usados (Factor)")]
            IVARBUF = 8,

            [Display(Name = "Impuesto Especifico al Cemento")]
            IEC = 12,

            [Display(Name = "Otros")]
            Otros = 99

        }



        public enum TipoVentaEnum
        {
            Contado = 1,
            Credito = 2
        }


        public enum TipopPagoEnum
        {
            Efectivo = 1,
            TarjetaCredito = 2,
            TarjetaDebito = 3,
            TransferenciaBancaria = 4,
            Cheque = 5,
            SinpeMovil = 6
        }
    }
}
