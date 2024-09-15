//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System.Collections;
using System;
namespace CSystem.Text.RegularExpressions
{
    internal class GroupEnumerator : IEnumerator
    {
        #region Fields

        internal int _curindex = -1;
        internal GroupCollection _rgc;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the capture element.
        /// </summary>
        public Capture Capture
        {
            get
            {
                if ((_curindex < 0) || (_curindex >= _rgc.Count))
                {
                    throw new InvalidOperationException("EnumNotStarted");
                }

                return _rgc[_curindex];
            }
        }

        /// <summary>
        /// Gets the current capture object.
        /// </summary>
        public object Current => Capture;

        #endregion

        #region Constructor

        internal GroupEnumerator(GroupCollection rgc)
        {
            _rgc = rgc;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Move tot the next element
        /// </summary>
        /// <returns>True if success</returns>
        public bool MoveNext()
        {
            int count = _rgc.Count;
            if (_curindex >= count)
            {
                return false;
            }

            return (++_curindex < count);
        }

        /// <summary>
        /// Reset the position.
        /// </summary>
        public void Reset()
        {
            _curindex = -1;
        }

        #endregion
    }
}
