using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Common.Enums
{
    //clase static para no crear espacio en memoria para esta clase, ya que solo se va a usar
    //para definir los enums, y no se va a crear instancias de esta clase
    public static class Enumeradores { 
         
        public enum TipoCedulaEnum
            {
            Nacional = 1,
            Juridica = 2,
            Dimex = 3,
            //nite es un tipo de cedula que se le asigna a las empresas extranjeras que operan en Costa Rica,
        //es un numero unico que se utiliza para identificarlas y para realizar transacciones comerciales(tributarias),
        //formato comienza con la letra N seguida de numero de 8 digitos #, por ejemplo: N12345678
            NITE = 4,
            Pasaporte = 5
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
