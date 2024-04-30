using System.Text.RegularExpressions;

namespace utils {
    class CompReg {
        public static Regex rankPtrn = new Regex(
            "<div class=\"general_info_rank\"><a href=\"\\/Category:([a-zA-Z]*)_rank_ground_vehicles\" title=\"Category:\\1 rank ground vehicles\">([IVX]*) Rank<\\/a><\\/div>[\r\n]",
            RegexOptions.Compiled
        );
        public static Regex premiumPtrn = new Regex(
            "<a href=\"([^\"]*)\" title=\"([^\"]*)\">(PREMIUM)<\\/a>",
            RegexOptions.Compiled
        );
        public static Regex squadronPtrn = new Regex(
            "<a href=\"([^\"]*)\" title=\"([^\"]*)\">(SQUADRON)<\\/a>",
            RegexOptions.Compiled
        );
        public static Regex gePattern = new Regex(
            """<div class="general_info_price_buy"><span class="desc">Purchase:</span><span class="value">([0-9]* ?[0-9]*?) <a href="/Golden_Eagles" title="Golden Eagles"><img alt="Specs-Card-Eagle\.png" src="/images/f/f6/Specs-Card-Eagle\.png" width="24" height="24" data-file-width="24" data-file-height="24" /></a></span></div>\n</div>""",
            RegexOptions.Compiled
        );
        public static Regex giftPtrn = new Regex(
            """<a href="/Category:Gift_ground_vehicles" title="Category:Gift ground vehicles">Bundle or Gift</a>""",
            RegexOptions.Compiled
        );
        public static Regex marketPtrn = new Regex(
            """<span>MARKET</span>""",
            RegexOptions.Compiled
        );
        public static Regex packPtrn = new Regex(
            "<span>STORE</span>",
            RegexOptions.Compiled
        );
        public static Regex rolePtrn = new Regex(
            "<a href=\"\\/Category:([a-zA-Z_\\-]*)\" title=\"Category:([a-zA-Z \\-]*)\">(Light|Medium|Heavy|SPAA|Tank destroyer)( tanks?)?<\\/a>",
            RegexOptions.Compiled
        );
        public static Regex brPtrn = new Regex(
            "<td>([0-9.]{0,4})<\\/td>\n?"+ // AB
            "<td>([0-9.]{0,4})<\\/td>\n?"+ // RB
            "<td>([0-9.]{0,4})<\\/td>",    // SB
            RegexOptions.Compiled
        );
        public static Regex mobilityPtrn = new Regex(
            "<table class=\"wikitable\" style=\"text-align:center\" width=\"70%\">(.*?)<th> Arcade\n<\\/th>\n"+
                "<td> ([0-9.,]*)\n<\\/td>\n"+ // AB forward
                "<td> ([0-9.,]*)\n<\\/td>\n"+ // AB Reverse
                "<td rowspan=\"2\">([0-9.,]*)\n<\\/td>\n"+ // Weight
                "(<td rowspan=\"2\"> ([0-9.,]*)\n<\\/td>\n)?"+ // Add-on armor
                "<td> ([0-9._,]*)\n<\\/td>\n"+ // AB Stock Engine Power
                "<td> ([0-9.,]*)\n<\\/td>\n"+ // AB Upgraded Engine Power
                "<td> ([0-9._,]*)\n<\\/td>\n"+ // AB Stock Power-to-weight ratio
                "<td> ([0-9.,]*)\n<\\/td>"+   // AB Upgraded Power-to-weight ratio
                "<\\/tr>((.)*?)<\\/th>\n"+
                "<td> ([0-9.,]*)\n<\\/td>\n"+ // RB+SB forward
                "<td> ([0-9.,]*)\n<\\/td>\n"+ // RB+SB reverse
                "<td> ([0-9._,]*)\n<\\/td>\n"+ // RB+SB Stock Engine Power
                "<td> ([0-9.,]*)\n<\\/td>\n"+ // RB+SB Upgraded Engine Power
                "<td> ([0-9._,]*)\n<\\/td>\n"+ // RB+SB Stock Power-to-weight ratio
                "<td> ([0-9.,]*)\n<\\/td>"+   // RB+SB Upgraded Power-to-weight ratio
            "<\\/tr>((.)*?)<\\/table>",
            RegexOptions.Compiled | RegexOptions.Singleline
        );

