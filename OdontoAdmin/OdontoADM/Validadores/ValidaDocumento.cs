using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdontoADM.Validadores
{
    class ValidaDocumento
    {
        public bool ValidaCPF(string sourceCPF)
        {
            //// Pré-Verificação do Tamanho da String (Apenas para Performance)
            //if (sourceCPF.Length != 14)
            //{
            //    return false;
            //}
            // Cria Objetos
            string clearCPF;
            int[] cpfArray;
            int totalDigitoI = 0;
            int totalDigitoII = 0;
            int modI;
            int modII;
            // Limpa o CPF
            clearCPF = sourceCPF.Trim(); // Elimina Espaços em Branco
            clearCPF = clearCPF.Replace("-", ""); // Remove Separador de Dígito Verificador
            clearCPF = clearCPF.Replace(".", ""); // Remove os Separadores das Casas
            clearCPF = clearCPF.Replace(",", ""); // Remove os Separadores das Casas
            // Verifica o Tamanho do Texto de Input
            if (clearCPF.Length != 11)
            {
                return false;
            }
            // Verifica os Patterns mais Comuns para CPF's Inválidos
            if (clearCPF.Equals("00000000000") ||
                clearCPF.Equals("11111111111") ||
                clearCPF.Equals("22222222222") ||
                clearCPF.Equals("33333333333") ||
                clearCPF.Equals("44444444444") ||
                clearCPF.Equals("55555555555") ||
                clearCPF.Equals("66666666666") ||
                clearCPF.Equals("77777777777") ||
                clearCPF.Equals("88888888888") ||
                clearCPF.Equals("99999999999"))
            {
                return false;
            }
            // Verifica se no Array Existe Apenas Números
            foreach (char c in clearCPF)
            {
                if (!char.IsNumber(c))
                {
                    return false;
                }
            }
            // Converte o CPF em Array Numérico para Validar
            cpfArray = new int[11];
            for (int i = 0; i < clearCPF.Length; i++)
            {
                cpfArray[i] = int.Parse(clearCPF[i].ToString());
            }
            // Computa os Totais para os 2 Dígitos Verificadores
            for (int position = 0; position < cpfArray.Length - 2; position++)
            {
                totalDigitoI += cpfArray[position] * (10 - position);
                totalDigitoII += cpfArray[position] * (11 - position);
            }
            // Aplica Regras do Dígito 1
            modI = totalDigitoI % 11;
            if (modI < 2) { modI = 0; }
            else { modI = 11 - modI; }
            // Verifica o Digito 1
            if (cpfArray[9] != modI)
            {
                return false;
            }
            // Aplica o Peso para o Digito Verificador 2
            totalDigitoII += modI * 2;
            // Aplica Regras do Dígito Verificador 2
            modII = totalDigitoII % 11;
            if (modII < 2) { modII = 0; }
            else { modII = 11 - modII; }
            // Verifica o Digito 2
            if (cpfArray[10] != modII)
            {
                return false;
            }
            // CPF Válido!
            return true;
        }
    }
}
