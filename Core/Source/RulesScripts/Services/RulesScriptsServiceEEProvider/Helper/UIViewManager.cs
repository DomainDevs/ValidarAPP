using System;
using System.Collections.Generic;
using System.IO;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.RulesScriptsServices.Helper
{
    public class UIViewManager
    {
        
        private static volatile UIViewManager instance;
        private static object syncRoot = new Object();        
        
        public static UIViewManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new UIViewManager();
                    }
                }

                return instance;
            }
        }

        private UIViewManager() 
        {
              
        } 


        public UIViewDefinition GetViewDefinition(string viewName)
        {
            return this.GetViewDefinition(viewName, null);
        }

        public UIViewDefinition GetViewDefinition(string viewName, string path)
        {            
            if (path == null || path.Equals(string.Empty))
            {
                path = "~/";
            }
            else if (path.Length > 0)
            {
                if (!path.EndsWith("/"))
                {
                    path += '/';
                }
            }

            UIViewDefinition def = null;

            path = path + "views/" + viewName + ".uiview";
            Stream s = new FileStream(path, FileMode.Open);

            if (s !=  null)
            {
                using (s)
                {
                    def = UIViewDefinition.ReadFromStream(s,viewName);// System.IO.Path.GetFileNameWithoutExtension(VirtualPathUtility.GetFileName(path)));
                }
            }
            
            return def;
        }

      

        private void SetFilter(SelectionQuery q, Predicate pred)
        {
            if (q is SelectQuery)
            {
                this.SetFilter(q as SelectQuery, pred);
            }
            else if (q is UnionQuery)
            {
                this.SetFilter(q as UnionQuery, pred);
            }
        }

        private void SetFilter(UnionQuery q, Predicate pred)
        {
            this.SetFilter(q.Select1, pred);
            this.SetFilter(q.Select2, pred);
        }

        private void SetFilter(SelectQuery q, Predicate pred)
        {
            IDictionary<string, Value> colmap = this.CreateColumnMap(q.Values);
            Predicate myPred = (Predicate)pred.Clone();

            this.ReplaceColumns(myPred, colmap);

            Predicate p = q.Where;
            if (p == null)
            {
                p = myPred;
            }
            else
            {
                BinaryPredicate and = new BinaryPredicate(BinaryPredicateType.And);
                and.Predicate1 = p;
                and.Predicate2 = myPred;
                p = and;
            }

            q.Where = p;
        }

        private IDictionary<string, Value> CreateColumnMap(ICollection<SelectValue> col)
        {
            IDictionary<string, Value> map = new Dictionary<string, Value>();

            foreach (SelectValue val in col)
            {
                map[val.Alias] = val.Value;
            }

            return map;
        }

        private void ReplaceColumns(Predicate pred, IDictionary<string, Value> map)
        {
            if (pred is Criterion)
            {
                this.ReplaceColumns(pred as Criterion, map);
            }
            else if (pred is BinaryPredicate)
            {
                this.ReplaceColumns(pred as BinaryPredicate, map);
            }
            else if (pred is UnaryPredicate)
            {
                this.ReplaceColumns(pred as UnaryPredicate, map);
            }
        }

        private void ReplaceColumns(UnaryPredicate pred, IDictionary<string, Value> map)
        {
            this.ReplaceColumns(pred.Predicate, map);
        }

        private void ReplaceColumns(BinaryPredicate pred, IDictionary<string, Value> map)
        {
            this.ReplaceColumns(pred.Predicate1, map);
            this.ReplaceColumns(pred.Predicate2, map);
        }

        private void ReplaceColumns(Criterion crit, IDictionary<string, Value> map)
        {
            if (crit is BinaryCriterion)
            {
                this.ReplaceColumns(crit as BinaryCriterion, map);
            }
            else if (crit is UnaryCriterion)
            {
                this.ReplaceColumns(crit as UnaryCriterion, map);
            }
        }

        private void ReplaceColumns(BinaryCriterion crit, IDictionary<string, Value> map)
        {
            Value val = crit.Operand1;
            if (val is Column)
            {
                Value v = (Value)map[(val as Column).Name];
                if (v == null) throw new Exception("No existe la columna en el UIView (" + (val as Column).Name + ")");
                crit.Operand1 = v;
            }

            val = crit.Operand2;
            if (val is Column)
            {
                Value v = (Value)map[(val as Column).Name];
                if (v == null) throw new Exception("No existe la columna en el UIView (" + (val as Column).Name + ")");
                crit.Operand2 = v;
            }
        }

        private void ReplaceColumns(UnaryCriterion crit, IDictionary<string, Value> map)
        {
            Value val = crit.Operand;
            if (val is Column)
            {
                Value v = (Value)map[(val as Column).Name];
                if (v == null) throw new Exception("No existe la columna en el UIView (" + (val as Column).Name + ")");
                crit.Operand = v;
            }
        }
    }
}