        public static Regex mobilityPtrn2 = new Regex(
            """<div class="specs_char">\n<div class="specs_char_block">\n<div class="specs_char_line head"><span class="name">Speed</span><span class="value">forward / back</span></div>\n<div class="specs_char_line indent"><span class="name">AB</span><span class="value">([0-9]*) / ([0-9]*) km/h</span></div>\n<div class="specs_char_line indent"><span class="name">RB and SB</span><span class="value">([0-9]*) / ([0-9]*) km/h</span></div>\n</div>\n<div class="specs_char_block">\n<div class="specs_char_line head"><span class="name">Number of gears</span><span class="value">([0-9]*) forward</span></div>\n<div class="specs_char_line indent"><span class="name"></span><span class="value">([0-9]*) back</span></div>\n</div>\n<div class="specs_char_block">\n<div class="specs_char_line head"><span class="name">Weight</span><span class="value">([0-9.]*) t</span></div>\n</div>\n<div class="specs_char_block">\n<div class="specs_char_line head"><span class="name">Engine power</span><span class="value"></span></div>\n<div class="specs_char_line indent"><span class="name">AB</span><span class="value">([0-9]* ?[0-9]*) hp</span></div>\n<div class="specs_char_line indent"><span class="name">RB and SB</span><span class="value">([0-9]* ?[0-9]*) hp</span></div>\n</div>\n<div class="specs_char_block">\n<div class="specs_char_line head"><span class="name">Power-to-weight ratio</span><span class="value"></span></div>\n<div class="specs_char_line indent"><span class="name">AB</span><span class="value">([0-9.]*) hp/t</span></div>\n<div class="specs_char_line indent"><span class="name">RB and SB</span><span class="value">([0-9.]*) hp/t</span></div>\n</div>\n</div>\n</div>""",
            RegexOptions.Compiled);
        public static Regex repairPtrn = new Regex(
            "<div class=\"specs_char_line indent\"><span class=\"name\">(A|R|S)B<\\/span><span class=\"value\">([\\d\\s]*)(?:→([\\d\\s]+) )?<a href=\"\\/Silver_Lions\" title=\"Silver Lions\">((.)*?)<\\/span><\\/div>",
            RegexOptions.Compiled | RegexOptions.Singleline
        );
        public static Regex featuresPtrn = new Regex(
            "<div class=\"feature (.*?)\">.*\n",
            RegexOptions.Compiled
        );
        public static Regex mainArmamentPtrn = new Regex(
            """<h3><span class="mw-headline" id="Main_armament">Main armament<\/span><\/h3>(?:\n.*)?\n<div class="specs_info weapon">\n<div class="specs_name_weapon">([0-9]*? x)?(?:&#32;)?(?:<a href="\/)?(.*?)(?:" class="mw-redirect)?(?:" title=".*?">.*?<\/a>)?<\/div>""",
            RegexOptions.Compiled
        );
        public static Regex diameterPtrn = new Regex(
            "([0-9.]*) ?mm",
            RegexOptions.Compiled
        );
        public static Func<Match, string> diameterSub = (Match m) => m.Groups[1].Value + "mm";
        public static Regex multipleGunsPtrn = new Regex(
            "^([0-9]*) x "
        );
        public static Func<Match, string> multiGunSub = (Match m) => m.Groups[1].Value + "x ";
        public static Regex SLModifierPtrn = new Regex(
            """<div class="specs_char_line indent"><span class="name"></span><span class="value">(<img alt="Talisman\.png" src="/images/thumb/9/99/Talisman\.png/18px-Talisman\.png" width="18" height="18" class="ttx-img-talisman" srcset="/images/9/99/Talisman\.png 1\.5x" data-file-width="24" data-file-height="24" /> 2 ×&#160;)?([0-9]*?) / ([0-9]*?) / ([0-9]*?)&#160;% <a href="/Silver_Lions" title="Silver Lions"><img alt="Sl icon\.png" src="/images/thumb/0/0f/Sl_icon\.png/19px-Sl_icon\.png" width="19" height="20" srcset="/images/thumb/0/0f/Sl_icon\.png/28px-Sl_icon\.png 1\.5x, /images/0/0f/Sl_icon\.png 2x" data-file-width="32" data-file-height="34" /></a></span></div>""",
            RegexOptions.Compiled
        );
        public static Regex RPModifierPtrn = new Regex(
            """<div class="specs_char_line indent"><span class="name"></span><span class="value">(<img alt="Talisman\.png" src="/images/thumb/9/99/Talisman\.png/18px-Talisman\.png" width="18" height="18" class="ttx-img-talisman" srcset="/images/9/99/Talisman\.png 1\.5x" data-file-width="24" data-file-height="24" /> 2 ×&#160;)?([0-9]*?) / ([0-9]*?) / ([0-9]*?)&#160;% <a href="/Research_Points" title="Research Points"><img alt="Rp icon\.png" src="/images/thumb/2/21/Rp_icon\.png/13px-Rp_icon\.png" width="13" height="20" srcset="/images/thumb/2/21/Rp_icon\.png/20px-Rp_icon\.png 1\.5x, /images/2/21/Rp_icon\.png 2x" data-file-width="23" data-file-height="34" /></a></span></div>""",
            RegexOptions.Compiled
        );
        public static Regex reloadPtrn = new Regex(
            """<div class="specs_char_line indent"><span class="name"></span><span class="value">([0-9.]*?) → ([0-9.]*?) s</span></div>|<div class="specs_char_line head"><span class="name">Reload</span><span class="value">([0-9.]*?) s</span></div>""",
            RegexOptions.Compiled
        );

        public static Regex reservePtrn = new Regex(
            """<div class="general_info_price_research"><span class="desc">Research:</span><span class="value">Free</span></div><div class="general_info_price_buy"><span class="desc">Purchase:</span><span class="value">Free</span></div>""",
            RegexOptions.Compiled
        );
    }
    class RegFunc {
        public static string Replace(string input, Regex reg, Func<Match, string> replace) {
            return reg.Replace(input, delegate(Match m) {return replace(m);});
        }
    }
}