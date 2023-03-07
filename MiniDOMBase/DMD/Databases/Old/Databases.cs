using System;
using System.Collections;
using System.Diagnostics;
using minidom;
using DMD.XML;
using static minidom.Sistema;


namespace minidom
{
    public partial class Databases
    {
        public const int DBCURSORPAGESIZE = 25;
        public static readonly DBNull NULL = DBNull.Value;




        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public enum ObjectStatus : int
        {
            OBJECT_TEMP = 0,
            OBJECT_VALID = 1,
            OBJECT_DELETED = 3
        }

        public enum DBTypesEnum : int
        {
            DBUNSUPPORTED_TYPE = 0,
            DBNUMERIC_TYPE = 1,
            DBDATETIME_TYPE = 2,
            DBBOOLEAN_TYPE = 3,
            DBTEXT_TYPE = 4,
            DBBINARY_TYPE = 5
        }

      

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */

        // CursorOptionEnum
        public enum adCusrorOptionEnum : int
        {
            adHoldRecords = 0x100,
            adMovePrevious = 0x200,
            adAddNew = 0x1000400,
            adDelete = 0x1000800,
            adUpdate = 0x1008000,
            adBookmark = 0x2000,
            adApproxPosition = 0x4000,
            adUpdateBatch = 0x10000,
            adResync = 0x20000,
            adNotify = 0x40000,
            adFind = 0x80000,
            adSeek = 0x400000,
            adIndex = 0x800000
        }

        // LockTypeEnum
        public enum adLockTypeEnum : int
        {
            adLockUnspecified = -1,    // Unspecified type of lock. Clones inherits lock type from the original Recordset.
            adLockReadOnly = 1, // Read-only records
            adLockPessimistic = 2, // Pessimistic locking, record by record. The provider lock records immediately after editing
            adLockOptimistic = 3, // Optimistic locking, record by record. The provider lock records only when calling update
            adLockBatchOptimistic = 4 // Optimistic batch updates. Required for batch update mode
        }

        // ExecuteOptionEnum
        public enum adExecuteOptionEnum : int
        {
            adAsyncExecute = 0x10,
            adAsyncFetch = 0x20,
            adAsyncFetchNonBlocking = 0x40,
            adExecuteNoRecords = 0x80
        }

        // ConnectOptionEnum
        public enum adConnectOptionEnum : int
        {
            adAsyncConnect = 0x10
        }

        // ObjectStateEnum
        public enum adObjectStateEnum : int
        {
            adStateClosed = 0x0,
            adStateOpen = 0x1,
            adStateConnecting = 0x2,
            adStateExecuting = 0x4,
            adStateFetching = 0x8
        }

        // CursorLocationEnum
        public enum adCursorLocationEnum : int
        {
            adUseNone = 1,    // OBSOLETE (appears only for backward compatibility). Does not use cursor services
            adUseServer = 2,   // Default. Uses a server-side cursor
            adUseClient = 3   // Uses a client-side cursor supplied by a local cursor library. For backward compatibility, the synonym adUseClientBatch is also supported
        }

        // CursorTypes
        public enum adCursorTypes : int
        {
            adOpenUnspecified = -1, // Does not specify the type of cursor.
            adOpenForwardOnly = 0,  // Default. Uses a forward-only cursor. Identical to a static cursor, except that you can only scroll forward through records. This improves performance when you need to make only one pass through a Recordset.
            adOpenKeyset = 1,  // Uses a keyset cursor. Like a dynamic cursor, except that you can't see records that other users add, although records that other users delete are inaccessible from your Recordset. Data changes by other users are still visible.
            adOpenDynamic = 2,  // Uses a dynamic cursor. Additions, changes, and deletions by other users are visible, and all types of movement through the Recordset are allowed, except for bookmarks, if the provider doesn't support them.
            adOpenStatic = 3  // Uses a static cursor. A static copy of a set of records that you can use to find data or generate reports. Additions, changes, or deletions by other users are not visible.
        }

