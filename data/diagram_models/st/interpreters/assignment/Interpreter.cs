using Osls.St.Boolean;
using Osls.St.Numerical;
using System.Collections.Generic;


namespace Osls.St.Assignment
{
    public class Interpreter
    {
        #region ==================== Fields Properties ====================
        private static readonly HashSet<string> AssignmentSymbol = new HashSet<string>() { "=", ":=" };
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Converts the string to a logical model.
        /// </summary>
        public static AssignmentExpression AsAssignmentExpression(string transition, IProcessingData processingData)
        {
            Terminals data = new Terminals(transition);
            return InterpretAssignmentExpression(data, processingData);
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        /// <summary>
        /// Interprets the given words into a logical model.
        /// We follow a fixed y = x format according the requirements.
        /// </summary>
        private static AssignmentExpression InterpretAssignmentExpression(Terminals data, IProcessingData context)
        {
            string targetWord = data.Current;
            data.MoveNext();
            if (data.IsEndReached) return null;
            string assignmentSymbol = data.Current;
            data.MoveNext();
            if (!AssignmentSymbol.Contains(assignmentSymbol) || data.IsEndReached) return null;
            if (context.OutputRegisters.ContainsBoolean(targetWord))
            {
                BooleanExpression sourceExpression = St.Boolean.Interpreter.InterpretBooleanExpression(data, context);
                return new Boolean(targetWord, sourceExpression, context);
            }
            else if (context.OutputRegisters.ContainsInteger(targetWord))
            {
                string numerical = data.Current;
                NumericalExpression sourceExpression = St.Numerical.Interpreter.AsNumericalExpression(numerical, context);
                return new Numerical(targetWord, sourceExpression, context);
            }
            return null; // not valid
        }
        #endregion
    }
}