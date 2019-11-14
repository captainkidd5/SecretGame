<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.0" name="MasterSpriteSheet" tilewidth="16" tileheight="16" tilecount="10000" columns="100">
 <image source="../../../Map/MasterSpriteSheet.png" width="1600" height="1600"/>
 <terraintypes>
  <terrain name="dirt" tile="904"/>
 </terraintypes>
 <tile id="85">
  <properties>
   <property name="action" value="plantable"/>
  </properties>
 </tile>
 <tile id="226">
  <properties>
   <property name="generate" value="water"/>
  </properties>
 </tile>
 <tile id="227">
  <properties>
   <property name="generate" value="water"/>
  </properties>
 </tile>
 <tile id="228">
  <properties>
   <property name="generate" value="water"/>
  </properties>
 </tile>
 <tile id="249">
  <objectgroup draworder="index">
   <object id="1" x="0.136365" y="0.0909" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="250">
  <properties>
   <property name="newSource" value="0,-16,0,16"/>
  </properties>
 </tile>
 <tile id="251">
  <properties>
   <property name="newSource" value="0,-16,0,16"/>
  </properties>
 </tile>
 <tile id="252">
  <properties>
   <property name="newSource" value="0,-16,0,16"/>
  </properties>
 </tile>
 <tile id="253">
  <properties>
   <property name="newSource" value="0,-16,0,16"/>
  </properties>
 </tile>
 <tile id="254">
  <properties>
   <property name="newSource" value="0,-16,0,16"/>
  </properties>
 </tile>
 <tile id="255">
  <objectgroup draworder="index">
   <object id="1" x="0.136365" y="0.181809" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="260">
  <properties>
   <property name="action" value="smelt"/>
  </properties>
 </tile>
 <tile id="288">
  <properties>
   <property name="AssociatedTiles" value="189"/>
  </properties>
 </tile>
 <tile id="289">
  <properties>
   <property name="AssociatedTiles" value="190"/>
  </properties>
 </tile>
 <tile id="290">
  <properties>
   <property name="AssociatedTiles" value="191"/>
   <property name="destructable" value="-50,1,0,16"/>
   <property name="loot" value="691:100:1,690:20:3"/>
  </properties>
 </tile>
 <tile id="295">
  <properties>
   <property name="AssociatedTiles" value="196"/>
  </properties>
 </tile>
 <tile id="296">
  <properties>
   <property name="AssociatedTiles" value="197"/>
   <property name="destructable" value="-50,1,0,16"/>
   <property name="loot" value="695:100:1,694:20:3"/>
  </properties>
 </tile>
 <tile id="325">
  <properties>
   <property name="generate" value="water"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.272727" y="2" width="14.9091" height="2.54545"/>
  </objectgroup>
 </tile>
 <tile id="326">
  <properties>
   <property name="generate" value="water"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.272727" y="0.0909091" width="15.3636" height="2.36364"/>
  </objectgroup>
 </tile>
 <tile id="327">
  <properties>
   <property name="generate" value="water"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="7.36364" y="0" width="7.27273" height="15.9091"/>
  </objectgroup>
 </tile>
 <tile id="328">
  <properties>
   <property name="generate" value="water"/>
  </properties>
 </tile>
 <tile id="329">
  <properties>
   <property name="generate" value="water"/>
  </properties>
 </tile>
 <tile id="349">
  <objectgroup draworder="index">
   <object id="1" x="0.136365" y="0.363627" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="355">
  <objectgroup draworder="index">
   <object id="1" x="0.136365" y="0.363627" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="360">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="16" height="1"/>
  </objectgroup>
 </tile>
 <tile id="361">
  <properties>
   <property name="action" value="1"/>
  </properties>
 </tile>
 <tile id="362">
  <properties>
   <property name="action" value="1"/>
  </properties>
 </tile>
 <tile id="425">
  <properties>
   <property name="generate" value="water"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.3125" y="0.0625" width="3.25" height="15.75"/>
  </objectgroup>
 </tile>
 <tile id="426">
  <properties>
   <property name="generate" value="water"/>
  </properties>
 </tile>
 <tile id="427">
  <properties>
   <property name="generate" value="water"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="11.2727" y="0.272727" width="4.09091" height="15.3636"/>
  </objectgroup>
 </tile>
 <tile id="428">
  <properties>
   <property name="generate" value="water"/>
  </properties>
 </tile>
 <tile id="429">
  <properties>
   <property name="generate" value="water"/>
  </properties>
 </tile>
 <tile id="450">
  <properties>
   <property name="newSource" value="0,-16,0,16"/>
  </properties>
 </tile>
 <tile id="455">
  <objectgroup draworder="index">
   <object id="1" x="0.0454559" y="0.272718" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="478">
  <properties>
   <property name="idleStart" value=""/>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
  <animation>
   <frame tileid="479" duration="100"/>
   <frame tileid="480" duration="100"/>
   <frame tileid="481" duration="100"/>
   <frame tileid="482" duration="100"/>
   <frame tileid="483" duration="100"/>
   <frame tileid="484" duration="100"/>
  </animation>
 </tile>
 <tile id="479">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="480">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="481">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="482">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="483">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="484">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="488">
  <properties>
   <property name="AssociatedTiles" value="389"/>
  </properties>
 </tile>
 <tile id="489">
  <properties>
   <property name="AssociatedTiles" value="390"/>
  </properties>
 </tile>
 <tile id="490">
  <properties>
   <property name="AssociatedTiles" value="391"/>
   <property name="destructable" value="-50,1,0,16"/>
   <property name="loot" value="689:100:1,688:20:2"/>
  </properties>
 </tile>
 <tile id="495">
  <properties>
   <property name="AssociatedTiles" value="396"/>
  </properties>
 </tile>
 <tile id="496">
  <properties>
   <property name="AssociatedTiles" value="397"/>
   <property name="destructable" value="-50,1,0,16"/>
   <property name="loot" value="697:100:1,696:20:3"/>
  </properties>
 </tile>
 <tile id="525">
  <properties>
   <property name="generate" value="water"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.272727" y="8.54545" width="14.9091" height="3.45455"/>
  </objectgroup>
 </tile>
 <tile id="526">
  <properties>
   <property name="generate" value="water"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.181818" y="13.7273" width="15.2727" height="1.81818"/>
  </objectgroup>
 </tile>
 <tile id="527">
  <properties>
   <property name="generate" value="water"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.272727" y="6" width="15.4545" height="5.27273"/>
  </objectgroup>
 </tile>
 <tile id="563">
  <properties>
   <property name="layer" value="3"/>
   <property name="newSource" value="-16,-64,48,80"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="578">
  <properties>
   <property name="AssociatedTiles" value="478"/>
   <property name="Speed" value=".15"/>
   <property name="destructable" value="1,0,0,2"/>
   <property name="idleStart" value=""/>
   <property name="loot" value="563:100:1"/>
   <property name="spawnWith" value="478"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="1.63636" y="0.727273" width="12.9091" height="11.6364"/>
  </objectgroup>
  <animation>
   <frame tileid="579" duration="100"/>
   <frame tileid="580" duration="100"/>
   <frame tileid="581" duration="100"/>
   <frame tileid="582" duration="100"/>
   <frame tileid="583" duration="100"/>
   <frame tileid="584" duration="100"/>
  </animation>
 </tile>
 <tile id="649">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="650">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="651">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.181818" y="7.90909" width="16" height="6.36364"/>
  </objectgroup>
 </tile>
 <tile id="652">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="653">
  <properties>
   <property name="newSource" value="0,-16,0,16"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.272727" y="8.63636" width="16" height="6.36364"/>
  </objectgroup>
 </tile>
 <tile id="654">
  <properties>
   <property name="newSource" value="0,-16,0,16"/>
  </properties>
 </tile>
 <tile id="655">
  <properties>
   <property name="newSource" value="0,-16,0,16"/>
  </properties>
 </tile>
 <tile id="656">
  <properties>
   <property name="newSource" value="0,-16,0,16"/>
  </properties>
 </tile>
 <tile id="663">
  <properties>
   <property name="destructable" value="21,4,1,3"/>
   <property name="loot" value="520:100:5,1003:10:2"/>
   <property name="spawnWith" value="563"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="3.125" y="0.25" width="8.25" height="12.625"/>
  </objectgroup>
 </tile>
 <tile id="678">
  <properties>
   <property name="idleStart" value=""/>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
  <animation>
   <frame tileid="679" duration="100"/>
   <frame tileid="680" duration="100"/>
   <frame tileid="681" duration="100"/>
   <frame tileid="682" duration="100"/>
   <frame tileid="683" duration="100"/>
   <frame tileid="684" duration="100"/>
  </animation>
 </tile>
 <tile id="679">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="680">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="681">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="682">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="683">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="684">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="690">
  <properties>
   <property name="destructable" value="-50,1,0,16"/>
   <property name="loot" value="693:100:2,691:20:2"/>
  </properties>
 </tile>
 <tile id="696">
  <properties>
   <property name="destructable" value="-50,1,0,16"/>
   <property name="loot" value="699:100:2,698:20:2"/>
  </properties>
 </tile>
 <tile id="705">
  <properties>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="706">
  <properties>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="707">
  <properties>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="709">
  <properties>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="710">
  <properties>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="711">
  <properties>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="712">
  <properties>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="778">
  <properties>
   <property name="AssociatedTiles" value="678"/>
   <property name="Speed" value=".15"/>
   <property name="destructable" value="1,0,0,8"/>
   <property name="idleStart" value=""/>
   <property name="loot" value="562:100:1"/>
   <property name="spawnWith" value="678"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.545455" y="0" width="14.3636" height="10.9091"/>
  </objectgroup>
  <animation>
   <frame tileid="779" duration="100"/>
   <frame tileid="780" duration="100"/>
   <frame tileid="781" duration="100"/>
   <frame tileid="782" duration="100"/>
   <frame tileid="783" duration="100"/>
   <frame tileid="784" duration="100"/>
  </animation>
 </tile>
 <tile id="828">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="829">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="830">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="831">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="832">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="833">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="854">
  <objectgroup draworder="index">
   <object id="1" x="0.500001" y="0.0909" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="896">
  <properties>
   <property name="destructable" value="-50,1,0,16"/>
   <property name="loot" value="729:100:2,728:20:2"/>
  </properties>
 </tile>
 <tile id="904">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="905">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="906">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="907">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="908">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="909">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="910">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="911">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="912">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="913">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="914">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="915">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="916">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="917">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="918">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="919">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="928">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="929">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="930">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="931">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="932">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="949">
  <objectgroup draworder="index">
   <object id="1" x="0.136365" y="0.272718" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="950">
  <objectgroup draworder="index">
   <object id="1" x="0.227274" y="0.0909" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="952">
  <objectgroup draworder="index">
   <object id="1" x="7.95455" y="0.0909" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="954">
  <objectgroup draworder="index">
   <object id="1" x="0.227274" y="0.181809" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="955">
  <objectgroup draworder="index">
   <object id="1" x="8.13637" y="0.181809" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="978">
  <properties>
   <property name="destructable" value="22,0,0,8"/>
   <property name="idleStart" value=""/>
   <property name="loot" value="1001:100:1"/>
   <property name="tileSelectorAllowed" value="2"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="1" y="1" width="13" height="13"/>
  </objectgroup>
  <animation>
   <frame tileid="979" duration="100"/>
   <frame tileid="980" duration="100"/>
   <frame tileid="981" duration="100"/>
   <frame tileid="982" duration="100"/>
   <frame tileid="983" duration="100"/>
  </animation>
 </tile>
 <tile id="1001">
  <properties>
   <property name="destructable" value="1,0,0,8"/>
   <property name="loot" value="221:100:1,220:15:1"/>
   <property name="spawnWith" value="1002"/>
   <property name="tileSelectorAllowed" value="2"/>
  </properties>
 </tile>
 <tile id="1002">
  <properties>
   <property name="layer" value="1"/>
   <property name="relationX" value="1"/>
   <property name="relationY" value="0"/>
  </properties>
 </tile>
 <tile id="1004">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1005">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1006">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1007">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1008">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1009">
  <properties>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1010">
  <properties>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1011">
  <properties>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1012">
  <properties>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1013">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1014">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1015">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1016">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1017">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1018">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1019">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1024">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1025">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1026">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1028">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="1029">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="1030">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="1031">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="1032">
  <properties>
   <property name="generate" value="stone"/>
  </properties>
 </tile>
 <tile id="1049">
  <objectgroup draworder="index">
   <object id="1" x="0.0454559" y="0.181809" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="1051">
  <objectgroup draworder="index">
   <object id="1" x="3.625" y="0.4375" width="8.75" height="15.375"/>
  </objectgroup>
 </tile>
 <tile id="1054">
  <objectgroup draworder="index">
   <object id="1" x="0.136365" y="0.272718" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="1055">
  <objectgroup draworder="index">
   <object id="1" x="8.04546" y="0.454536" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="1063">
  <properties>
   <property name="layer" value="3"/>
   <property name="newSource" value="-32,-48,80,64"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="1078">
  <properties>
   <property name="destructable" value="2,0,3,6"/>
   <property name="idleStart" value=""/>
   <property name="loot" value="1002:100:1"/>
   <property name="tileSelectorAllowed" value="3"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="2" y="2" width="12" height="11"/>
  </objectgroup>
  <animation>
   <frame tileid="1079" duration="100"/>
   <frame tileid="1080" duration="100"/>
   <frame tileid="1081" duration="100"/>
   <frame tileid="1082" duration="100"/>
   <frame tileid="1083" duration="100"/>
  </animation>
 </tile>
 <tile id="1104">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1105">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1106">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1107">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1108">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1109">
  <properties>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1110">
  <properties>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1111">
  <properties>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1112">
  <properties>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1113">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1114">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1115">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1116">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1117">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1118">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1119">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1120">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1121">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1122">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1123">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1124">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1125">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1149">
  <objectgroup draworder="index">
   <object id="1" x="4.22727" y="-0.0341" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="1150">
  <objectgroup draworder="index">
   <object id="1" x="3.5" y="0.3125" width="8.75" height="15.375"/>
  </objectgroup>
 </tile>
 <tile id="1151">
  <objectgroup draworder="index">
   <object id="1" x="3.75" y="0.375" width="8.75" height="15.375"/>
  </objectgroup>
 </tile>
 <tile id="1152">
  <objectgroup draworder="index">
   <object id="1" x="3.875" y="0.3125" width="8.75" height="15.375"/>
  </objectgroup>
 </tile>
 <tile id="1154">
  <objectgroup draworder="index">
   <object id="1" x="0.136365" y="0.272718" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="1155">
  <objectgroup draworder="index">
   <object id="1" x="8.04546" y="0.272718" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="1163">
  <properties>
   <property name="destructable" value="21,4,1,3"/>
   <property name="loot" value="520:100:5,1003:10:2"/>
   <property name="spawnWith" value="1063"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="4" y="0.25" width="7.125" height="10.75"/>
  </objectgroup>
 </tile>
 <tile id="1185">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="1186">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="1"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="1194">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="1196">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="1204">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1205">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1206">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1207">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1208">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1209">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1210">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1211">
  <properties>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1212">
  <properties>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1213">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1214">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1215">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1216">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1217">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1218">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1219">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1220">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1221">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1222">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1223">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1224">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1225">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1249">
  <objectgroup draworder="index">
   <object id="1" x="3.85227" y="0.181809" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="1254">
  <objectgroup draworder="index">
   <object id="1" x="0.136365" y="0.272718" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="1255">
  <objectgroup draworder="index">
   <object id="1" x="8.22727" y="0.0909" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="1271">
  <properties>
   <property name="destructable" value="22,3,0,8"/>
   <property name="loot" value="646:100:1"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="1272">
  <properties>
   <property name="destructable" value="22,3,0,8"/>
   <property name="loot" value="645:100:1"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="1273">
  <properties>
   <property name="destructable" value="22,3,0,8"/>
   <property name="loot" value="644:100:1"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="1274">
  <properties>
   <property name="destructable" value="22,3,0,8"/>
   <property name="loot" value="643:100:1"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="1275">
  <properties>
   <property name="destructable" value="22,3,0,8"/>
   <property name="loot" value="642:100:1"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="1276">
  <properties>
   <property name="destructable" value="22,3,0,8"/>
   <property name="loot" value="641:100:1"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="1277">
  <properties>
   <property name="destructable" value="22,3,0,8"/>
   <property name="loot" value="640:100:1"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.0909091" y="0.0909091" width="15.8182" height="14"/>
  </objectgroup>
 </tile>
 <tile id="1285">
  <properties>
   <property name="destructable" value="2,0,3,6"/>
   <property name="loot" value="712:25:2"/>
   <property name="spawnWith" value="1185,1186,1286"/>
  </properties>
 </tile>
 <tile id="1286">
  <properties>
   <property name="layer" value="1"/>
   <property name="relationX" value="1"/>
   <property name="relationY" value="0"/>
  </properties>
 </tile>
 <tile id="1293">
  <properties>
   <property name="destructable" value="-50,1,0,16"/>
   <property name="loot" value="789:100:1"/>
  </properties>
 </tile>
 <tile id="1294">
  <properties>
   <property name="destructable" value="-50,1,0,5"/>
   <property name="loot" value="183:100:1"/>
   <property name="spawnWith" value="1194"/>
  </properties>
 </tile>
 <tile id="1296">
  <properties>
   <property name="destructable" value="-50,1,0,5"/>
   <property name="loot" value="184:100:1"/>
   <property name="spawnWith" value="1196"/>
  </properties>
 </tile>
 <tile id="1297">
  <properties>
   <property name="destructable" value="-50,1,0,5"/>
   <property name="loot" value="192:100:1"/>
  </properties>
 </tile>
 <tile id="1308">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1309">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1310">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1311">
  <properties>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1312">
  <properties>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1313">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1314">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1315">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1316">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1317">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1318">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1319">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1320">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1321">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1322">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1323">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1349">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="4.25" y="7.625" width="7.875" height="5"/>
  </objectgroup>
 </tile>
 <tile id="1350">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="-0.125" y="9.5" width="16" height="5"/>
  </objectgroup>
 </tile>
 <tile id="1351">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="-0.125" y="9" width="16" height="5"/>
  </objectgroup>
 </tile>
 <tile id="1352">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0" y="9.25" width="12" height="5"/>
  </objectgroup>
 </tile>
 <tile id="1353">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="4.5" y="8.375" width="11.5" height="5"/>
  </objectgroup>
 </tile>
 <tile id="1355">
  <objectgroup draworder="index">
   <object id="1" x="8.22727" y="0.363627" width="7.72727" height="15.8182"/>
  </objectgroup>
 </tile>
 <tile id="1402">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1403">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1404">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1405">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1406">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1407">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1408">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1409">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1410">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1411">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1412">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1413">
  <properties>
   <property name="generate" value="dirt"/>
   <property name="step" value="4"/>
  </properties>
 </tile>
 <tile id="1414">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1415">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1416">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1417">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1418">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1419">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="grass"/>
   <property name="step" value="1"/>
  </properties>
 </tile>
 <tile id="1420">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1421">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1422">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1423">
  <properties>
   <property name="generate" value="sand"/>
  </properties>
 </tile>
 <tile id="1457">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="1475">
  <properties>
   <property name="destructable" value="21,4,1,3"/>
   <property name="loot" value="520:100:5"/>
   <property name="spawnWith" value="1476,1477"/>
  </properties>
  <objectgroup draworder="index">
   <object id="2" x="5.18182" y="11.9091" width="10.7273" height="4"/>
  </objectgroup>
 </tile>
 <tile id="1476">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="1"/>
   <property name="relationY" value="0"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.181805" y="11.8182" width="15.4546" height="4"/>
  </objectgroup>
 </tile>
 <tile id="1477">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="2"/>
   <property name="relationY" value="0"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.363623" y="11.8182" width="10.7273" height="4"/>
  </objectgroup>
 </tile>
 <tile id="1484">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="1485">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="1502">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1503">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1504">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1505">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1506">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1507">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1550">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0" y="7.875" width="11.5" height="5"/>
  </objectgroup>
 </tile>
 <tile id="1551">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="3.75" y="9.75" width="8.25" height="5"/>
  </objectgroup>
 </tile>
 <tile id="1552">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="4" y="8.125" width="12.25" height="5"/>
  </objectgroup>
 </tile>
 <tile id="1557">
  <properties>
   <property name="newSource" value="0,-16,0,16"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.318183" y="7.99999" width="7.72727" height="5.36365"/>
  </objectgroup>
 </tile>
 <tile id="1563">
  <properties>
   <property name="layer" value="3"/>
   <property name="newSource" value="-16,-48,48,64"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="1580">
  <properties>
   <property name="destructable" value="-50,1,0,5"/>
   <property name="loot" value="1000:100:1"/>
  </properties>
 </tile>
 <tile id="1581">
  <properties>
   <property name="destructable" value="-50,1,0,16"/>
   <property name="loot" value="710:100:1"/>
  </properties>
 </tile>
 <tile id="1582">
  <properties>
   <property name="destructable" value="-50,1,0,16"/>
   <property name="loot" value="711:100:1"/>
  </properties>
 </tile>
 <tile id="1583">
  <properties>
   <property name="destructable" value="-50,1,0,5"/>
   <property name="loot" value="709:100"/>
   <property name="spawnWith" value="1483"/>
  </properties>
 </tile>
 <tile id="1584">
  <properties>
   <property name="destructable" value="2,0,0,6"/>
   <property name="loot" value="713:100:1"/>
   <property name="spawnWith" value="1484"/>
  </properties>
 </tile>
 <tile id="1585">
  <properties>
   <property name="destructable" value="-50,1,0,5"/>
   <property name="loot" value="708:100:1"/>
   <property name="spawnWith" value="1485"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="3" y="0.272727" width="7.72727" height="4"/>
  </objectgroup>
 </tile>
 <tile id="1604">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1605">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1663">
  <properties>
   <property name="destructable" value="21,4,1,3"/>
   <property name="loot" value="520:100:5,1003:10:2"/>
   <property name="spawnWith" value="1563"/>
  </properties>
  <objectgroup draworder="index">
   <object id="2" x="3" y="0.375" width="10" height="11.625"/>
  </objectgroup>
 </tile>
 <tile id="1690">
  <properties>
   <property name="idleStart" value=""/>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
  <animation>
   <frame tileid="1490" duration="100"/>
  </animation>
 </tile>
 <tile id="1692">
  <properties>
   <property name="idleStart" value=""/>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
  <animation>
   <frame tileid="1492" duration="100"/>
  </animation>
 </tile>
 <tile id="1704">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1705">
  <properties>
   <property name="step" value="2"/>
  </properties>
 </tile>
 <tile id="1752">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="1790">
  <properties>
   <property name="action" value="sanctuaryAdd,123,0,0"/>
   <property name="idleStart" value=""/>
   <property name="spawnWith" value="1690"/>
  </properties>
  <animation>
   <frame tileid="1590" duration="100"/>
  </animation>
 </tile>
 <tile id="1792">
  <properties>
   <property name="action" value="sanctuaryAdd,123,0,0"/>
   <property name="idleStart" value=""/>
   <property name="spawnWith" value="1692"/>
  </properties>
  <animation>
   <frame tileid="1592" duration="100"/>
  </animation>
 </tile>
 <tile id="1843">
  <properties>
   <property name="newHitBox" value="0,8, 32, 16"/>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
 </tile>
 <tile id="1845">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
  </properties>
  <objectgroup draworder="index">
   <object id="3" x="6.09091" y="0.727273" width="4.90909" height="8.90909"/>
  </objectgroup>
 </tile>
 <tile id="1846">
  <properties>
   <property name="newSource" value="0,-16,16,16"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="7.27273" y="4.54545" width="3.63636" height="6.36364"/>
  </objectgroup>
 </tile>
 <tile id="1848">
  <objectgroup draworder="index">
   <object id="1" x="2" y="4" width="11" height="8"/>
  </objectgroup>
 </tile>
 <tile id="1852">
  <properties>
   <property name="AssociatedTiles" value="1752"/>
   <property name="action" value="chestLoot"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="1" y="5" width="14" height="8"/>
  </objectgroup>
 </tile>
 <tile id="2050">
  <properties>
   <property name="newHitBox" value="0, -10, 16, 16"/>
  </properties>
 </tile>
 <tile id="2051">
  <properties>
   <property name="newHitBox" value="0, -10, 16, 16"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0.454545" y="0.0909091" width="14.8182" height="10.5455"/>
  </objectgroup>
 </tile>
 <tile id="2057">
  <properties>
   <property name="action" value="enterPlayerHouse"/>
  </properties>
 </tile>
 <tile id="2139">
  <properties>
   <property name="action" value="cook"/>
   <property name="newHitBox" value="0, -10, 16, 16"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="2142">
  <properties>
   <property name="newHitBox" value="0, -10, 16, 16"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="2143">
  <properties>
   <property name="action" value="smelt"/>
   <property name="newHitBox" value="0, -10, 16, 16"/>
   <property name="newSource" value="0,-32,16,48"/>
  </properties>
 </tile>
 <tile id="2144">
  <properties>
   <property name="newHitBox" value="0,8, 32, 16"/>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
 </tile>
 <tile id="2157">
  <properties>
   <property name="action" value="enterPlayerHouse"/>
   <property name="newHitBox" value="-32,-16,64,32"/>
   <property name="newSource" value="-32,-48,64,64"/>
  </properties>
 </tile>
 <tile id="2163">
  <properties>
   <property name="layer" value="3"/>
   <property name="newSource" value="-32,-48,80,64"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="2263">
  <properties>
   <property name="destructable" value="21,4,1,3"/>
   <property name="loot" value="521:25:5,1003:5:2"/>
   <property name="spawnWith" value="2163"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="6" y="0.375" width="6" height="10.625"/>
  </objectgroup>
 </tile>
 <tile id="2304">
  <properties>
   <property name="action" value="triggerLift"/>
   <property name="newSource" value="-16,-48,32,48"/>
  </properties>
 </tile>
 <tile id="2624">
  <objectgroup draworder="index">
   <object id="1" x="0" y="10" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="2625">
  <objectgroup draworder="index">
   <object id="1" x="0" y="10" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="2626">
  <objectgroup draworder="index">
   <object id="1" x="0" y="10" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="2627">
  <objectgroup draworder="index">
   <object id="1" x="0" y="10" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="2628">
  <objectgroup draworder="index">
   <object id="1" x="0" y="10" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="2629">
  <objectgroup draworder="index">
   <object id="1" x="0" y="10" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="2630">
  <objectgroup draworder="index">
   <object id="1" x="0" y="10" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="2631">
  <objectgroup draworder="index">
   <object id="1" x="0" y="10" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="2723">
  <objectgroup draworder="index">
   <object id="1" x="1" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="2732">
  <objectgroup draworder="index">
   <object id="1" x="11" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="2823">
  <objectgroup draworder="index">
   <object id="1" x="4" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="2832">
  <objectgroup draworder="index">
   <object id="1" x="8" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="2863">
  <properties>
   <property name="layer" value="3"/>
   <property name="newSource" value="-16,-64,48,80"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="2923">
  <objectgroup draworder="index">
   <object id="1" x="3" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="2932">
  <objectgroup draworder="index">
   <object id="1" x="9" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="2963">
  <properties>
   <property name="destructable" value="21,4,1,3"/>
   <property name="loot" value="520:100:5,1003:10:2"/>
   <property name="spawnWith" value="2863"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="3.125" y="0.375" width="11.25" height="10.5"/>
  </objectgroup>
 </tile>
 <tile id="3023">
  <objectgroup draworder="index">
   <object id="1" x="3" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3032">
  <objectgroup draworder="index">
   <object id="1" x="9" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3123">
  <objectgroup draworder="index">
   <object id="1" x="3" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3132">
  <objectgroup draworder="index">
   <object id="1" x="9" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3223">
  <objectgroup draworder="index">
   <object id="1" x="2" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3232">
  <objectgroup draworder="index">
   <object id="1" x="10" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3323">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3332">
  <objectgroup draworder="index">
   <object id="1" x="12" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3423">
  <objectgroup draworder="index">
   <object id="1" x="3" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3432">
  <objectgroup draworder="index">
   <object id="1" x="10" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3523">
  <objectgroup draworder="index">
   <object id="1" x="3" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3532">
  <objectgroup draworder="index">
   <object id="1" x="9" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3563">
  <properties>
   <property name="layer" value="3"/>
   <property name="newSource" value="-16,-64,48,80"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="3623">
  <objectgroup draworder="index">
   <object id="1" x="3" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3632">
  <objectgroup draworder="index">
   <object id="1" x="10" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3663">
  <properties>
   <property name="destructable" value="21,4,1,3"/>
   <property name="loot" value="520:100:5,1003:10:2"/>
   <property name="spawnWith" value="3563"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="1.125" y="0.3125" width="13.6875" height="2.3125"/>
  </objectgroup>
 </tile>
 <tile id="3723">
  <objectgroup draworder="index">
   <object id="1" x="2" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3732">
  <objectgroup draworder="index">
   <object id="1" x="10" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3823">
  <objectgroup draworder="index">
   <object id="1" x="2" y="0" width="14" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3832">
  <objectgroup draworder="index">
   <object id="1" x="8" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="3925">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="3926">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="3927">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="3928">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="3929">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="3930">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="16" height="6"/>
  </objectgroup>
 </tile>
 <tile id="4180">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-2"/>
  </properties>
 </tile>
 <tile id="4205">
  <properties>
   <property name="lightSource" value="1"/>
  </properties>
 </tile>
 <tile id="4280">
  <properties>
   <property name="action" value="triggerLift"/>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="4281">
  <properties>
   <property name="action" value="triggerLift"/>
  </properties>
 </tile>
 <tile id="4380">
  <properties>
   <property name="AssociatedTiles" value="4280,4180"/>
   <property name="action" value="replaceSmallCog"/>
  </properties>
 </tile>
 <tile id="4413">
  <properties>
   <property name="prop" value="prop"/>
  </properties>
 </tile>
 <tile id="4614">
  <properties>
   <property name="prop" value="prop"/>
  </properties>
 </tile>
 <tile id="6067">
  <properties>
   <property name="action" value="readSanctuary"/>
  </properties>
 </tile>
 <tile id="6068">
  <properties>
   <property name="action" value="readSanctuary"/>
  </properties>
 </tile>
 <tile id="6069">
  <properties>
   <property name="action" value="readSanctuary"/>
  </properties>
 </tile>
 <tile id="6167">
  <properties>
   <property name="action" value="readSanctuary"/>
  </properties>
 </tile>
 <tile id="6168">
  <properties>
   <property name="action" value="readSanctuary"/>
  </properties>
 </tile>
 <tile id="6169">
  <properties>
   <property name="action" value="readSanctuary"/>
  </properties>
 </tile>
 <tile id="6759">
  <properties>
   <property name="lightSource" value="1"/>
  </properties>
 </tile>
 <tile id="7159">
  <properties>
   <property name="newSource" value="0,-64,0,64"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="1.375" y="0.5" width="12.75" height="12.75"/>
  </objectgroup>
 </tile>
 <tile id="7914">
  <animation>
   <frame tileid="7614" duration="100"/>
   <frame tileid="7314" duration="100"/>
  </animation>
 </tile>
 <tile id="7915">
  <animation>
   <frame tileid="7615" duration="100"/>
   <frame tileid="7315" duration="100"/>
  </animation>
 </tile>
 <tile id="8014">
  <animation>
   <frame tileid="7714" duration="100"/>
   <frame tileid="7414" duration="100"/>
  </animation>
 </tile>
 <tile id="8015">
  <animation>
   <frame tileid="7715" duration="100"/>
   <frame tileid="7415" duration="100"/>
  </animation>
 </tile>
 <tile id="8114">
  <animation>
   <frame tileid="7814" duration="100"/>
   <frame tileid="7514" duration="100"/>
  </animation>
 </tile>
 <tile id="8115">
  <animation>
   <frame tileid="7815" duration="100"/>
   <frame tileid="7515" duration="100"/>
  </animation>
 </tile>
 <tile id="9024">
  <properties>
   <property name="action" value="triggerLift"/>
   <property name="newHitBox" value="-16,-16,64,32"/>
   <property name="newSource" value="-32,-96,96,112"/>
  </properties>
 </tile>
 <tile id="9624">
  <properties>
   <property name="action" value="triggerLift"/>
   <property name="newSource" value="-32,-80,96,96"/>
  </properties>
 </tile>
 <tile id="9702">
  <animation>
   <frame tileid="9602" duration="100"/>
   <frame tileid="9502" duration="100"/>
   <frame tileid="9402" duration="100"/>
   <frame tileid="9302" duration="100"/>
   <frame tileid="9202" duration="100"/>
   <frame tileid="9102" duration="100"/>
   <frame tileid="9002" duration="100"/>
   <frame tileid="8902" duration="100"/>
   <frame tileid="8802" duration="100"/>
   <frame tileid="8702" duration="100"/>
   <frame tileid="8602" duration="100"/>
   <frame tileid="8502" duration="100"/>
   <frame tileid="8402" duration="100"/>
   <frame tileid="8302" duration="100"/>
   <frame tileid="8202" duration="100"/>
   <frame tileid="8102" duration="100"/>
   <frame tileid="8002" duration="100"/>
  </animation>
 </tile>
 <tile id="9703">
  <animation>
   <frame tileid="9603" duration="100"/>
   <frame tileid="9503" duration="100"/>
   <frame tileid="9403" duration="100"/>
   <frame tileid="9303" duration="100"/>
   <frame tileid="9203" duration="100"/>
   <frame tileid="9103" duration="100"/>
   <frame tileid="9003" duration="100"/>
   <frame tileid="8903" duration="100"/>
   <frame tileid="8803" duration="100"/>
   <frame tileid="8703" duration="100"/>
   <frame tileid="8603" duration="100"/>
   <frame tileid="8503" duration="100"/>
   <frame tileid="8403" duration="100"/>
   <frame tileid="8303" duration="100"/>
   <frame tileid="8203" duration="100"/>
   <frame tileid="8103" duration="100"/>
   <frame tileid="8003" duration="100"/>
  </animation>
 </tile>
 <tile id="9704">
  <animation>
   <frame tileid="9604" duration="100"/>
   <frame tileid="9504" duration="100"/>
   <frame tileid="9404" duration="100"/>
   <frame tileid="9304" duration="100"/>
   <frame tileid="9204" duration="100"/>
   <frame tileid="9104" duration="100"/>
   <frame tileid="9004" duration="100"/>
   <frame tileid="8904" duration="100"/>
   <frame tileid="8804" duration="100"/>
   <frame tileid="8704" duration="100"/>
   <frame tileid="8604" duration="100"/>
   <frame tileid="8504" duration="100"/>
   <frame tileid="8404" duration="100"/>
   <frame tileid="8304" duration="100"/>
   <frame tileid="8204" duration="100"/>
   <frame tileid="8104" duration="100"/>
   <frame tileid="8004" duration="100"/>
  </animation>
 </tile>
 <tile id="9705">
  <animation>
   <frame tileid="9605" duration="100"/>
   <frame tileid="9505" duration="100"/>
   <frame tileid="9405" duration="100"/>
   <frame tileid="9305" duration="100"/>
   <frame tileid="9205" duration="100"/>
   <frame tileid="9105" duration="100"/>
   <frame tileid="9005" duration="100"/>
   <frame tileid="8905" duration="100"/>
   <frame tileid="8805" duration="100"/>
   <frame tileid="8705" duration="100"/>
   <frame tileid="8605" duration="100"/>
   <frame tileid="8505" duration="100"/>
   <frame tileid="8405" duration="100"/>
   <frame tileid="8305" duration="100"/>
   <frame tileid="8205" duration="100"/>
   <frame tileid="8105" duration="100"/>
   <frame tileid="8005" duration="100"/>
  </animation>
 </tile>
 <tile id="9707">
  <animation>
   <frame tileid="9707" duration="100"/>
   <frame tileid="9607" duration="100"/>
   <frame tileid="9507" duration="100"/>
   <frame tileid="9407" duration="100"/>
   <frame tileid="9307" duration="100"/>
   <frame tileid="9207" duration="100"/>
   <frame tileid="9107" duration="100"/>
   <frame tileid="9007" duration="100"/>
   <frame tileid="8907" duration="100"/>
   <frame tileid="8807" duration="100"/>
   <frame tileid="8707" duration="100"/>
   <frame tileid="8607" duration="100"/>
   <frame tileid="8507" duration="100"/>
   <frame tileid="8407" duration="100"/>
   <frame tileid="8307" duration="100"/>
   <frame tileid="8207" duration="100"/>
   <frame tileid="8107" duration="100"/>
   <frame tileid="8007" duration="100"/>
  </animation>
 </tile>
 <tile id="9708">
  <animation>
   <frame tileid="9708" duration="100"/>
   <frame tileid="9608" duration="100"/>
   <frame tileid="9508" duration="100"/>
   <frame tileid="9408" duration="100"/>
   <frame tileid="9308" duration="100"/>
   <frame tileid="9208" duration="100"/>
   <frame tileid="9108" duration="100"/>
   <frame tileid="9008" duration="100"/>
   <frame tileid="8908" duration="100"/>
   <frame tileid="8808" duration="100"/>
   <frame tileid="8708" duration="100"/>
   <frame tileid="8608" duration="100"/>
   <frame tileid="8508" duration="100"/>
   <frame tileid="8408" duration="100"/>
   <frame tileid="8308" duration="100"/>
   <frame tileid="8208" duration="100"/>
   <frame tileid="8108" duration="100"/>
   <frame tileid="8008" duration="100"/>
  </animation>
 </tile>
 <tile id="9709">
  <animation>
   <frame tileid="9709" duration="100"/>
   <frame tileid="9609" duration="100"/>
   <frame tileid="9509" duration="100"/>
   <frame tileid="9409" duration="100"/>
   <frame tileid="9309" duration="100"/>
   <frame tileid="9209" duration="100"/>
   <frame tileid="9109" duration="100"/>
   <frame tileid="9009" duration="100"/>
   <frame tileid="8909" duration="100"/>
   <frame tileid="8809" duration="100"/>
   <frame tileid="8709" duration="100"/>
   <frame tileid="8609" duration="100"/>
   <frame tileid="8509" duration="100"/>
   <frame tileid="8409" duration="100"/>
   <frame tileid="8309" duration="100"/>
   <frame tileid="8209" duration="100"/>
   <frame tileid="8109" duration="100"/>
   <frame tileid="8009" duration="100"/>
  </animation>
 </tile>
 <tile id="9710">
  <animation>
   <frame tileid="9610" duration="100"/>
   <frame tileid="9510" duration="100"/>
   <frame tileid="9410" duration="100"/>
   <frame tileid="9310" duration="100"/>
   <frame tileid="9210" duration="100"/>
   <frame tileid="9110" duration="100"/>
   <frame tileid="9010" duration="100"/>
   <frame tileid="8910" duration="100"/>
   <frame tileid="8810" duration="100"/>
   <frame tileid="8710" duration="100"/>
   <frame tileid="8610" duration="100"/>
   <frame tileid="8510" duration="100"/>
   <frame tileid="8410" duration="100"/>
   <frame tileid="8310" duration="100"/>
   <frame tileid="8210" duration="100"/>
   <frame tileid="8110" duration="100"/>
   <frame tileid="8010" duration="100"/>
  </animation>
 </tile>
</tileset>