        // DataTypeEnum
        public enum adDataTypeEnum  : int
        {
            adEmpty = 0,
            adTinyInt = 16,
            adSmallInt = 2,
            adInteger = 3,
            adBigInt = 20,
            adUnsignedTinyInt = 17,
            adUnsignedSmallInt = 18,
            adUnsignedInt = 19,
            adUnsignedBigInt = 21,
            adSingle = 4,
            adDouble = 5,
            adCurrency = 6,
            adDecimal = 14,
            adNumeric = 131,
            adBoolean = 11,
            adError = 10,
            adUserDefined = 132,
            adVariant = 12,
            adIDispatch = 9,
            adIUnknown = 13,
            adGUID = 72,
            adDate = 7,
            adDBDate = 133,
            adDBTime = 134,
            adDBTimeStamp = 135,
            adBSTR = 8,
            adChar = 129,
            adVarChar = 200,
            adLongVarChar = 201,
            adWChar = 130,
            adVarWChar = 202,
            adLongVarWChar = 203,
            adBinary = 128,
            adVarBinary = 204,
            adLongVarBinary = 205,
            adChapter = 136,
            adFileTime = 64,
            adPropVariant = 138,
            adVarNumeric = 139,
            adArray = 0x2000
        }

        // FieldAttributeEnum
        public enum adFieldAttributeEnum : int
        {
            adFldMayDefer = 0x2,
            adFldUpdatable = 0x4,
            adFldUnknownUpdatable = 0x8,
            adFldFixed = 0x10,
            adFldIsNullable = 0x20,
            adFldMayBeNull = 0x40,
            adFldLong = 0x80,
            adFldRowID = 0x100,
            adFldRowVersion = 0x200,
            adFldCacheDeferred = 0x1000,
            adFldIsChapter = 0x2000,
            adFldNegativeScale = 0x4000,
            adFldKeyColumn = 0x8000,
            adFldIsRowURL = 0x10000,
            adFldIsDefaultStream = 0x20000,
            adFldIsCollection = 0x40000
        }

        // EditModeEnum
        public enum asEditModeEnum : int
        {
            adEditNone = 0x0,
            adEditInProgress = 0x1,
            adEditAdd = 0x2,
            adEditDelete = 0x4
        }

        // RecordStatusEnum
        public enum adRecordStatusEnum : int
        {
            adRecOK = 0x0,
            adRecNew = 0x1,
            adRecModified = 0x2,
            adRecDeleted = 0x4,
            adRecUnmodified = 0x8,
            adRecInvalid = 0x10,
            adRecMultipleChanges = 0x40,
            adRecPendingChanges = 0x80,
            adRecCanceled = 0x100,
            adRecCantRelease = 0x400,
            adRecConcurrencyViolation = 0x800,
            adRecIntegrityViolation = 0x1000,
            adRecMaxChangesExceeded = 0x2000,
            adRecObjectOpen = 0x4000,
            adRecOutOfMemory = 0x8000,
            adRecPermissionDenied = 0x10000,
            adRecSchemaViolation = 0x20000,
            adRecDBDeleted = 0x40000
        }

        // GetRowsOptionEnum
        public enum adGetRowsOptionEnum : int
        {
            adGetRowsRest = -1
        }

        // PositionEnum
        public enum adPositionEnum : int
        {
            adPosUnknown = -1,
            adPosBOF = -2,
            adPosEOF = -3
        }

        // BookmarkEnum
        public enum adBookmarkEnum : int
        {
            adBookmarkCurrent = 0,
            adBookmarkFirst = 1,
            adBookmarkLast = 2
        }

        // MarshalOptionsEnum
        public enum adMarshalOptionsEnum : int
        {
            adMarshalAll = 0,
            adMarshalModifiedOnly = 1
        }

        // AffectEnum
        public enum adAffectEnum : int
        {
            adAffectCurrent = 1,
            adAffectGroup = 2,
            adAffectAllChapters = 4
        }

        // ResyncEnum
        public enum adResyncEnum : int
        {
            adResyncUnderlyingValues = 1,
            adResyncAllValues = 2
        }

