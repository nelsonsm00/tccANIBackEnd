using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace ANI.Arquitetura
{
    public abstract class ModuloBase
    {
        private DataBase db;
        public readonly string Conta;

        public ModuloBase()
        {
            this.db = new DataBase();
            this.Conta = ContextRequest.ContextConta();
        }

        public ModuloBase(DataBase? db)
        {
            if (db == null)
                this.db = new DataBase();
            else
                this.db = db;
            this.Conta = ContextRequest.ContextConta();
        }

        public DataBase getDataBase()
        {
            return this.db;
        }

        public int ExecuteSequence(string pSequence)
        {
            return this.ExecuteScalar<int>($@"Select next value for {pSequence} as Id");
        }

        public T ExecuteScalar<T>(string pCommand)
        {
            return db.ExecuteScalar<T>(pCommand);
        }

        public T ExecuteQuery<T>(string pCommand) where T: new()
        {
            return db.ExecuteQuery<T>(pCommand);
        }

        public List<T> ExecuteQueryList<T>(string pCommand) where T : new()
        {
            return db.ExecuteQueryList<T>(pCommand);
        }

        public void ExecuteCommand<T>(string pCommand, T pRegistro)
        {
            db.ExecuteCommand<T>(pCommand, pRegistro);
        }

        public bool ExisteRegistro(string pTabela, string? pCondicao = null)
        {
            if (!string.IsNullOrEmpty(pCondicao))
                pCondicao = " Where " + pCondicao;
            else
                pCondicao = "";
            return this.ExecuteScalar<int>("Select TOP 1 1 from " + pTabela + pCondicao) == 1;
        }

        public string FormataData(DateTime? pData)
        {
            if (pData == null)
                return "NULL";
            else
                return $@"CAST(N'{pData.Value.ToString("yyyy-MM-dd HH:mm:00.000")}' As DATETIME)";
        }

    }
}
