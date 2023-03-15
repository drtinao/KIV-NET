using System;

namespace DrSearch
{
    /// <summary>
    /// Contains only static methods, which are used in various stages of preprocessing. Most of methods rewritten from KIV/IR - Java templates.
    /// </summary>
    class PreprocessingTools
    {
        /// <summary>
        /// Deletes derivational from input.
        /// </summary>
        /// <param name="strToModify">input string, which should be modified</param>
        /// <returns>input text WO derivational</returns>
        public static String removeDerivational(String strToModify)
        {
            int inputStrLen = strToModify.Length;
            if ((inputStrLen > 8) && strToModify.Substring(inputStrLen - 6).Equals("obinec"))
            {
                strToModify = strToModify.Remove(inputStrLen - 6);
                return strToModify;
            }
            if (inputStrLen > 7)
            {
                if (strToModify.Substring(inputStrLen - 5).Equals("ion\u00e1\u0159"))
                { // -ionář
                    strToModify = strToModify.Remove(inputStrLen - 4);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 5).Equals("ovisk") ||
                        strToModify.Substring(inputStrLen - 5).Equals("ovstv") ||
                        strToModify.Substring(inputStrLen - 5).Equals("ovi\u0161t") ||  //-ovišt
                        strToModify.Substring(inputStrLen - 5).Equals("ovn\u00edk"))
                { //-ovník
                    strToModify = strToModify.Remove(inputStrLen - 5);
                    return strToModify;
                }
            }//inputStrLen>7
            if (inputStrLen > 6)
            {
                if (strToModify.Substring(inputStrLen - 4).Equals("\u00e1sek") || // -ásek
                        strToModify.Substring(inputStrLen - 4).Equals("loun") ||
                        strToModify.Substring(inputStrLen - 4).Equals("nost") ||
                        strToModify.Substring(inputStrLen - 4).Equals("teln") ||
                        strToModify.Substring(inputStrLen - 4).Equals("ovec") ||
                        strToModify.Substring(inputStrLen - 5).Equals("ov\u00edk") || //-ovík
                        strToModify.Substring(inputStrLen - 4).Equals("ovtv") ||
                        strToModify.Substring(inputStrLen - 4).Equals("ovin") ||
                        strToModify.Substring(inputStrLen - 4).Equals("\u0161tin"))
                { //-štin
                    strToModify = strToModify.Remove(inputStrLen - 4);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 4).Equals("enic") ||
                        strToModify.Substring(inputStrLen - 4).Equals("inec") ||
                        strToModify.Substring(inputStrLen - 4).Equals("itel"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 3);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
            }//inputStrLen>6
            if (inputStrLen > 5)
            {
                if (strToModify.Substring(inputStrLen - 3).Equals("\u00e1rn"))
                { //-árn

                    strToModify = strToModify.Remove(inputStrLen - 3);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 3).Equals("\u011bnk"))
                { //-ěnk
                    strToModify = strToModify.Remove(inputStrLen - 2);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 3).Equals("i\u00e1n") || //-ián
                        strToModify.Substring(inputStrLen - 3).Equals("ist") ||
                        strToModify.Substring(inputStrLen - 3).Equals("isk") ||
                        strToModify.Substring(inputStrLen - 3).Equals("i\u0161t") || //-išt
                        strToModify.Substring(inputStrLen - 3).Equals("itb") ||
                        strToModify.Substring(inputStrLen - 3).Equals("\u00edrn"))
                {  //-írn

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 3).Equals("och") ||
                        strToModify.Substring(inputStrLen - 3).Equals("ost") ||
                        strToModify.Substring(inputStrLen - 3).Equals("ovn") ||
                        strToModify.Substring(inputStrLen - 3).Equals("oun") ||
                        strToModify.Substring(inputStrLen - 3).Equals("out") ||
                        strToModify.Substring(inputStrLen - 3).Equals("ou\u0161"))
                {  //-ouš
                    strToModify = strToModify.Remove(inputStrLen - 3);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 3).Equals("u\u0161k"))
                { //-ušk

                    strToModify = strToModify.Remove(inputStrLen - 3);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 3).Equals("kyn") ||
                        strToModify.Substring(inputStrLen - 3).Equals("\u010dan") ||    //-čan
                        strToModify.Substring(inputStrLen - 3).Equals("k\u00e1\u0159") || //kář
                        strToModify.Substring(inputStrLen - 3).Equals("n\u00e9\u0159") || //néř
                        strToModify.Substring(inputStrLen - 3).Equals("n\u00edk") ||      //-ník
                        strToModify.Substring(inputStrLen - 3).Equals("ctv") ||
                        strToModify.Substring(inputStrLen - 3).Equals("stv"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 3);
                    return strToModify;
                }
            }//inputStrLen>5
            if (inputStrLen > 4)
            {
                if (strToModify.Substring(inputStrLen - 2).Equals("\u00e1\u010d") || // -áč
                        strToModify.Substring(inputStrLen - 2).Equals("a\u010d") ||      //-ač
                        strToModify.Substring(inputStrLen - 2).Equals("\u00e1n") ||      //-án
                        strToModify.Substring(inputStrLen - 2).Equals("an") ||
                        strToModify.Substring(inputStrLen - 2).Equals("\u00e1\u0159") || //-ář
                        strToModify.Substring(inputStrLen - 2).Equals("as"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 2).Equals("ec") ||
                        strToModify.Substring(inputStrLen - 2).Equals("en") ||
                        strToModify.Substring(inputStrLen - 2).Equals("\u011bn") ||   //-ěn
                        strToModify.Substring(inputStrLen - 2).Equals("\u00e9\u0159"))
                {  //-éř

                    strToModify = strToModify.Remove(inputStrLen - 1);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 2).Equals("\u00ed\u0159") || //-íř
                        strToModify.Substring(inputStrLen - 2).Equals("ic") ||
                        strToModify.Substring(inputStrLen - 2).Equals("in") ||
                        strToModify.Substring(inputStrLen - 2).Equals("\u00edn") ||  //-ín
                        strToModify.Substring(inputStrLen - 2).Equals("it") ||
                        strToModify.Substring(inputStrLen - 2).Equals("iv"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 1);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }

                if (strToModify.Substring(inputStrLen - 2).Equals("ob") ||
                        strToModify.Substring(inputStrLen - 2).Equals("ot") ||
                        strToModify.Substring(inputStrLen - 2).Equals("ov") ||
                        strToModify.Substring(inputStrLen - 2).Equals("o\u0148"))
                { //-oň

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 2).Equals("ul"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 2).Equals("yn"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 2).Equals("\u010dk") ||              //-čk
                        strToModify.Substring(inputStrLen - 2).Equals("\u010dn") ||  //-čn
                        strToModify.Substring(inputStrLen - 2).Equals("dl") ||
                        strToModify.Substring(inputStrLen - 2).Equals("nk") ||
                        strToModify.Substring(inputStrLen - 2).Equals("tv") ||
                        strToModify.Substring(inputStrLen - 2).Equals("tk") ||
                        strToModify.Substring(inputStrLen - 2).Equals("vk"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    return strToModify;
                }
            }//inputStrLen>4
            if (inputStrLen > 3)
            {
                if (strToModify[strToModify.Length - 1] == 'c' ||
                        strToModify[strToModify.Length - 1] == '\u010d' || //-č
                        strToModify[strToModify.Length - 1] == 'k' ||
                        strToModify[strToModify.Length - 1] == 'l' ||
                        strToModify[strToModify.Length - 1] == 'n' ||
                        strToModify[strToModify.Length - 1] == 't')
                {
                    strToModify = strToModify.Remove(inputStrLen - 1);
                }
            }//inputStrLen>3
            return strToModify;
        }

        /// <summary>
        /// Removes ci ce and others from input.
        /// </summary>
        /// <param name="strToModify">input string, which should be modified</param>
        /// <returns>input text WO ci ce and so on</returns>
        public static String palatalise(String strToModify)
        {
            int inputStrLen = strToModify.Length;

            if (strToModify.Substring(inputStrLen - 2).Equals("ci") ||
                    strToModify.Substring(inputStrLen - 2).Equals("ce") ||
                    strToModify.Substring(inputStrLen - 2).Equals("\u010di") ||      //-či
                    strToModify.Substring(inputStrLen - 2).Equals("\u010de"))
            {   //-če

                strToModify = strToModify.Substring(0, inputStrLen - 2);
                strToModify += 'k';
                return strToModify;
            }
            if (strToModify.Substring(inputStrLen - 2).Equals("zi") ||
                    strToModify.Substring(inputStrLen - 2).Equals("ze") ||
                    strToModify.Substring(inputStrLen - 2).Equals("\u017ei") ||    //-ži
                    strToModify.Substring(inputStrLen - 2).Equals("\u017ee"))
            {  //-že

                strToModify = strToModify.Substring(0, inputStrLen - 2);
                strToModify += 'h';
                return strToModify;
            }
            if (strToModify.Substring(inputStrLen - 3).Equals("\u010dt\u011b") ||     //-čtě
                    strToModify.Substring(inputStrLen - 3).Equals("\u010dti") ||   //-čti
                    strToModify.Substring(inputStrLen - 3).Equals("\u010dt\u00ed"))
            {   //-čtí

                strToModify = strToModify.Substring(0, inputStrLen - 3);
                strToModify += "ck";
                return strToModify;
            }
            if (strToModify.Substring(inputStrLen - 2).Equals("\u0161t\u011b") ||   //-ště
                    strToModify.Substring(inputStrLen - 2).Equals("\u0161ti") ||   //-šti
                    strToModify.Substring(inputStrLen - 2).Equals("\u0161t\u00ed"))
            {  //-ští

                strToModify = strToModify.Substring(0, inputStrLen - 2);
                strToModify += "sk";
                return strToModify;
            }
            strToModify = strToModify.Remove(inputStrLen - 1);
            return strToModify;
        }

        /// <summary>
        /// Removes endings from words.
        /// </summary>
        /// <param name="strToModify">input string, which should be modified</param>
        /// <returns>input text WO endings</returns>
        public static String removeCase(String strToModify)
        {
            int inputStrLen = strToModify.Length;
            //
            if ((inputStrLen > 7) &&
                    strToModify.Substring(inputStrLen - 5).Equals("atech"))
            {
                strToModify = strToModify.Remove(inputStrLen - 5);
                return strToModify;
            }//inputStrLen>7
            if (inputStrLen > 6)
            {
                if (strToModify.Substring(inputStrLen - 4).Equals("\u011btem"))
                {   //-ětem

                    strToModify = strToModify.Remove(inputStrLen - 3);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 4).Equals("at\u016fm"))
                {  //-atům
                    strToModify = strToModify.Remove(inputStrLen - 4);
                    return strToModify;
                }

            }
            if (inputStrLen > 5)
            {
                if (strToModify.Substring(inputStrLen - 3).Equals("ech") ||
                        strToModify.Substring(inputStrLen - 3).Equals("ich") ||
                        strToModify.Substring(inputStrLen - 3).Equals("\u00edch"))
                { //-ích

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 3).Equals("\u00e9ho") || //-ého
                        strToModify.Substring(inputStrLen - 3).Equals("\u011bmi") ||  //-ěmu
                        strToModify.Substring(inputStrLen - 3).Equals("emi") ||
                        strToModify.Substring(inputStrLen - 3).Equals("\u00e9mu") ||  // -ému				                                                                strToModify.Substring( inputStrLen-3,inputStrLen).Equals("ete")||
                        strToModify.Substring(inputStrLen - 3).Equals("eti") ||
                        strToModify.Substring(inputStrLen - 3).Equals("iho") ||
                        strToModify.Substring(inputStrLen - 3).Equals("\u00edho") ||  //-ího
                        strToModify.Substring(inputStrLen - 3).Equals("\u00edmi") ||  //-ími
                        strToModify.Substring(inputStrLen - 3).Equals("imu"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 3).Equals("\u00e1ch") || //-ách
                        strToModify.Substring(inputStrLen - 3).Equals("ata") ||
                        strToModify.Substring(inputStrLen - 3).Equals("aty") ||
                        strToModify.Substring(inputStrLen - 3).Equals("\u00fdch") ||   //-ých
                        strToModify.Substring(inputStrLen - 3).Equals("ama") ||
                        strToModify.Substring(inputStrLen - 3).Equals("ami") ||
                        strToModify.Substring(inputStrLen - 3).Equals("ov\u00e9") ||   //-ové
                        strToModify.Substring(inputStrLen - 3).Equals("ovi") ||
                        strToModify.Substring(inputStrLen - 3).Equals("\u00fdmi"))
                {  //-ými

                    strToModify = strToModify.Remove(inputStrLen - 3);
                    return strToModify;
                }
            }
            if (inputStrLen > 4)
            {
                if (strToModify.Substring(inputStrLen - 2).Equals("em"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 1);
                    strToModify = palatalise(strToModify);
                    return strToModify;

                }
                if (strToModify.Substring(inputStrLen - 2).Equals("es") ||
                        strToModify.Substring(inputStrLen - 2).Equals("\u00e9m") ||    //-ém
                        strToModify.Substring(inputStrLen - 2).Equals("\u00edm"))
                {   //-ím

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 2).Equals("\u016fm"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 2).Equals("at") ||
                        strToModify.Substring(inputStrLen - 2).Equals("\u00e1m") ||    //-ám
                        strToModify.Substring(inputStrLen - 2).Equals("os") ||
                        strToModify.Substring(inputStrLen - 2).Equals("us") ||
                        strToModify.Substring(inputStrLen - 2).Equals("\u00fdm") ||     //-ým
                        strToModify.Substring(inputStrLen - 2).Equals("mi") ||
                        strToModify.Substring(inputStrLen - 2).Equals("ou"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    return strToModify;
                }
            }//inputStrLen>4
            if (inputStrLen > 3)
            {
                if (strToModify.Substring(inputStrLen - 1).Equals("e") ||
                        strToModify.Substring(inputStrLen - 1).Equals("i"))
                {

                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 1).Equals("\u00ed") ||    //-é
                        strToModify.Substring(inputStrLen - 1).Equals("\u011b"))
                {   //-ě

                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 1).Equals("u") ||
                        strToModify.Substring(inputStrLen - 1).Equals("y") ||
                        strToModify.Substring(inputStrLen - 1).Equals("\u016f"))
                {   //-ů

                    strToModify = strToModify.Remove(inputStrLen - 1);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 1).Equals("a") ||
                        strToModify.Substring(inputStrLen - 1).Equals("o") ||
                        strToModify.Substring(inputStrLen - 1).Equals("\u00e1") ||  // -á
                        strToModify.Substring(inputStrLen - 1).Equals("\u00e9") ||  //-é
                        strToModify.Substring(inputStrLen - 1).Equals("\u00fd"))
                {   //-ý

                    strToModify = strToModify.Remove(inputStrLen - 1);
                    return strToModify;
                }
            }//inputStrLen>3
            return strToModify;
        }

        /// <summary>
        /// Removes possessives from input.
        /// </summary>
        /// <param name="strToModify">input string, which should be modified</param>
        /// <returns>input text WO possessives</returns>
        public static String removePossessives(String strToModify)
        {
            int inputStrLen = strToModify.Length;

            if (inputStrLen > 5)
            {
                if (strToModify.Substring(inputStrLen - 2).Equals("ov"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 2).Equals("\u016fv"))
                { //-ův

                    strToModify = strToModify.Remove(inputStrLen - 2);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 2).Equals("in"))
                {
                    strToModify = strToModify.Remove(inputStrLen - 1);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
            }
            return strToModify;
        }//removePossessives

        /// <summary>
        /// Removes augmentatives from input.
        /// </summary>
        /// <param name="strToModify">input string, which should be modified</param>
        /// <returns>input text WO augmentatives</returns>
        public static String removeAugmentative(String strToModify)
        {
            int inputStrLen = strToModify.Length;

            //
            if ((inputStrLen > 6) &&
                    strToModify.Substring(inputStrLen - 4).Equals("ajzn"))
            {

                strToModify = strToModify.Remove(inputStrLen - 4);
                return strToModify;
            }
            if ((inputStrLen > 5) &&
                    (strToModify.Substring(inputStrLen - 3).Equals("izn") ||
                            strToModify.Substring(inputStrLen - 3).Equals("isk")))
            {

                strToModify = strToModify.Remove(inputStrLen - 2);
                strToModify = palatalise(strToModify);
                return strToModify;
            }
            if ((inputStrLen > 4) &&
                    strToModify.Substring(inputStrLen - 2).Equals("\00e1k"))
            { //-ák

                strToModify = strToModify.Remove(inputStrLen - 2);
                return strToModify;
            }
            return strToModify;
        }

        /// <summary>
        /// Deletes diminutives from input.
        /// </summary>
        /// <param name="strToModify">input string, which should be modified</param>
        /// <returns>input text WO diminutives</returns>
        public static String removeDiminutive(String strToModify)
        {
            int inputStrLen = strToModify.Length;
            //
            if ((inputStrLen > 7) &&
                    strToModify.Substring(inputStrLen - 5).Equals("ou\u0161ek"))
            {  //-oušek

                strToModify = strToModify.Remove(inputStrLen - 5);
                return strToModify;
            }
            if (inputStrLen > 6)
            {
                if (strToModify.Substring(inputStrLen - 4).Equals("e\u010dek") ||      //-eček
                        strToModify.Substring(inputStrLen - 4).Equals("\u00e9\u010dek") ||    //-éček
                        strToModify.Substring(inputStrLen - 4).Equals("i\u010dek") ||         //-iček
                        strToModify.Substring(inputStrLen - 4).Equals("\u00ed\u010dek") ||    //íček
                        strToModify.Substring(inputStrLen - 4).Equals("enek") ||
                        strToModify.Substring(inputStrLen - 4).Equals("\u00e9nek") ||      //-ének
                        strToModify.Substring(inputStrLen - 4).Equals("inek") ||
                        strToModify.Substring(inputStrLen - 4).Equals("\u00ednek"))
                {      //-ínek

                    strToModify = strToModify.Remove(inputStrLen - 3);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 4).Equals("\u00e1\u010dek") || //áček
                        strToModify.Substring(inputStrLen - 4).Equals("a\u010dek") ||   //aček
                        strToModify.Substring(inputStrLen - 4).Equals("o\u010dek") ||   //oček
                        strToModify.Substring(inputStrLen - 4).Equals("u\u010dek") ||   //uček
                        strToModify.Substring(inputStrLen - 4).Equals("anek") ||
                        strToModify.Substring(inputStrLen - 4).Equals("onek") ||
                        strToModify.Substring(inputStrLen - 4).Equals("unek") ||
                        strToModify.Substring(inputStrLen - 4).Equals("\u00e1nek"))
                {   //-ánek

                    strToModify = strToModify.Remove(inputStrLen - 4);
                    return strToModify;
                }
            }//inputStrLen>6
            if (inputStrLen > 5)
            {
                if (strToModify.Substring(inputStrLen - 3).Equals("e\u010dk") ||   //-ečk
                        strToModify.Substring(inputStrLen - 3).Equals("\u00e9\u010dk") ||  //-éčk
                        strToModify.Substring(inputStrLen - 3).Equals("i\u010dk") ||   //-ičk
                        strToModify.Substring(inputStrLen - 3).Equals("\u00ed\u010dk") ||    //-íčk
                        strToModify.Substring(inputStrLen - 3).Equals("enk") ||   //-enk
                        strToModify.Substring(inputStrLen - 3).Equals("\u00e9nk") ||  //-énk
                        strToModify.Substring(inputStrLen - 3).Equals("ink") ||   //-ink
                        strToModify.Substring(inputStrLen - 3).Equals("\u00ednk"))
                {   //-ínk

                    strToModify = strToModify.Remove(inputStrLen - 3);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 3).Equals("\u00e1\u010dk") ||  //-áčk
                        strToModify.Substring(inputStrLen - 3).Equals("au010dk") || //-ačk
                        strToModify.Substring(inputStrLen - 3).Equals("o\u010dk") ||  //-očk
                        strToModify.Substring(inputStrLen - 3).Equals("u\u010dk") ||   //-učk
                        strToModify.Substring(inputStrLen - 3).Equals("ank") ||
                        strToModify.Substring(inputStrLen - 3).Equals("onk") ||
                        strToModify.Substring(inputStrLen - 3).Equals("unk"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 3);
                    return strToModify;

                }
                if (strToModify.Substring(inputStrLen - 3).Equals("\u00e1tk") || //-átk
                        strToModify.Substring(inputStrLen - 3).Equals("\u00e1nk") ||  //-ánk
                        strToModify.Substring(inputStrLen - 3).Equals("u\u0161k"))
                {   //-ušk

                    strToModify = strToModify.Remove(inputStrLen - 3);
                    return strToModify;
                }
            }//inputStrLen>5
            if (inputStrLen > 4)
            {
                if (strToModify.Substring(inputStrLen - 2).Equals("ek") ||
                        strToModify.Substring(inputStrLen - 2).Equals("\u00e9k") ||  //-ék
                        strToModify.Substring(inputStrLen - 2).Equals("\u00edk") ||  //-ík
                        strToModify.Substring(inputStrLen - 2).Equals("ik"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 1);
                    strToModify = palatalise(strToModify);
                    return strToModify;
                }
                if (strToModify.Substring(inputStrLen - 2).Equals("\u00e1k") ||  //-ák
                        strToModify.Substring(inputStrLen - 2).Equals("ak") ||
                        strToModify.Substring(inputStrLen - 2).Equals("ok") ||
                        strToModify.Substring(inputStrLen - 2).Equals("uk"))
                {

                    strToModify = strToModify.Remove(inputStrLen - 1);
                    return strToModify;
                }
            }
            if ((inputStrLen > 3) &&
                    strToModify.Substring(inputStrLen - 1).Equals("k"))
            {

                strToModify = strToModify.Remove(inputStrLen - 1);
                return strToModify;
            }

            return strToModify;
        }//removeDiminutives

        /// <summary>
        /// Deletes comparatives from input.
        /// </summary>
        /// <param name="strToModify">input string, which should be modified</param>
        /// <returns>input text WO comparatives</returns>
        public static String removeComparative(String strToModify)
        {
            int inputStrLen = strToModify.Length;
            //
            if ((inputStrLen > 5) &&
                    (strToModify.Substring(inputStrLen - 3).Equals("ej\u0161") ||  //-ejš
                            strToModify.Substring(inputStrLen - 3).Equals("\u011bj\u0161")))
            {   //-ějš

                strToModify = strToModify.Remove(inputStrLen - 2);
                strToModify = palatalise(strToModify);
                return strToModify;
            }
            return strToModify;
        }
    }
}