        // CompareEnum
        public enum adCompareEnum : int
        {
            adCompareLessThan = 0,
            adCompareEqual = 1,
            adCompareGreaterThan = 2,
            adCompareNotEqual = 3,
            adCompareNotComparable = 4
        }

        // FilterGroupEnum
        public enum adFilterGroupEnum : int
        {
            adFilterNone = 0,
            adFilterPendingRecords = 1,
            adFilterAffectedRecords = 2,
            adFilterFetchedRecords = 3,
            adFilterConflictingRecords = 5
        }

        // SearchDirectionEnum
        public enum adSearchDirectionEnum : int
        {
            adSearchForward = 1,
            adSearchBackward = -1
        }

        // PersistFormatEnum
        public enum adPersistFormatEnum : int
        {
            adPersistADTG = 0,
            adPersistXML = 1
        }

        // StringFormatEnum
        public enum adStringFormatEnum : int
        {
            adClipString = 2
        }

        // ConnectPromptEnum
        public enum adConnectPromptEnum : int
        {
            adPromptAlways = 1,
            adPromptComplete = 2,
            adPromptCompleteRequired = 3,
            adPromptNever = 4
        }

        public enum adConnectModeEnum : int
        {
            adModeUnknown = 0,
            adModeRead = 1,
            adModeWrite = 2,
            adModeReadWrite = 3,
            adModeShareDenyRead = 4,
            adModeShareDenyWrite = 8,
            adModeShareExclusive = 0xC,
            adModeShareDenyNone = 0x10,
            adModeRecursive = 0x400000
        }

        public enum adRecordCreateOptionsEnum : int
        {
            adCreateCollection = 0x2000,
            adCreateStructDoc = unchecked((int)0x80000000), // - 2147483648, //0x80000000,
            adCreateNonCollection = 0x0,
            adOpenIfExists = 0x2000000,
            adCreateOverwrite = 0x4000000,
            adFailIfNotExists = -1
        }

        public enum adRecordOpenOptionsEnum : int
        {
            adOpenRecordUnspecified = -1,
            adOpenSource = 0x800000,
            adOpenAsync = 0x1000,
            adDelayFetchStream = 0x4000,
            adDelayFetchFields = 0x8000
        }

        public enum adIsolationLevelEnum : int
        {
            adXactUnspecified = -1, //0xFFFFFFFF,
            adXactChaos = 0x10,
            adXactReadUncommitted = 0x100,
            adXactBrowse = 0x100,
            adXactCursorStability = 0x1000,
            adXactReadCommitted = 0x1000,
            adXactRepeatableRead = 0x10000,
            adXactSerializable = 0x100000,
            adXactIsolated = 0x100000
        }

        public enum adXactAttributeEnum : int
        {
            adXactCommitRetaining = 0x20000,
            adXactAbortRetaining = 0x40000
        }

        public enum adPropertyAttributesEnum : int
        {
            adPropNotSupported = 0x0,
            adPropRequired = 0x1,
            adPropOptional = 0x2,
            adPropRead = 0x200,
            adPropWrite = 0x400
        }

