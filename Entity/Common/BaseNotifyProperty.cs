using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Entity.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseNotifyProperty : INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Property Change Event Handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Property Change Event
        /// <summary>
        /// Method that fires Property Change Event
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="propName"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        protected bool SetValue<T>(T val, ref T prop, [CallerMemberName] string propName = null)
        {
            if (EqualityComparer<T>.Default.Equals(prop, val))
            {
                return true;
            }
            prop = val;
            OnPropertyChanged(propName);
            return false;
        }
        #endregion
    }
}
