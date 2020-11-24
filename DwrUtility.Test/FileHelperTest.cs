using DwrUtility.Test.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DwrUtility.Test
{
    [TestClass]
    public class FileHelperTest
    {
        public FileHelperTest()
        {
#if NETSTANDARD
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        }

        private static bool TestEncoding(string txt)
        {
            var path = $"{TestConfig.TestDir}Logs/{Guid.NewGuid()}.txt";
            path.CreateDirByFilePath();

            //utf-8bom
            File.WriteAllText(path, txt, Encoding.UTF8);
            var s1 = FileHelper.ReadText(path);
            var b1 = s1 == txt;

            //utf-8
            File.WriteAllText(path, txt, new UTF8Encoding());
            var s2 = FileHelper.ReadText(path);
            var b2 = s2 == txt;

            //gb2312  ANSI
            File.WriteAllText(path, txt, Encoding.GetEncoding("gb2312"));
            var s3 = FileHelper.ReadText(path);
            var b3 = s3 == txt;

            //gbk  ANSI
            File.WriteAllText(path, txt, Encoding.GetEncoding("gbk"));
            var s4 = FileHelper.ReadText(path);
            var b4 = s4 == txt;

            //ANSI
            File.WriteAllText(path, txt, Encoding.Default);
            var s5 = FileHelper.ReadText(path);
            var b5 = s5 == txt;

            //Unicode   UCS-2 LE BOM
            File.WriteAllText(path, txt, Encoding.Unicode);
            var s6 = FileHelper.ReadText(path);
            var b6 = s6 == txt;

            //BigEndianUnicode  UCS-2 BE BOM
            File.WriteAllText(path, txt, Encoding.BigEndianUnicode);
            var b7 = FileHelper.ReadText(path) == txt;

            FileHelper.DeleteFile(path, false);
            var b =
                    b1 &&
                    b2 &&
                    b3 &&
                    b4 &&
                    b5 &&
                    b7 &&
                    b6;
            return b;
        }

        [TestMethod]
        public void TestMethod1()
        {
            var b1 = TestEncoding("我是中国人");
            Assert.IsTrue(b1);

            var b2 = TestEncoding("abcABC");
            Assert.IsTrue(b2);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var list = new List<string>()
            {
                "一丌丐专丕业丝丞丧丨丫丶为丿乇么之乍乒乜也习乾亍亘些亟亩亭什仟伞伟伪伲伽位住佗佟佣佴佶佻佼佾使侃侄侉侏侑侔侪侬俅俑俚俜俟俣俦俨俪俳俸倏倩倬债值偌偏停偶偷傻僧僻兀元全冉冒冖写农冢冤冥冶准凇凭凸刍刖删刷券刹",
                "前剩劝劢努劬劭勋募匕匣匹医十千卅协卓卟占卢卤卮卯印危却卸压原去双取叨叩只台叱史叶司叹叻叽吆同吒吖吮吱呀呋呒呓呔呕呖员呛味咋咏咬品哟哦唯唰唳唷啖啜啤啥啪啵啶啷啸啻啾喁喂喃喈喋喑喔喙喜喟喱喹喽喾嗉嗑嗒嗓嗖嗟",
                "嗪嗫嗷嘟嘶噎噩囟园围图圆圈圣址坍坛坠坦坪垄垣埭埽堀堋堍堑堙堞堠塄塥塬墀墁墉墓墙墚墨士壮壹夂夕夜夤夥太夭失头奂女奴妆妗妞妤妫妯妲妾姆姊始姒委姗姘姚姝姣姹娄娅娆娈娉娌娑娓娣娲娴娶娼婀婊婕婢婧婴婵媒媳嫂孝学孬",
                "宀宄宅宓宕实宥宸寓寞寤寨寮寰寻尉小尧尸尾屁屎屑展山屺岈岌岍岐岑岖岘岙岚岜岢岣岫岬岱岵岷岽峁峄峋峒峡峤峥崭嵌巍巡帅师希帧席帷帽幄幔幕幛幞幡平庄庐应庭庸廷廿弄弋式弑强彀录彤影徒循微忒志忙态怒思怨恕恪息恰恽悃",
                "悉悌悒悖悚悛悝悭悱悴悻惆惘惚惜惝惟惬惴愀愎愕愠愣愦愫愿慊慕慰慵憔憧憬懈懦懿戋戏戗战戛戟戡戢戤戥戬戮扦执扫扭抓投抬抹押拇拙拢拥拧拳拴拼拽拾指挚挞挟挪挺捅捉掖探掳揖援搂搔搴携搿摇摊摔摘摩摹摺撇撕撙撞撰撷撸撺",
                "擀擐擗擘擢擤攉攥攮支攴攵效敕散敫斋斐斓斜斩斯於施旃旄旆旌旎旒旖旨旬旮旯旰时昃昊昕昙映晒暇暖暮曰曳朔木未末杀权杈杉杌杓杞杩杲枚枝枪某染栅栈栓校桅桐桑桩桶梅械棣森棰椁椋植椎椐椤椰椴椹楂楔楗楝楠楣楦楫楱楸楹楼",
                "榀榄榇榈榉榘榛榧榨榫榭榷榻槌槎模樱檀檄欠欤欧欷欹歃歆歇歉歙止殉殖殳殴毂母每毓毛毡毪毯毳毵毹毽氅氆氇氍氐氓氕氘氙氚氡氤氩氪氲水汀汁汐汛汰沙没沤沫沾泄泉泞注泰泳泻洄洇洌洎洗洙洚洧洫洮洳洵洹洽浃浅浈浍浏浒浓浔",
                "浜浞浠浣浯浴浼涂涌涑涓涔涞涠涩液淇渊渚渭湛湿源溪滩漂漏漠潍潜潞潭澧澶澹濂濉濞濠濡濮濯瀚瀛瀣瀵瀹灏灞炀炉炔炜炭炸烁烯然煞煤煽熄熏燃爪爰爷爻爽片牍牒牖牛牟牡牵犀犬状狮狻猊猓猕猗猝猞猡猢猥猫猬猱猸猹猿獍獐獗獠",
                "獬獭獯獾玫珊琚琛瑁瑕瑗瑙瑜瑟瑭瑷瑾璀璁璇璋璎璐璜璞璧璨璩璺瓒瓞瓠瓢瓤瓯瓴瓶瓿甏甑甓甙甩甬甯町畀畈畋畎畏畛畲畹疃疟疲症痈痊痛痞痰瘛瘠瘢瘫瘭瘰瘳瘵瘸瘼瘾瘿癀癃癍癔癖癜癞癣癫癯皈皎皓皙皤皮盈盏盛目盲直省眉眨眩",
                "睡睢睥睽睾睿瞀瞌瞍瞎瞑瞟瞠瞢瞥瞬瞰瞳瞵瞻瞽矛矢知石砂砖硕硬确碌碳磨示祝票禄禺私秃稀税稷稹穑穰穴穸穹窀突窄窆窈窑窕窠窦窨窬站童竦笑筒筛签篆篇篓粘糯系约纬纭纰纱纸纽纾绀绁绂细织绉绋绌绐绔绗绚绛统绠绡绨绫绮绯",
                "绱绲维绶绺绻绾缀缁缂缃缇缈缍缘缨缶缺罂罘罟罡罨罱罴罹羁羌群翊耀耄耍耶职聘聙聛聜聝聞聟聠聡聢聣聤聥聦聧聨聫聬聭聮聯聰聲聳聴聵聶職聸聹聺聻聼聽肖肟肢胁胃胎胜胤脂脌脓脕脗脙脛脜脝脟脠脡脢脣脤脥脦脧脨脩脪脫脭脮",
                "脰脳脴脵脷脹脺脻脼脽脾脿腋腔膜膝膦膧膩膫膬膭膮膯膰膱膲膴膵膶膷膸膹膼膽膾膿臁臃臄臅臇臈臉臋臍臎臏臐臑臒臓臧舀舛舜艀艁艂艃艅艆艇艈艊艌艍艎艐艑艒艓艔艕艖艗艙艛艜艝艞艠艡艢艣艤艥艦艧艩色艹艽艿芄芊芎芏芑芒芗",
                "芙芝芦芨芫芯芸芽苇苑苔苫英苹苺苼苽苾苿茀茂茅茊茋茍茐茒茓茖茘茙茛茝茞茟茠茡茢茣茤茥茦茩茪茫茮茰茲茷茻茽荧荨荩荪荬荭荮药荸荻荼荽莅莎莓莘莜莞莠莨莩莪莫莯莰莳莴莵莶莸莹莺莻莼莽莾莿菁菂菃菄菆菈菉菋菍菎菐菑菒",
                "菓菕菗菘菙菚菛菞菢菣菤菥菦菧菨菫菬菭萁萍萎萤营葊葋葌葍葎葏葐葒葓葔葕葖葘葝葞葟葠葢葤葥葦葧葨葪葮葯葰葲葴葷葹葻葼蓘蓙蓚蓛蓜蓞蓡蓢蓤蓧蓨蓩蓪蓫蓭蓮蓯蓱蓲蓳蓴蓵蓶蓷蓸蓹蓺蓻蓼蓽蓾蓿蔀蔁蔂蔚蔷蔻蕃蕈蕖蕗蕘蕙蕚",
                "蕛蕜蕝蕞蕟蕠蕡蕢蕣蕤蕥蕦蕧蕨蕩蕪蕫蕬蕭蕮蕯蕰蕱蕲蕳蕵蕶蕷蕸蕹蕺蕻蕼蕽蕿薀薁薅薇薏薛薜薤薨薪薮薰薷薹藁藓藔藕藖藗藘藙藚藛藜藝藞藟藠藡藢藣藥藦藧藨藪藫藬藭藮藯藰藱藲藳藴藵藶藷藸藿蘅蘑蘧蘸虁虂虃虄虅虆虇虈虉虊",
                "虋虌虍虏虐虒虓虔處虖虗虘虙虛虜虝號虠虡虢虣虤虥虦虧虨虩虪虬虮虱虺虻虼虾虿蚀蚋蚍蚓蚝蚣蚧蚨蚩蚪蚬蛝蛠蛡蛢蛣蛥蛦蛧蛨蛪蛫蛬蛯蛵蛶蛷蛹蛺蛻蛼蛽蛿蜁蜄蜅蜆蜋蜌蜎蜏蜐蜑蜔蜖蜘蝇蝎蝷蝸蝹蝺蝿螀螁螃螄螅螆螇螈螉螊螌螎",
                "螏螐螑螒螔螕螖螗螘螙螚螛螜螝螞螠螡螢螣螤螫螬螭螳螵螽蟀蟆蟊蟋蟑蟓蟛蟠蟥蟪蟮蟹蟺蟻蟼蟽蟾蟿蠀蠁蠂蠄蠅蠆蠇蠈蠉蠊蠋蠌蠍蠎蠏蠐蠑蠒蠓蠔蠖蠗蠘蠙蠚蠛蠜蠝蠞蠟蠠蠡蠣蠹蠼血衰衻衼袀袁袃袆袇袉袊袌袎袏袐袑袒袓袔袕袗袘",
                "袙袚袛袝袞袟袠袡袣袥袦袧袨袩袪袭装裕裙褉褋褌褍褎褏褑褔褕褖褗褘褜褝褞褟褠褢褣褤褦褧褨褩褬褭褮褯褰褱褲褳褵褷襽襾覀要覂覄覅覇覈覉覊見覌覍覎規覐覑覒覓覔覕視覗覘覙覚覛覜覝覞覟覠覡觳觻觼觽觾觿訁訂訃訄訅訆計訉",
                "訊訋訌訍討訏訐訑訒訓訔訕訖託記訙訚訛訜訝詈詟詠詡詢詣詤詥試詧詨詩詪詫詬詭詮詯詰話該詳詴詵詶詷詸詹詺詻詼詽詾詿誀諃諄諅諆談諈諉諊請諌諍諎諏諐諑諒諓諔諕論諗諘諙諚諛諜諝諞諟諠諡諢諣謤謥謧謨謩謪謫謬謭謮謯謰謱",
                "謲謳謴謵謶謷謸謹謺謻謼謽謾謿譀譁譂譃譄譅譬讇讈讉變讋讌讍讎讏讐讑讒讓讔讕讖讗讘讙讚讛讜讝讞讟讠讦讧讪讬训讯讱讴讵讷讻证诂诃识诇诈诋诎诏诐诒诓诔诖诗诘诙诜诟诠询诤诨诩诪诮诰诳说诺谁谆谈谉谋谐谓谞谢谣谦谩谭",
                "谴豫貈貋貌貍貎貏貐貑貒貓貕貖貗貙貚貛貜貝貞貟負財貢貣貤貥貦貧貨販貪貫責貭賭賮賯賰賱賲賳賴賵賶賷賸賹賺賻購賽賾賿贀贁贂贃贄贅贆贇贈贉贊贋贌贍贪贫贸赂赘赚赢越趢趣趤趥趦趧趨趩趪趫趬趭趮趯趰趲趴趶趷趹趻趽趾跀",
                "跁跂跃跅跇跈跉跊跍跐跒跓跔路踊踏踿蹃蹅蹆蹇蹋蹌蹍蹎蹏蹐蹓蹔蹕蹖蹗蹘蹚蹛蹜蹝蹞蹟蹠蹡蹢蹣蹤蹥蹧蹨蹪蹫蹮蹱軃軄軅軆軇軈軉車軋軌軍軎軏軐軑軒軓軔軕軖軗軘軙軚軛軜軝軞軟軠軡転軣軤輤輥輦輧輨輩輪輫輬輭輮輯輰輱輲輳",
                "輴輵輶輷輸輹輺輻輼輽輾輿轀轁轂轃轄转辍辎辏辕辖辗辘辚迁迅迉迊迋迌迍迎迏迒迖迗迚远违迠迡迣迧迬迯迱迲迴迵迶迺迻迼追迾迿逇逈选逊逌逎透逓途逕逘通遣遥還邅邆邇邉邊邌邍邎邏邐邒邔邖邘邚邜邞邟邠邤邥邧邨邩邪邫邭邰",
                "邲邷邸邼邽邾邿郀郄郅郇郏郐郑郓郗郛郜郢郦郫郯郾鄄鄞鄢鄣鄯鄱鄹酃酄酆酞酮酶醛醼醽醾醿釀釁釂釃釄釅釆釈釋野釐釒釓釔釕釖釗釘釙釚釛針釞釟釠釡釢釣釤釥鈥鈦鈧鈨鈩鈪鈫鈬鈭鈮鈯鈰鈱鈲鈳鈴鈵鈶鈷鈸鈹鈺鈻鈼鈽鈾鈿鉀鉁鉂",
                "鉃鉄鉅銆銇銈銉銊銋銌銍銏銐銑銒銓銔銕銖銗銘銙銚銛銜銝銞銟銠銡銢銣銤銥銦銧鋩鋪鋫鋬鋭鋮鋯鋰鋱鋲鋳鋴鋵鋶鋷鋸鋹鋺鋻鋼鋽鋾鋿錀錁錂錃錄錅錆錇錈錉鍊鍋鍌鍍鍎鍏鍐鍑鍒鍓鍔鍕鍖鍗鍘鍙鍚鍛鍜鍝鍞鍟鍠鍡鍢鍣鍤鍥鍦鍧鍨鍩",
                "鍫鎬鎭鎮鎯鎰鎱鎲鎳鎴鎵鎶鎷鎸鎹鎺鎻鎼鎽鎾鎿鏀鏁鏂鏃鏄鏅鏆鏇鏈鏉鏋鏌鏍鐎鐏鐐鐑鐒鐓鐔鐕鐖鐗鐘鐙鐚鐛鐜鐝鐞鐟鐠鐡鐢鐣鐤鐥鐦鐧鐨鐩鐪鐫鐬鐭鐮鑰鑱鑲鑳鑴鑵鑶鑷鑸鑹鑺鑻鑼鑽鑾鑿钀钁钂钃钄钎钑钖钘钥钮钱钳铅铆铇铏铓",
            };

            var failed = new List<string>();
            foreach (var item in list)
            {
                var flag = TestEncoding(item);
                if (!flag)
                {
                    failed.Add(item);
                }
            }

            if (failed.Count > 0)
            {
                Assert.Inconclusive("gb2312/gbk/ANSI写入字符，读取后为乱码");
            }
            //Assert.IsTrue(failed.Count == 0);

            var str = string.Join("\r\n", failed);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var str = "芙芝芦芨芫芯芸芽苇苑苔苫英苹苺苼苽苾苿茀茂茅茊茋茍茐茒茓茖茘茙茛茝茞茟茠茡茢茣茤茥茦茩茪茫茮茰茲茷茻茽荧荨荩荪荬荭荮药荸荻荼荽莅莎莓莘莜莞莠莨莩莪莫莯莰莳莴莵莶莸莹莺莻莼莽莾莿菁菂菃菄菆菈菉菋菍菎菐菑菒";
            var flag = TestEncoding(str);
            //utf-8
            //֥ܽ«ܸܾоܿѿέԷ̦ɻӢƻƀƁƂƃƄƅïéƆƇƈƉƊƋƌƍƎݢƏƐƑƒƓƔƕƖƗƘƙƚãƛƜƝƞƟƠӫݡݣݥݤݦݧҩݩݶݱݴݰɯݮݷݯݸݬݹݳݭĪǀݨݪݫǁݲݵӨݺǂݻçǃǄݼǅǆǇǈǉǊǋǌǍǎǏǐ

            if (!flag)
            {
                Assert.Inconclusive("gb2312/gbk/ANSI写入字符，读取后为乱码");
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            var str = "菓菕菗菘菙菚菛菞菢菣菤菥菦菧菨菫菬菭萁萍萎萤营葊葋葌葍葎葏葐葒葓葔葕葖葘葝葞葟葠葢葤葥葦葧葨葪葮葯葰葲葴葷葹葻葼蓘蓙蓚蓛蓜蓞蓡蓢蓤蓧蓨蓩蓪蓫蓭蓮蓯蓱蓲蓳蓴蓵蓶蓷蓸蓹蓺蓻蓼蓽蓾蓿蔀蔁蔂蔚蔷蔻蕃蕈蕖蕗蕘蕙蕚";
            var flag = TestEncoding(str);
            //utf-8
            //ǑǒǓݿǔǕǖǗǘǙǚݾǛǜǝǞǟǠݽƼήөӪȀȁȂȃȄȅȆȇȈȉȊȋȌȍȎȏȐȑȒȓȔȕȖȗȘșȚțȜȝȞȟȠɀɁɂɃɄɅɆɇɈɉɊɋɌɍɎɏɐɑɒɓɔɕɖɗɘəɚɛޤɜɝޣɞɟɠεǾޢެަޡʀʁޥʂ

            if (!flag)
            {
                Assert.Inconclusive("gb2312/gbk/ANSI写入字符，读取后为乱码");
            }
        }

        [TestMethod]
        public void TestMethod5()
        {
            var str = "蕛蕜蕝蕞蕟蕠蕡蕢蕣蕤蕥蕦蕧蕨蕩蕪蕫蕬蕭蕮蕯蕰蕱蕲蕳蕵蕶蕷蕸蕹蕺蕻蕼蕽蕿薀薁薅薇薏薛薜薤薨薪薮薰薷薹藁藓藔藕藖藗藘藙藚藛藜藝藞藟藠藡藢藣藥藦藧藨藪藫藬藭藮藯藰藱藲藳藴藵藶藷藸藿蘅蘑蘧蘸虁虂虃虄虅虆虇虈虉虊";
            var flag = TestEncoding(str);

            if (!flag)
            {
                Assert.Inconclusive("gb2312/gbk/ANSI写入字符，读取后为乱码");
            }
        }

        [TestMethod]
        public void TestMethod6()
        {
            var str = "謲謳謴謵謶謷謸謹謺謻謼謽謾謿譀譁譂譃譄譅譬讇讈讉變讋讌讍讎讏讐讑讒讓讔讕讖讗讘讙讚讛讜讝讞讟讠讦讧讪讬训讯讱讴讵讷讻证诂诃识诇诈诋诎诏诐诒诓诔诖诗诘诙诜诟诠询诤诨诩诪诮诰诳说诺谁谆谈谉谋谐谓谞谢谣谦谩谭";
            var flag = TestEncoding(str);

            if (!flag)
            {
                Assert.Inconclusive("gb2312/gbk/ANSI写入字符，读取后为乱码");
            }
        }

        [TestMethod]
        public void TestMethod7()
        {
            var str = "谴豫貈貋貌貍貎貏貐貑貒貓貕貖貗貙貚貛貜貝貞貟負財貢貣貤貥貦貧貨販貪貫責貭賭賮賯賰賱賲賳賴賵賶賷賸賹賺賻購賽賾賿贀贁贂贃贄贅贆贇贈贉贊贋贌贍贪贫贸赂赘赚赢越趢趣趤趥趦趧趨趩趪趫趬趭趮趯趰趲趴趶趷趹趻趽趾跀";
            var flag = TestEncoding(str);

            if (!flag)
            {
                Assert.Inconclusive("gb2312/gbk/ANSI写入字符，读取后为乱码");
            }
        }

        /// <summary>
        /// gb2312
        /// </summary>
        [TestMethod]
        public void TestMethod8()
        {
            var list = new Dictionary<string, Encoding>()
            {
                { "GB2312", Encoding.GetEncoding("gb2312") },
                { "GBK", Encoding.GetEncoding("gbk")},
                { "ANSI", Encoding.Default},

                { "BigEndianUnicode",   Encoding.BigEndianUnicode},
                { "Unicode",  Encoding.Unicode},
                { "UTF-8BOM", Encoding.UTF8},

                { "UTF8", new UTF8Encoding()},
            };

            var txt = "是";

            var results = new List<string>();
            foreach (var item in list)
            {
                results.Add(GetResult(txt, item.Value, item.Key));
            }
            var b = results.Distinct().Count() == list.Count;
            if (b)
            {
                var res = txt;
            }
        }

        private static string GetResult(string str, Encoding encoding, string encodingName)
        {
            var path = $"{TestConfig.TestDir}Logs/{Guid.NewGuid()}.txt";
            File.WriteAllText(path, str, encoding);

            var s1 = File.ReadAllText(path, Encoding.GetEncoding("gb2312"));
            var s2 = File.ReadAllText(path, Encoding.GetEncoding("gbk"));
            var s3 = File.ReadAllText(path, Encoding.Default);
            var s4 = File.ReadAllText(path, Encoding.BigEndianUnicode);
            var s5 = File.ReadAllText(path, Encoding.Unicode);
            var s6 = File.ReadAllText(path, Encoding.UTF8);
            var s7 = File.ReadAllText(path, new UTF8Encoding());
            var result = $"{s1}{s2}{s3}{s4}{s5}{s6}{s7}";

            FileHelper.DeleteFile(path, false);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestMethod9()
        {
            var sb = new StringBuilder();
            sb.AppendLine("测试|&#39321;&#28207;&#35686;&#26041;");
            sb.AppendLine("超链接|ChaoLianJie");
            sb.AppendLine("网址|WebSite");

            var b2 = TestEncoding(sb.ToString());
            Assert.IsTrue(b2);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestMethod10()
        {
            var path1 = $"{TestConfig.TestDir}DataFiles/images/error.jpg";
            var b1 = FileHelper.GetFileExtension(path1) == ".jpg";

            var path2 = "DataFiles/error.vue";
            var b2 = FileHelper.GetFileExtension(path2) == ".vue";

            Assert.IsTrue(b1 && b2);
        }
    }
}