        public enum adErrorValueEnum : int
        {
            adErrProviderFailed = 0xBB8,
            adErrInvalidArgument = 0xBB9,
            adErrOpeningFile = 0xBBA,
            adErrReadFile = 0xBBB,
            adErrWriteFile = 0xBBC,
            adErrNoCurrentRecord = 0xBCD,
            adErrIllegalOperation = 0xC93,
            adErrCantChangeProvider = 0xC94,
            adErrInTransaction = 0xCAE,
            adErrFeatureNotAvailable = 0xCB3,
            adErrItemNotFound = 0xCC1,
            adErrObjectInCollection = 0xD27,
            adErrObjectNotSet = 0xD5C,
            adErrDataConversion = 0xD5D,
            adErrObjectClosed = 0xE78,
            adErrObjectOpen = 0xE79,
            adErrProviderNotFound = 0xE7A,
            adErrBoundToCommand = 0xE7B,
            adErrInvalidParamInfo = 0xE7C,
            adErrInvalidConnection = 0xE7D,
            adErrNotReentrant = 0xE7E,
            adErrStillExecuting = 0xE7F,
            adErrOperationCancelled = 0xE80,
            adErrStillConnecting = 0xE81,
            adErrInvalidTransaction = 0xE82,
            adErrUnsafeOperation = 0xE84,
            adwrnSecurityDialog = 0xE85,
            adwrnSecurityDialogHeader = 0xE86,
            adErrIntegrityViolation = 0xE87,
            adErrPermissionDenied = 0xE88,
            adErrDataOverflow = 0xE89,
            adErrSchemaViolation = 0xE8A,
            adErrSignMismatch = 0xE8B,
            adErrCantConvertvalue = 0xE8C,
            adErrCantCreate = 0xE8D,
            adErrColumnNotOnThisRow = 0xE8E,
            adErrURLIntegrViolSetColumns = 0xE8F,
            adErrURLDoesNotExist = 0xE8F,
            adErrTreePermissionDenied = 0xE90,
            adErrInvalidURL = 0xE91,
            adErrResourceLocked = 0xE92,
            adErrResourceExists = 0xE93,
            adErrCannotComplete = 0xE94,
            adErrVolumeNotFound = 0xE95,
            adErrOutOfSpace = 0xE96,
            adErrResourceOutOfScope = 0xE97,
            adErrUnavailable = 0xE98,
            adErrURLNamedRowDoesNotExist = 0xE99,
            adErrDelResOutOfScope = 0xE9A,
            adErrPropInvalidColumn = 0xE9B,
            adErrPropInvalidOption = 0xE9C,
            adErrPropInvalidValue = 0xE9D,
            adErrPropConflicting = 0xE9E,
            adErrPropNotAllSettable = 0xE9F,
            adErrPropNotSet = 0xEA0,
            adErrPropNotSettable = 0xEA1,
            adErrPropNotSupported = 0xEA2,
            adErrCatalogNotSet = 0xEA3,
            adErrCantChangeConnection = 0xEA4,
            adErrFieldsUpdateFailed = 0xEA5,
            adErrDenyNotSupported = 0xEA6,
            adErrDenyTypeNotSupported = 0xEA7
        }

        public enum adParameterAttributesEnum : int
        {
            adParamSigned = 0x10,
            adParamNullable = 0x40,
            adParamLong = 0x80
        }

        public enum adParameterDirectionEnum : int
        {
            adParamUnknown = 0x0,
            adParamInput = 0x1,
            adParamOutput = 0x2,
            adParamInputOutput = 0x3,
            adParamReturnValue = 0x4
        }

        public enum adCommandTypeEnum : int
        {
            adCmdUnknown = 0x8,
            adCmdText = 0x1,
            adCmdTable = 0x2,
            adCmdStoredProc = 0x4,
            adCmdFile = 0x100,
            adCmdTableDirect = 0x200
        }

        public enum adEventStatusEnum : int
        {
            adStatusOK = 0x1,
            adStatusErrorsOccurred = 0x2,
            adStatusCantDeny = 0x3,
            adStatusCancel = 0x4,
            adStatusUnwantedEvent = 0x5
        }

        public enum adEventReasonEnum : int
        {
            adRsnAddNew = 1,
            adRsnDelete = 2,
            adRsnUpdate = 3,
            adRsnUndoUpdate = 4,
            adRsnUndoAddNew = 5,
            adRsnUndoDelete = 6,
            adRsnRequery = 7,
            adRsnResynch = 8,
            adRsnClose = 9,
            adRsnMove = 10,
            adRsnFirstChange = 11,
            adRsnMoveFirst = 12,
            adRsnMoveNext = 13,
            adRsnMovePrevious = 14,
            adRsnMoveLast = 15
        }

