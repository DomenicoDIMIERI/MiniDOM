using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;


namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Rappresenta un punto della mappa del territorio attraversato dall'utente per la commissione
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class GPSRecord 
            : minidom.Databases.DBObjectBase
        {
            /// <summary>
            /// Dimensione media del raggion terrestre
            /// </summary>
            public const double EARTHRADIUS = 6371000d;
            
            
            [NonSerialized] private Dispositivo m_Dispositivo;
            private int m_IDDispositivo;
            private DateTime m_Istante1;                       // Data 
            private DateTime m_Istante2;                       // Data 
            private double m_Latitudine;                  // Latitudine
            private double m_Longitudine;                 // Longitudine
            private double m_Altitudine;                  // Altitudine
            private double m_Bearing;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public GPSRecord()
            {
                m_Dispositivo = null;
                m_IDDispositivo = 0;
                m_Istante1 = DMD.DateUtils.Now();
                m_Istante2 = DMD.DateUtils.Now();
                m_Latitudine = 0d;
                m_Longitudine = 0d;
                m_Altitudine = 0d;
                m_Bearing = 0d;
                m_Flags = 0;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="lat"></param>
            /// <param name="lon"></param>
            public GPSRecord(double lat, double lon) 
                : this()
            {
                m_Latitudine = lat;
                m_Longitudine = lon;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="lat"></param>
            /// <param name="lon"></param>
            /// <param name="alt"></param>
            public GPSRecord(double lat, double lon, double alt) 
                : this()
            {
                m_Latitudine = lat;
                m_Longitudine = lon;
                m_Altitudine = alt;
            }

            /// <summary>
            /// Angolo in gradi da Nord
            /// </summary>
            public double Bearing
            {
                get
                {
                    return m_Bearing;
                }

                set
                {
                    double oldValue = m_Bearing;
                    if (oldValue == value)
                        return;
                    m_Bearing = value;
                    DoChanged("Bearing", value, oldValue);
                }
            }
             



            /// <summary>
            /// Restituisce o imposta il dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Dispositivo Dispositivo
            {
                get
                {
                    if (m_Dispositivo is null)
                        m_Dispositivo = Dispositivi.GetItemById(m_IDDispositivo);
                    return m_Dispositivo;
                }

                set
                {
                    var oldValue = Dispositivo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Dispositivo = value;
                    m_IDDispositivo = DBUtils.GetID(value, 0);
                    DoChanged("Dispositivo", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il dispositivo
            /// </summary>
            /// <param name="dispositivo"></param>
            protected internal void SetDispositivo(Dispositivo dispositivo)
            {
                m_Dispositivo = dispositivo;
                m_IDDispositivo = DBUtils.GetID(dispositivo, 0);
            }

            /// <summary>
            /// Restituisce o imposta l'ID del dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDDispositivo
            {
                get
                {
                    return DBUtils.GetID(m_Dispositivo, m_IDDispositivo);
                }

                set
                {
                    int oldValue = IDDispositivo;
                    if (oldValue == value)
                        return;
                    m_IDDispositivo = value;
                    m_Dispositivo = null;
                    DoChanged("Dispositivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'istante in cui è stato memorizzata la posizione per la prima volta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime Istante1
            {
                get
                {
                    return m_Istante1;
                }

                set
                {
                    var oldValue = m_Istante1;
                    if (oldValue == value)
                        return;
                    m_Istante1 = value;
                    DoChanged("Istante1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'istante in cui è stato memorizzata la posizione per l'ultima volta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime Istante2
            {
                get
                {
                    return m_Istante2;
                }

                set
                {
                    var oldValue = m_Istante1;
                    if (oldValue == value)
                        return;
                    m_Istante2 = value;
                    DoChanged("Istante2", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la latitudine
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double Latitudine
            {
                get
                {
                    return m_Latitudine;
                }

                set
                {
                    double oldValue = m_Latitudine;
                    if (oldValue == value)
                        return;
                    m_Latitudine = value;
                    DoChanged("Latitudine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la longitudine
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double Longitudine
            {
                get
                {
                    return m_Longitudine;
                }

                set
                {
                    double oldValue = m_Longitudine;
                    if (oldValue == value)
                        return;
                    m_Longitudine = value;
                    DoChanged("Longitudine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'altitudine
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double Altitudine
            {
                get
                {
                    return m_Altitudine;
                }

                set
                {
                    double oldValue = m_Altitudine;
                    if (oldValue == value)
                        return;
                    m_Altitudine = value;
                    DoChanged("Altitudine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("{ " , m_Latitudine , ", " , m_Longitudine, ", ", this.m_Altitudine,  " }");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.Radius);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is GPSRecord) && this.Equals((GPSRecord)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(GPSRecord obj)
            {
                return base.Equals(obj)
                       && DMD.Integers.EQ(this.m_IDDispositivo, obj.m_IDDispositivo)
                       && DMD.DateUtils.EQ(this.m_Istante1, obj.m_Istante1)
                       && DMD.DateUtils.EQ(this.m_Istante2, obj.m_Istante2)
                       && DMD.Doubles.EQ(this.m_Latitudine, obj.m_Latitudine)
                       && DMD.Doubles.EQ(this.m_Longitudine, obj.m_Longitudine)
                       && DMD.Doubles.EQ(this.m_Altitudine, obj.m_Altitudine)
                       && DMD.Doubles.EQ(this.m_Bearing, obj.m_Bearing)
                        ;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.GPSRecords;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeGPS";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_IDDispositivo = reader.Read("IDDispositivo", this.m_IDDispositivo);
                this.m_Latitudine = reader.Read("Latitudine", this.m_Latitudine);
                this.m_Longitudine = reader.Read("Longitudine", this.m_Longitudine);
                this.m_Altitudine = reader.Read("Altitudine", this.m_Altitudine);
                this.m_Bearing = reader.Read("Bearing", this.m_Bearing);
                this.m_Istante1 = reader.Read("Istante1", this.m_Istante1);
                this.m_Istante2 = reader.Read("Istante2", this.m_Istante2);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDDispositivo", IDDispositivo);
                writer.Write("Latitudine", m_Latitudine);
                writer.Write("Longitudine", m_Longitudine);
                writer.Write("Altitudine", m_Altitudine);
                writer.Write("Bearing", m_Bearing);
                writer.Write("Istante1", m_Istante1);
                writer.Write("Istante2", m_Istante2);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDDispositivo", typeof(int), 1);
                c = table.Fields.Ensure("Latitudine", typeof(double), 1);
                c = table.Fields.Ensure("Longitudine", typeof(double), 1);
                c = table.Fields.Ensure("Altitudine", typeof(double), 1);
                c = table.Fields.Ensure("Bearing", typeof(double), 1);
                c = table.Fields.Ensure("Istante1", typeof(DateTime), 1);
                c = table.Fields.Ensure("Istante2", typeof(DateTime), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxDispositivo", new string[] { "IDDispositivo", "Istante1", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxPosizione", new string[] { "Latitudine", "Longitudine", "Altitudine" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxParams", new string[] { "Bearing", "Istante2" }, DBFieldConstraintFlags.PrimaryKey);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDDispositivo", IDDispositivo);
                writer.WriteAttribute("Latitudine", m_Latitudine);
                writer.WriteAttribute("Longitudine", m_Longitudine);
                writer.WriteAttribute("Altitudine", m_Altitudine);
                writer.WriteAttribute("Bearing", m_Bearing);
                writer.WriteAttribute("Istante1", m_Istante1);
                writer.WriteAttribute("Istante2", m_Istante2);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDDispositivo":
                        {
                            m_IDDispositivo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Latitudine":
                        {
                            m_Latitudine = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Longitudine":
                        {
                            m_Longitudine = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Altitudine":
                        {
                            m_Altitudine = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Bearing":
                        {
                            m_Bearing = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Istante1":
                        {
                            m_Istante1 = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Istante2":
                        {
                            m_Istante2 = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }
 
                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Restituisce la distanza del punto dal centro della terra
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double Radius
            {
                get
                {
                    return EARTHRADIUS + m_Altitudine;
                }

                set
                {
                    double oldValue = Radius;
                    if (oldValue == value)
                        return;
                    m_Altitudine = value - EARTHRADIUS;
                    DoChanged("Radius", value, oldValue);
                }
            }

            /// <summary>
            /// Calcola la distanza sul globo tra il punto corrente ed il punto specificato
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public double DistanceTo(GPSRecord p)
            {
                double R = Radius;
                double lat1 = Maths.toRadians(m_Latitudine);
                double lon1 = Maths.toRadians(m_Longitudine);
                double lat2 = Maths.toRadians(p.m_Latitudine);
                double lon2 = Maths.toRadians(p.m_Longitudine);
                double dLat = lat2 - lat1;
                double dLon = lon2 - lon1;
                double a = Maths.Sin(dLat / 2d) * Maths.Sin(dLat / 2d) + Maths.Cos(lat1) * Maths.Cos(lat2) * Maths.Sin(dLon / 2d) * Maths.Sin(dLon / 2d);
                double c = 2d * Maths.atan2(Maths.Sqrt(a), Maths.Sqrt(1d - a));
                return R * c;
            }

            /// <summary>
            /// Returns the (initial) bearing from this point to the supplied point, in degrees
            /// see http://williams.best.vwh.net/avform.htm#Crs
            /// * @param   {LatLon} point: Latitude/longitude of destination point
            /// </summary>
            /// <param name="point"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public double bearingTo(GPSRecord point)
            {
                double lat1 = Maths.toRadians(m_Latitudine);
                double lat2 = Maths.toRadians(point.m_Latitudine);
                double dLon = Maths.toRadians(point.m_Longitudine - m_Longitudine);
                float y = (float)(Maths.Sin(dLon) * Maths.Cos(lat2));
                float x = (float)(Maths.Cos(lat1) * Maths.Sin(lat2) - Maths.Sin(lat1) * Maths.Cos(lat2) * Maths.Cos(dLon));
                double brng = Maths.atan2(y, x);
                return Maths.toDegrees(brng + 360d) % 360d;
            }

            /// <summary>
            /// Returns final bearing arriving at supplied destination point from this point; the final bearing 
            /// will differ from the initial bearing by varying degrees according to distance and latitude
            ///</summary> 
            /// <param name="point">Latitude/longitude of destination point</param>
            /// <returns></returns>
            public double finalBearingTo(GPSRecord point)
            {
                // get initial bearing from supplied point back to this point...
                double lat1 = Maths.toRadians(point.m_Latitudine);
                double lat2 = Maths.toRadians(m_Latitudine);
                double dLon = Maths.toRadians(m_Longitudine - point.m_Longitudine);
                double y = Maths.Sin(dLon) * Maths.Cos(lat2);
                double x = Maths.Cos(lat1) * Maths.Sin(lat2) - Maths.Sin(lat1) * Maths.Cos(lat2) * Maths.Cos(dLon);
                double brng = Maths.atan2(y, x);
                // ... & reverse it by adding 180Â°
                return (Maths.toDegrees(brng) + 180d) % 360d;
            }

            /// <summary>
            ///  Returns the midpoint between this point and the supplied point.
            ///  see http://mathforum.org/library/drmath/view/51822.html for derivation
            ///  
            /// * @param   {LatLon} point: Latitude/longitude of destination point
            /// * @returns {LatLon} Midpoint between this point and the supplied point
            /// </summary>
            /// <param name="point"></param>
            /// <returns></returns>
            public GPSRecord midpointTo(GPSRecord point)
            {
                double lat1 = Maths.toRadians(m_Latitudine);
                double lon1 = Maths.toRadians(m_Longitudine);
                double lat2 = Maths.toRadians(point.m_Latitudine);
                double dLon = Maths.toRadians(point.m_Longitudine - m_Longitudine);
                double Bx = Maths.Cos(lat2) * Maths.Cos(dLon);
                double By = Maths.Cos(lat2) * Maths.Sin(dLon);
                double lat3 = Maths.atan2(Maths.Sin(lat1) + Maths.Sin(lat2), Maths.Sqrt((Maths.Cos(lat1) + Bx) * (Maths.Cos(lat1) + Bx) + By * By));
                double lon3 = lon1 + Maths.atan2(By, Maths.Cos(lat1) + Bx);
                lon3 = (lon3 + 3d * Maths.PI) % (2d * Maths.PI) - Maths.PI; // normalise to -180..+180Âº
                return new GPSRecord(Maths.toDegrees(lat3), Maths.toDegrees(lon3));
            }



            /// <summary>
            /// Returns the destination point from this point having travelled the given distance (in km) on the 
            /// given initial bearing (bearing may vary before destination is reached)
            /// see http://williams.best.vwh.net/avform.htm#LL
            /// 
            /// @param   {Number} brng: Initial bearing in degrees
            /// @param   {Number} dist: Distance in km
            /// @returns {LatLon} Destination point
            /// </summary>
            /// <param name="brng"></param>
            /// <param name="dist"></param>
            /// <returns></returns>
            public GPSRecord destinationPoint(double brng, double dist)
            {
                // Dim dist = typeof(dist)=='number' ? dist : typeof(dist)=='string' && dist.trim()!='' ? +dist : NaN;
                dist = dist / Radius; // convert dist to angular distance in radians
                brng = Maths.toRadians(brng);
                double lat1 = Maths.toRadians(m_Latitudine);
                double lon1 = Maths.toRadians(m_Longitudine);
                double lat2 = Maths.asin(Maths.Sin(lat1) * Maths.Cos(dist) + Maths.Cos(lat1) * Maths.Sin(dist) * Maths.Cos(brng));
                double lon2 = lon1 + Maths.atan2(Maths.Sin(brng) * Maths.Sin(dist) * Maths.Cos(lat1), Maths.Cos(dist) - Maths.Sin(lat1) * Maths.Sin(lat2));
                lon2 = (lon2 + 3d * Maths.PI) % (2d * Maths.PI) - Maths.PI; // normalise to -180..+180Âº
                return new GPSRecord(Maths.toDegrees(lat2), Maths.toDegrees(lon2));
            }


            /// <summary>
            /// Returns the point of intersection of two paths defined by point and bearing
            /// see http://williams.best.vwh.net/avform.htm#Intersection
            /// @param   {LatLon} p1: First point
            /// @param   {Number} brng1: Initial bearing from first point
            /// @param   {LatLon} p2: Second point
            /// @param   {Number} brng2: Initial bearing from second point
            /// @returns {LatLon} Destination point (null if no unique intersection defined)
            /// </summary>
            /// <param name="p1"></param>
            /// <param name="brng1"></param>
            /// <param name="p2"></param>
            /// <param name="brng2"></param>
            /// <returns></returns>
            public static GPSRecord intersection(GPSRecord p1, double brng1, GPSRecord p2, double brng2)
            {
                // brng1 = typeof brng1 == 'number' ? brng1 : typeof brng1 == 'string' && trim(brng1)!='' ? +brng1 : NaN;
                // brng2 = typeof brng2 == 'number' ? brng2 : typeof brng2 == 'string' && trim(brng2)!='' ? +brng2 : NaN;
                double lat1 = Maths.toRadians(p1.m_Latitudine);
                double lon1 = Maths.toRadians(p1.m_Longitudine);
                double lat2 = Maths.toRadians(p2.m_Latitudine);
                double lon2 = Maths.toRadians(p2.m_Longitudine);
                double brng13 = Maths.toRadians(brng1);
                double brng23 = Maths.toRadians(brng2);
                double dLat = lat2 - lat1;
                double dLon = lon2 - lon1;
                double dist12 = 2 * Maths.asin(Maths.Sqrt(Maths.Sin(dLat / 2d) * Maths.Sin(dLat / 2d) + Maths.Cos(lat1) * Maths.Cos(lat2) * Maths.Sin(dLon / 2d) * Maths.Sin(dLon / 2d)));
                if (dist12 == 0d)
                    return null;

                // initial/final bearings between points
                double brngA = Maths.acos((Maths.Sin(lat2) - Maths.Sin(lat1) * Maths.Cos(dist12)) / (Maths.Sin(dist12) * Maths.Cos(lat1)));
                if (double.IsNaN(brngA))
                    brngA = 0d; // protect against rounding
                double brngB = Maths.acos((Maths.Sin(lat1) - Maths.Sin(lat2) * Maths.Cos(dist12)) / (Maths.Sin(dist12) * Maths.Cos(lat2)));
                double brng12;
                double brng21;
                if (Maths.Sin(lon2 - lon1) > 0d)
                {
                    brng12 = brngA;
                    brng21 = 2d * Maths.PI - brngB;
                }
                else
                {
                    brng12 = 2d * Maths.PI - brngA;
                    brng21 = brngB;
                }

                double alpha1 = (brng13 - brng12 + Maths.PI) % (2d * Maths.PI) - Maths.PI; // angle 2-1-3
                double alpha2 = (brng21 - brng23 + Maths.PI) % (2d * Maths.PI) - Maths.PI; // angle 1-2-3
                if (Maths.Sin(alpha1) == 0d && Maths.Sin(alpha2) == 0d)
                    return null; // infinite intersections
                if (Maths.Sin(alpha1) * Maths.Sin(alpha2) < 0d)
                    return null; // ambiguous intersection

                // alpha1 = Maths.abs(alpha1);
                // alpha2 = Maths.abs(alpha2);
                // ... Ed Williams takes abs of alpha1/alpha2, but seems to break calculation?

                double alpha3 = Maths.acos(-Maths.Cos(alpha1) * Maths.Cos(alpha2) + Maths.Sin(alpha1) * Maths.Sin(alpha2) * Maths.Cos(dist12));
                double dist13 = Maths.atan2(Maths.Sin(dist12) * Maths.Sin(alpha1) * Maths.Sin(alpha2), Maths.Cos(alpha2) + Maths.Cos(alpha1) * Maths.Cos(alpha3));
                double lat3 = Maths.asin(Maths.Sin(lat1) * Maths.Cos(dist13) + Maths.Cos(lat1) * Maths.Sin(dist13) * Maths.Cos(brng13));
                double dLon13 = Maths.atan2(Maths.Sin(brng13) * Maths.Sin(dist13) * Maths.Cos(lat1), Maths.Cos(dist13) - Maths.Sin(lat1) * Maths.Sin(lat3));
                double lon3 = lon1 + dLon13;
                lon3 = (lon3 + 3d * Maths.PI) % (2d * Maths.PI) - Maths.PI; // normalise to -180..+180Âº
                return new GPSRecord(Maths.toDegrees(lat3), Maths.toDegrees(lon3));
            }



            /// <summary>
            /// Returns the distance from this point to the supplied point, in km, travelling along a rhumb line
            /// see http://williams.best.vwh.net/avform.htm#Rhumb
            /// @param   {LatLon} point: Latitude/longitude of destination point
            /// @returns {Number} Distance in km between this point and destination point
            /// </summary>
            /// <param name="point"></param>
            /// <returns></returns>
            public double rhumbDistanceTo(GPSRecord point)
            {
                double R = Radius;
                double lat1 = Maths.toRadians(m_Latitudine);
                double lat2 = Maths.toRadians(point.m_Latitudine);
                double dLat = Maths.toRadians(point.m_Latitudine - m_Latitudine);
                double dLon = Maths.toRadians(Maths.Abs(point.m_Longitudine - m_Longitudine));
                double dPhi = Maths.Log(Maths.tan(lat2 / 2d + Maths.PI / 4d) / Maths.tan(lat1 / 2d + Maths.PI / 4d));
                double q;
                if (dPhi != 0d) // isFinite(dLat/dPhi)) 
                {
                    q = dLat / dPhi;
                }
                else
                {
                    q = Maths.Cos(lat1);
                } // E-W line gives dPhi=0

                // if dLon over 180Â° take shorter rhumb across anti-meridian:
                if (Maths.Abs(dLon) > Maths.PI)
                {
                    if (dLon > 0d)
                    {
                        dLon = -(2d * Maths.PI - dLon);
                    }
                    else
                    {
                        dLon = 2d * Maths.PI + dLon;
                    }
                }

                double dist = Maths.Sqrt(dLat * dLat + q * q * dLon * dLon) * R;
                return dist; // .toPrecisionFixed(4);  // 4 sig figs reflects typical 0.3% accuracy of spherical model
            }

            /// <summary>
            /// Returns the bearing from this point to the supplied point along a rhumb line, in degrees
            /// @param   {LatLon} point: Latitude/longitude of destination point
            /// @returns {Number} Bearing in degrees from North
            /// </summary>
            /// <param name="point"></param>
            /// <returns></returns>
            public double rhumbBearingTo(GPSRecord point)
            {
                double lat1 = Maths.toRadians(m_Latitudine);
                double lat2 = Maths.toRadians(point.m_Latitudine);
                double dLon = Maths.toRadians(point.m_Longitudine - m_Longitudine);
                double dPhi = Maths.Log(Maths.tan(lat2 / 2d + Maths.PI / 4d) / Maths.tan(lat1 / 2d + Maths.PI / 4d));
                if (Maths.Abs(dLon) > Maths.PI)
                {
                    if (dLon > 0d)
                    {
                        dLon = -(2d * Maths.PI - dLon);
                    }
                    else
                    {
                        dLon = 2d * Maths.PI + dLon;
                    }
                }

                double brng = Maths.atan2(dLon, dPhi);
                return (Maths.toDegrees(brng) + 360d) % 360d;
            }

            /// <summary>
            /// Returns the destination point from this point having travelled the given distance (in km) on the 
            /// given bearing along a rhumb line
            /// 
            /// @param   {Number} brng: Bearing in degrees from North
            /// @param   {Number} dist: Distance in km
            /// @returns {LatLon} Destination point
            /// </summary>
            /// <param name="brng"></param>
            /// <param name="dist"></param>
            /// <returns></returns>
            public GPSRecord rhumbDestinationPoint(double brng, double dist)
            {
                double R = Radius;
                double d = dist / R; // d = angular distance covered on earthâ€™s surface
                double lat1 = Maths.toRadians(m_Latitudine);
                double lon1 = Maths.toRadians(m_Longitudine);
                brng = Maths.toRadians(brng);
                double dLat = d * Maths.Cos(brng);
                // nasty kludge to overcome ill-conditioned results around parallels of latitude:
                if (Maths.Abs(dLat) < 0.0000000001d)
                    dLat = 0d; // dLat < 1 mm
                double lat2 = lat1 + dLat;
                double dPhi = Maths.Log(Maths.tan(lat2 / 2d + Maths.PI / 4d) / Maths.tan(lat1 / 2d + Maths.PI / 4d));
                double q;
                if (dPhi != 0d) // (isFinite(dLat/dPhi)) 
                {
                    q = dLat / dPhi;
                }
                else
                {
                    q = Maths.Cos(lat1);
                } // E-W line gives dPhi=0

                double dLon = d * Maths.Sin(brng) / q;

                // check for some daft bugger going past the pole, normalise latitude if so
                if (Maths.Abs(lat2) > Maths.PI / 2d)
                {
                    if (lat2 > 0d)
                    {
                        lat2 = Maths.PI - lat2;
                    }
                    else
                    {
                        lat2 = -Maths.PI - lat2;
                    }
                }

                double lon2 = (lon1 + dLon + 3d * Maths.PI) % (2d * Maths.PI) - Maths.PI;
                return new GPSRecord(Maths.toDegrees(lat2), Maths.toDegrees(lon2));
            }

            /// <summary>
            /// Returns the loxodromic midpoint (along a rhumb line) between this point and the supplied point.
            /// see http://mathforum.org/kb/message.jspa?messageID=148837
            /// @param   {LatLon} point: Latitude/longitude of destination point
            /// @returns {LatLon} Midpoint between this point and the supplied point
            /// </summary>
            /// <param name="point"></param>
            /// <returns></returns>
            public GPSRecord rhumbMidpointTo(GPSRecord point)
            {
                double lat1 = Maths.toRadians(m_Latitudine);
                double lon1 = Maths.toRadians(m_Longitudine);
                double lat2 = Maths.toRadians(point.m_Latitudine);
                double lon2 = Maths.toRadians(point.m_Longitudine);
                if (Maths.Abs(lon2 - lon1) > Maths.PI)
                    lon1 += 2d * Maths.PI; // crossing anti-meridian
                double lat3 = (lat1 + lat2) / 2d;
                double f1 = Maths.tan(Maths.PI / 4d + lat1 / 2d);
                double f2 = Maths.tan(Maths.PI / 4d + lat2 / 2d);
                double f3 = Maths.tan(Maths.PI / 4d + lat3 / 2d);
                double lon3;
                if (Maths.Log(f2 / f1) == 0d)
                {
                    lon3 = (lon1 + lon2) / 2d; // parallel of latitude
                }
                else
                {
                    lon3 = ((lon2 - lon1) * Maths.Log(f3) + lon1 * Maths.Log(f2) - lon2 * Maths.Log(f1)) / Maths.Log(f2 / f1);
                }

                lon3 = (lon3 + 3d * Maths.PI) % (2d * Maths.PI) - Maths.PI; // normalise to -180..+180Âº
                return new GPSRecord(Maths.toDegrees(lat3), Maths.toDegrees(lon3));
            }
        }
    }
}