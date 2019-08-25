<?xml version="1.0" encoding="UTF-8"?>
<tileset name="MasterSpriteSheet" tilewidth="16" tileheight="16" tilecount="10000" columns="100">
 <image source="../../../Map/MasterSpriteSheet.png" width="1600" height="1600"/>
 <tile id="85">
  <properties>
   <property name="action" value="plantable"/>
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
   <property name="loot" value="171:100"/>
  </properties>
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
 <tile id="478">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
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
   <property name="loot" value="171:100"/>
  </properties>
 </tile>
 <tile id="578">
  <properties>
   <property name="AnimatedX" value="7"/>
   <property name="AssociatedTiles" value="5580"/>
   <property name="Speed" value=".15"/>
   <property name="destructable" value="1,1,0,2"/>
   <property name="loot" value="169:100"/>
   <property name="spawnWith" value="478"/>
  </properties>
 </tile>
 <tile id="678">
  <properties>
   <property name="layer" value="3"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
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
 <tile id="778">
  <properties>
   <property name="AnimatedX" value="7"/>
   <property name="AssociatedTiles" value="5780"/>
   <property name="Speed" value=".15"/>
   <property name="destructable" value="1,1,0,8"/>
   <property name="loot" value="149:100"/>
   <property name="spawnWith" value="678"/>
  </properties>
 </tile>
 <tile id="904">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="905">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="906">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="907">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="978">
  <properties>
   <property name="AnimatedX" value="5"/>
   <property name="Speed" value=".15"/>
   <property name="destructable" value="1,1,0,8"/>
   <property name="idleStart" value=""/>
   <property name="loot" value="130:100,183:15"/>
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
 <tile id="1004">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="1005">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="1006">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="1007">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="1078">
  <properties>
   <property name="AnimatedX" value="6"/>
   <property name="Speed" value=".15"/>
   <property name="destructable" value="2,1,3,6"/>
   <property name="loot" value="129:100"/>
   <property name="tileSelectorAllowed" value="3"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="2" y="2" width="12" height="11"/>
  </objectgroup>
 </tile>
 <tile id="1104">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="1105">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="1106">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="1107">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
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
 <tile id="1204">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="1205">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="1206">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="1207">
  <properties>
   <property name="action" value="diggable"/>
   <property name="generate" value="dirt"/>
  </properties>
 </tile>
 <tile id="1285">
  <properties>
   <property name="destructable" value="2,0,3,6"/>
   <property name="loot" value="181:25"/>
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
 <tile id="1563">
  <properties>
   <property name="layer" value="3"/>
   <property name="newSource" value="-8,-24,16,24"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="1583">
  <properties>
   <property name="destructable" value="-50,1,0,5"/>
   <property name="loot" value="171:100"/>
   <property name="spawnWith" value="1483"/>
  </properties>
 </tile>
 <tile id="1584">
  <properties>
   <property name="destructable" value="2,0,0,6"/>
   <property name="loot" value="191:100"/>
   <property name="spawnWith" value="1484"/>
  </properties>
 </tile>
 <tile id="1585">
  <properties>
   <property name="destructable" value="-50,1,0,5"/>
   <property name="loot" value="171:100"/>
   <property name="spawnWith" value="1485"/>
  </properties>
 </tile>
 <tile id="1663">
  <properties>
   <property name="destructable" value="0,4,1,3"/>
   <property name="loot" value="142:100,122:10"/>
   <property name="spawnWith" value="1563"/>
  </properties>
  <objectgroup draworder="index">
   <object id="2" x="3" y="4" width="10" height="8"/>
  </objectgroup>
 </tile>
 <tile id="2163">
  <properties>
   <property name="layer" value="3"/>
   <property name="newSource" value="-16,-24,32,24"/>
   <property name="relationX" value="0"/>
   <property name="relationY" value="-1"/>
  </properties>
 </tile>
 <tile id="2263">
  <properties>
   <property name="destructable" value="0,4,1,3"/>
   <property name="loot" value="123:100,122:5"/>
   <property name="spawnWith" value="2163"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="6" y="8" width="6" height="3"/>
  </objectgroup>
 </tile>
 <tile id="4205">
  <properties>
   <property name="lightSource" value="1"/>
  </properties>
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
</tileset>