        public enum adSchemaEnum : int
        {
            adSchemaProviderSpecific = -1,
            adSchemaAsserts = 0,
            adSchemaCatalogs = 1,
            adSchemaCharacterSets = 2,
            adSchemaCollations = 3,
            adSchemaColumns = 4,
            adSchemaCheckConstraints = 5,
            adSchemaConstraintColumnUsage = 6,
            adSchemaConstraintTableUsage = 7,
            adSchemaKeyColumnUsage = 8,
            adSchemaReferentialConstraints = 9,
            adSchemaTableConstraints = 10,
            adSchemaColumnsDomainUsage = 11,
            adSchemaIndexes = 12,
            adSchemaColumnPrivileges = 13,
            adSchemaTablePrivileges = 14,
            adSchemaUsagePrivileges = 15,
            adSchemaProcedures = 16,
            adSchemaSchemata = 17,
            adSchemaSQLLanguages = 18,
            adSchemaStatistics = 19,
            adSchemaTables = 20,
            adSchemaTranslations = 21,
            adSchemaProviderTypes = 22,
            adSchemaViews = 23,
            adSchemaViewColumnUsage = 24,
            adSchemaViewTableUsage = 25,
            adSchemaProcedureParameters = 26,
            adSchemaForeignKeys = 27,
            adSchemaPrimaryKeys = 28,
            adSchemaProcedureColumns = 29,
            adSchemaDBInfoKeywords = 30,
            adSchemaDBInfoLiterals = 31,
            adSchemaCubes = 32,
            adSchemaDimensions = 33,
            adSchemaHierarchies = 34,
            adSchemaLevels = 35,
            adSchemaMeasures = 36,
            adSchemaProperties = 37,
            adSchemaMembers = 38,
            adSchemaTrustees = 39
        }

        public enum adFieldStatusEnum : int
        {
            adFieldOK = 0,
            adFieldCantConvertValue = 2,
            adFieldIsNull = 3,
            adFieldTruncated = 4,
            adFieldSignMismatch = 5,
            adFieldDataOverflow = 6,
            adFieldCantCreate = 7,
            adFieldUnavailable = 8,
            adFieldPermissionDenied = 9,
            adFieldIntegrityViolation = 10,
            adFieldSchemaViolation = 11,
            adFieldBadStatus = 12,
            adFieldDefault = 13,
            adFieldIgnore = 15,
            adFieldDoesNotExist = 16,
            adFieldInvalidURL = 17,
            adFieldResourceLocked = 18,
            adFieldResourceExists = 19,
            adFieldCannotComplete = 20,
            adFieldVolumeNotFound = 21,
            adFieldOutOfSpace = 22,
            adFieldCannotDeleteSource = 23,
            adFieldReadOnly = 24,
            adFieldResourceOutOfScope = 25,
            adFieldAlreadyExists = 26,
            adFieldPendingInsert = 0x10000,
            adFieldPendingDelete = 0x20000,
            adFieldPendingChange = 0x40000,
            adFieldPendingUnknown = 0x80000,
            adFieldPendingUnknownDelete = 0x100000
        }

        public enum adSeekEnum : int
        {
            adSeekFirstEQ = 0x1,
            adSeekLastEQ = 0x2,
            adSeekAfterEQ = 0x4,
            adSeekAfter = 0x8,
            adSeekBeforeEQ = 0x10,
            adSeekBefore = 0x20
        }

        public enum adADCPROP_UPDATECRITERIA_ENUM : int
        {
            adCriteriaKey = 0,
            adCriteriaAllCols = 1,
            adCriteriaUpdCols = 2,
            adCriteriaTimeStamp = 3
        }

        public enum adADCPROP_ASYNCTHREADPRIORITY_ENUM : int
        {
            adPriorityLowest = 1,
            adPriorityBelowNormal = 2,
            adPriorityNormal = 3,
            adPriorityAboveNormal = 4,
            adPriorityHighest = 5
        }

        public enum adADCPROP_AUTORECALC_ENUM : int
        {
            adRecalcUpFront = 0,
            adRecalcAlways = 1
        }

        // Public Enum ad ADCPROP_UPDATERESYNC_ENUM

        // ADCPROP_UPDATERESYNC_ENUM

