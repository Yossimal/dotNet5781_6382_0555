﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    /// <summary>
    /// Line Trip entity - represents frequancy of buses on a specific line
    /// </summary>
    public class DAOLineTrip
    {
        #region background data
        public bool IsRunningId => true;
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
        #endregion
        #region real data
        public int LineId { get; set; }
        public TimeSpan StartAt { get; set; }
        public TimeSpan? Frequency { get; set; }
        public TimeSpan? FinishAt { get; set; }
        #endregion
    }
}