        public enum adMoveRecordOptionsEnum : int
        {
            adMoveUnspecified = -1,
            adMoveOverWrite = 1,
            adMoveDontUpdateLinks = 2,
            adMoveAllowEmulation = 4
        }

        public enum adCopyRecordOptionsEnum : int
        {
            adCopyUnspecified = -1,
            adCopyOverWrite = 1,
            adCopyAllowEmulation = 4,
            adCopyNonRecursive = 2
        }

        public enum adStreamTypeEnum : int
        {
            adTypeBinary = 1,
            adTypeText = 2
        }

        public enum adLineSeparatorEnum : int
        {
            adLF = 10,
            adCR = 13,
            adCRLF = -1
        }

        public enum adStreamOpenOptionsEnum : int
        {
            adOpenStreamUnspecified = -1,
            adOpenStreamAsync = 1,
            adOpenStreamFromRecord = 4
        }

        public enum adStreamWriteEnum : int
        {
            adWriteChar = 0,
            adWriteLine = 1
        }

        public enum adSaveOptionsEnum : int
        {
            adSaveCreateNotExist = 1,
            adSaveCreateOverWrite = 2
        }

        public enum adFieldEnum : int
        {
            adDefaultStream = -1,
            adRecordURL = -2
        }

        public enum adStreamReadEnum : int
        {
            adReadAll = -1,
            adReadLine = -2
        }

        public enum adRecordTypeEnum : int
        {
            adSimpleRecord = 0,
            adCollectionRecord = 1,
            adStructDoc = 2
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public interface ICopyObject
        {
            object CopyFrom(object source);
        }

      

      

        public interface IDBObjectCollection : IEnumerable, IDBMinObject
        {
        }

       

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [Serializable]
        public class DBException : Exception
        {
            private string m_SQL = "";

            public DBException()
            {
                DMDObject.IncreaseCounter(this);
            }

            public DBException(string message) : base(message)
            {
                DMDObject.IncreaseCounter(this);
            }

            public DBException(string message, Exception inner) : base(message, inner)
            {
                DMDObject.IncreaseCounter(this);
            }

            public DBException(string message, string sql, Exception inner) : base(message, inner)
            {
                DMDObject.IncreaseCounter(this);
                m_SQL = sql;
            }

            public string SQL
            {
                get
                {
                    return m_SQL;
                }
            }

            ~DBException()
            {
                DMDObject.DecreaseCounter(this);
            }

            public override string ToString()
            {
                var ret = new System.Text.StringBuilder();
                ret.Append(Message);
                if (!string.IsNullOrEmpty(m_SQL))
                    ret.Append(DMD.Strings.vbNewLine + "SQL: " + m_SQL);
                if (InnerException is object)
                    ret.Append(DMD.Strings.vbNewLine + "Inner: " + InnerException.ToString());
                return ret.ToString();
            }
        }

        public sealed class PropertyChangedEventArgs : EventArgs, IDMDXMLSerializable
        {
            private string m_PropertyName;
            private object m_NewValue;
            private object m_OldValue;
            private string m_TypeName;

            public PropertyChangedEventArgs()
            {
                DMDObject.IncreaseCounter(this);
                m_PropertyName = DMD.Strings.vbNullString;
                m_NewValue = null;
                m_OldValue = null;
                m_TypeName = null;
            }

            public PropertyChangedEventArgs(string propName, object newVal = null, object oldVal = null) : this()
            {
                m_PropertyName = DMD.Strings.Trim(propName);
                m_OldValue = oldVal;
                m_NewValue = newVal;
            }

            public string PropertyName
            {
                get
                {
                    return m_PropertyName;
                }
            }

            public object OldValue
            {
                get
                {
                    return m_OldValue;
                }
            }

            public object NewValue
            {
                get
                {
                    return m_NewValue;
                }
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Name":
                        {
                            m_PropertyName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NewVal":
                        {
                            m_NewValue = GetValue(fieldValue, m_TypeName);
                            break;
                        }

                    case "OldVal":
                        {
                            m_OldValue = GetValue(fieldValue, m_TypeName);
                            break;
                        }

                    case "TypeName":
                        {
                            m_TypeName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                }
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", m_PropertyName);
                writer.WriteAttribute("TypeName", Sistema.vbTypeName(m_NewValue));
                WriteValue(writer, "NewVal", m_NewValue);
                WriteValue(writer, "OldVal", m_NewValue);
            }

            private void WriteValue(XMLWriter writer, string tag, object value)
            {
                switch (Sistema.vbTypeName(value) ?? "")
                {
                    case "Byte":
                    case "SByte":
                        {
                            writer.WriteAttribute(tag, MakeValue<byte>(value));
                            break;
                        }

                    case "Short":
                    case "UShort":
                    case "Int16":
                    case "UInt16":
                        {
                            writer.WriteAttribute(tag, MakeValue<short>(value));
                            break;
                        }

                    case "Integer":
                    case "UInteger":
                    case "Int32":
                    case "UInt32":
                        {
                            writer.WriteAttribute(tag, MakeValue<short>(value));
                            break;
                        }

                    case "Long":
                    case "ULong":
                    case var @case when @case == "Int16":
                    case "UInt64":
                        {
                            writer.WriteAttribute(tag, MakeValue<short>(value));
                            break;
                        }

                    case "Single":
                        {
                            writer.WriteAttribute(tag, MakeValue<float>(value));
                            break;
                        }

                    case "Double":
                        {
                            writer.WriteAttribute(tag, MakeValue<double>(value));
                            break;
                        }

                    case "Decimal":
                        {
                            writer.WriteAttribute(tag, MakeValue<decimal>(value));
                            break;
                        }

                    case "Date":
                    case "DateTime":
                        {
                            writer.WriteAttribute(tag, MakeValue<DateTime>(value));
                            break;
                        }

                    case "Boolean":
                        {
                            writer.WriteAttribute(tag, MakeValue<bool>(value));
                            break;
                        }

                    case "String":
                        {
                            writer.WriteAttribute(tag, DMD.Strings.CStr(value));
                            break;
                        }

                    default:
                        {
                            Debug.Print(Sistema.vbTypeName(value));
                            break;
                        }
                }
            }

            private T? MakeValue<T>(object v) where T : struct
            {
                return (T?)v;
            }

            private object GetValue(object v, string tName)
            {
                switch (tName ?? "")
                {
                    case "Byte":
                    case "SByte":
                        {
                            return DMD.XML.Utils.Serializer.DeserializeInteger(v);
                        }

                    case "Short":
                    case "UShort":
                    case "Int16":
                    case "UInt16":
                        {
                            return DMD.XML.Utils.Serializer.DeserializeInteger(v);
                        }

                    case "Integer":
                    case "UInteger":
                    case "Int32":
                    case "UInt32":
                        {
                            return DMD.XML.Utils.Serializer.DeserializeInteger(v);
                        }

                    case "Long":
                    case "ULong":
                    case var @case when @case == "Int16":
                    case "UInt64":
                        {
                            return DMD.XML.Utils.Serializer.DeserializeLong(DMD.Strings.CStr(v));
                        }

                    case "Single":
                    case "Double":
                        {
                            return DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(v));
                        }

                    case "Decimal":
                        {
                            return DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(v));
                        }

                    case "Date":
                    case "DateTime":
                        {
                            return DMD.XML.Utils.Serializer.DeserializeDate(v);
                        }

                    case "Boolean":
                        {
                            return DMD.XML.Utils.Serializer.DeserializeBoolean(v);
                        }

                    case "String":
                        {
                            return DMD.Strings.CStr(v);
                        }

                    default:
                        {
                            return null;
                        }
                }
            }

            ~PropertyChangedEventArgs()
            {
                DMDObject.DecreaseCounter(this);
            }
        }








        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        private Databases()
        {
            DMDObject.IncreaseCounter(this);
        }

        public static int GetID(IDBObjectBase value, int defaultID = 0)
        {
            if (value is null)
                return defaultID;
            return value.ID;
        }

        ~Databases()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}