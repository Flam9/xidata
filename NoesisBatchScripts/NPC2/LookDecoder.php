<?php

$lookTable = [];

/**
 * This is a translation from:
 * - process_npc_data
 * - unpack_gear_id
 * 
 * From the "NPCDecoder" Windower Addon.
 */
function getModelIdFromLookData($lookdata) {
    $a = hexdec(substr($lookdata, 0, 2));
    $b = hexdec(substr($lookdata, 2, 2));

    $shift = $b << 8;
    $combined = $a + $shift;
    $masked = $combined & 0x0FFF;

    return $masked;
}

/**
 * Basic write to console line
 */
function console(string $text) {
    $date = date('Y-m-d H:i:s');
    echo "[{$date}] {$text}\n";
}

/**
 * Builds the look table from the "gear_dat_json" files
 */
function buildLookTable() {
    global $lookTable;

    console("Building gear lookup table...");

    $dirJson = __DIR__ .'/gear_dat_json';
    $dirJsonScan = array_diff(scandir($dirJson), ['..', '.']);

    foreach ($dirJsonScan as $folderRace) {
        console("- Race: {$folderRace}");
        $jsonGearFiles = array_diff(scandir("{$dirJson}/{$folderRace}"), ['..', '.']);

        foreach ($jsonGearFiles as $jsonGearFile) {
            $jsonGear = file_get_contents("{$dirJson}/{$folderRace}/{$jsonGearFile}");
            $jsonGear = json_decode($jsonGear, true);

            $slot =  pathinfo($jsonGearFile, PATHINFO_FILENAME);
            console("   Slot: {$slot}");

            $lookTable[$folderRace][$slot] = $jsonGear;
        }
    }

    console("!! Database Built!");
    console(" ");
}

/**
 * Find the model information from the LookTable 
 * using race, slot and model id.
 */
function findModelData($race, $slot, $modelId) {
    global $lookTable;

    $data = $lookTable[$race][$slot];

    foreach ($data as $d) {
        if ($d['ModelID'] == $modelId) {
            return $d;
        }
    }

    return null;
}

// reset this file
file_put_contents(__DIR__ .'/LookFailed.txt', '');

/**
 * Write a failed entry to the LookFailed file
 */
function writeFailedEntry($npc) {
    file_put_contents(__DIR__ .'/LookFailed.txt', json_encode($npc, JSON_PRETTY_PRINT) . "\n", FILE_APPEND);
}

/**
 * Writes out an Noesis ff11datset file
 */
$noesisDatsetDirectory = __DIR__ .'/NpcLookDatsets_Noesis';
function writeNoesisFile($npc) {
    global $noesisDatsetDirectory;

    $datset = [];
    $datset[] = 'NOESIS_FF11_DAT_SET';
    $datset[] = '';
    $datset[] = 'setPathAbs	"E:/SquareEnix/SquareEnix/FINAL FANTASY XI/"';
    $datset[] = '';

    // Main skele is the one from race
    $datset[] = sprintf('dat			"__skeleton"	"%s"', $npc['race']['dat']);

    // basic stuff all npcs have
    $datset[] = sprintf('dat			"face"	"%s"', $npc['face']['Path']);
    $datset[] = sprintf('dat			"body"	"%s"', $npc['body']['Path']);

    if (!$npc['body']) {
        print_r($npc);
        die;
    }
    
    /**
     * Big thanks to Ashenbubs for noticing this, a lot of slots are optional for things like:
     * - Dinner Jacket, Behemoth Body, Moogle Suit, etc. However you'll always have a Face+Body
     */
    foreach (['head', 'hands', 'feet', 'legs', 'main', 'sub', 'ranged'] as $optionalSlot) {
        if ($npc[$optionalSlot]) {
            $datset[] = sprintf('dat			"%s"	"%s"', $optionalSlot, $npc[$optionalSlot]['Path']);
        }
    }

    // ensure we have the zone folder
    $saveDirectory = "{$noesisDatsetDirectory}/{$npc['data']['zone']}";

    if (!is_dir($saveDirectory)) {
        mkdir($saveDirectory);
    }

    // save filename
    $simpleName = simpleName($npc['data']['name']);
    $saveFilename = "{$saveDirectory}/{$npc['data']['id']}_{$simpleName}.ff11datset";

    file_put_contents($saveFilename, implode("\n", $datset));
}

$jsonDatsetDirectory = __DIR__ .'/NpcLookDatsets_Json';
function writeJsonFile($npc) {
    global $jsonDatsetDirectory;

    $json = json_encode($npc, JSON_PRETTY_PRINT);

    // ensure we have the zone folder
    $saveDirectory = "{$jsonDatsetDirectory}/{$npc['data']['zone']}";

    if (!is_dir($saveDirectory)) {
        mkdir($saveDirectory);
    }

    // save filename
    $simpleName = simpleName($npc['data']['name']);
    $saveFilename = "{$saveDirectory}/{$npc['data']['id']}_{$simpleName}.json";

    file_put_contents($saveFilename, $json);
}

/**
 * Formats a nice simple name and strips everything, 
 * changing spaces to _ and fixing the ??? names.
 */
function simpleName($string) {
    // remove all non-alphanumeric characters
    $string = preg_replace("/[^a-zA-Z0-9]+/", "", $string);

    // convert to lowercase
    $string = strtolower($string);

    // replace spaces with underscores
    $string = str_replace(" ", "_", $string);

    if (empty($string)) {
        return 'question_mark';
    }

    return $string;
}

// Build look table ...
buildLookTable();

// This is a test look (overwriten by CSV)
$npcList = [
    '17617233,Putori-Tutori,"0","0.000","0.000","0.000","010003054B104B204B304B404B504E6100700000",,Ifrits_Cauldron',
    '17617234,Kilhwch,"0","0.000","0.000","0.000","01000D014D104D204D304D404D50406100700000",,Ifrits_Cauldron',
    '17617235,Mieuseloir,"0","0.000","0.000","0.000","010000034D104D204D304D404D503F6100700000",,Ifrits_Cauldron',
    '17617236,Lhu Mhakaracca,"0","0.000","0.000","0.000","01000D07511051205130514051505B6100700000",,Ifrits_Cauldron',
    '17617237,Xonia,"0","0.000","0.000","0.000","01000D0241104120413041404150536100700000",,Ifrits_Cauldron',
    '17617238,Fonove,"0","0.000","0.000","0.000","01000C044F104F204F304F404F50526100700000",,Ifrits_Cauldron',
    '17617239,Dzhau Yaam,"0","0.000","0.000","0.000","010007075B105B205B305B405B50D96000700000",,Ifrits_Cauldron',
    '17617240,Kupalu-Harupalu,"0","0.000","0.000","0.000","01000C0559105920593059405950586100700000",,Ifrits_Cauldron',
    '17617241,Noillurie,"0","0.000","0.000","0.000","01000A0457105720573057405750546100700000",,Ifrits_Cauldron',
    '17617242,Umon-Paumon,"0","0.000","0.000","0.000","01000B0547104720473047404750566100700000",,Ifrits_Cauldron',
    '17617243,Ghebben,"0","0.000","0.000","0.000","01000C0855105520553055405550006000700000",,Ifrits_Cauldron',
    '17617244,Elrica,"0","0.000","0.000","0.000","0100040255105520553055405550006000700000",,Ifrits_Cauldron',
    '17617245,Terounalivet,"0","0.000","0.000","0.000","0100060353105320533053405350006000700000",,Ifrits_Cauldron',
    '17617246,???,"0","0.000","0.000","0.000","0000320000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617247,Ramblix,77,102.968,"0.503",-303.176,"0000550000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617248,Goblin Footprint,77,102.968,"0.503",-303.176,"0000340000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617249,GoalPoint,7,-59.858,1.575,-267.641,"0000340000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617250,GoalPoint,158,"-178.950","0.000",-79.581,"0000340000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617251,GoalPoint,252,35.508,"9.240",178.501,"0000340000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617252,GoalPoint,36,58.273,"0.146","53.870","0000340000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617253,GoalPoint,"0","0.000","0.000","0.000","0000340000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617254,GoalPoint,"0","0.000","0.000","0.000","0000340000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617255,Moogle,7,-59.858,1.575,-267.641,"0000520000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617256,Moogle,158,"-178.950","0.000",-79.581,"0000520000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617257,Moogle,252,35.508,"9.240",178.501,"0000520000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617258,Moogle,36,58.273,"0.146","53.870","0000520000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617259,Moogle,"0","0.000","0.000","0.000","0000520000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617260,Moogle,"0","0.000","0.000","0.000","0000520000000000000000000000000000000000",,Ifrits_Cauldron',
    '17617261,Planar Rift,"0","-269.000",3.599,"-169.000","00006F0900000000000000000000000000000000",VOIDWATCH,Ifrits_Cauldron',
    '17617262,Planar Rift,"0","-125.000",3.635,"170.000","00006F0900000000000000000000000000000000",VOIDWATCH,Ifrits_Cauldron',
    '17617263,Planar Rift,"0","9.000",3.799,"290.000","00006F0900000000000000000000000000000000",VOIDWATCH,Ifrits_Cauldron',
    '17617264,Riftworn Pyxis,"0",-268.899,3.599,"-169.000","0000C90300000000000000000000000000000000",VOIDWATCH,Ifrits_Cauldron',
    '17617265,Riftworn Pyxis,"0","-124.900",3.635,"170.000","0000C90300000000000000000000000000000000",VOIDWATCH,Ifrits_Cauldron',
    '17617266,Riftworn Pyxis,"0","9.100",3.799,"290.000","0000C90300000000000000000000000000000000",VOIDWATCH,Ifrits_Cauldron',
    '17617267,Grounds Tome,113,-19.991,2.676,-256.924,"0000F20800000000000000000000000000000000",GROUNDS_TOMES,Ifrits_Cauldron',
    '17617268,Grounds Tome,57,-208.951,"0.891",-182.458,"0000F20800000000000000000000000000000000",GROUNDS_TOMES,Ifrits_Cauldron',
    '17617269,Grounds Tome,1,-57.389,"0.797",128.889,"0000F20800000000000000000000000000000000",GROUNDS_TOMES,Ifrits_Cauldron',
];

// Parse from CSV
$npcList = file_get_contents(__DIR__ .'/LookNpcDBData.csv');
$npcList = explode("\n", $npcList);

/**
 * Look data is fetched like so (from LandBoatSea Database)
 * 
 * 
    SELECT 
        npc_list.npcid as npc_id,
        npc_list.polutils_name as npc_name,
        npc_list.pos_rot as npc_pos_rotation,
        npc_list.pos_x as npc_pos_x,
        npc_list.pos_y as npc_pos_y,
        npc_list.pos_z as npc_pos_z,
        HEX(`look`) as npc_look,
        npc_list.content_tag as npc_content,
        zone_settings.name as zone_name
    FROM npc_list 
    INNER JOIN zone_settings
    ON (npc_list.npcid & 0xFFF000) >> 12 = zone_settings.zoneid
    WHERE npc_list.polutils_name != ''
 *
 * what the fuck is that inner join lol LBS code is weird.
 */

// Store for simplicity
file_put_contents(__DIR__ .'/LookTable.json', json_encode($lookTable, JSON_PRETTY_PRINT));

/**
 *              | --- this is all gear --------|
 * 0x01000C01   E9100421C930B9400251006000700000
 * 
 * 0x01 00 0C 01
 * 
 * 0x01 = skip (if you pull from MySQL you won't have 0x)
 * 00 = look/style
 * 0C = face
 * 01 = race
 */

$races = [
    [
        // None - I think these are objects like gates
    ],
    [
        'name'  => 'Hume Male',
        'dat'   => 'ROM/27/82.DAT',
        'look'  => 'HumeMale',
    ],
    [
        'name'  => 'Hume Female',
        'dat'   => 'ROM/32/58.DAT',
        'look'  => 'HumeFemale',
    ],
    [
        'name'  => 'Elvaan Male',
        'dat'   => 'ROM/37/31.DAT',
        'look'  => 'ElvaanMale',
    ],
    [
        'name'  => 'Elvaan Female',
        'dat'   => 'ROM/42/4.DAT',
        'look'  => 'ElvaanFemale',
    ],
    [
        'name'  => 'Tarutaru Male',
        'dat'   => 'ROM/46/93.DAT',
        'look'  => 'Tarutaru',
    ],
    [
        'name'  => 'Tarutaru Female',
        'dat'   => 'ROM/46/93.DAT',
        'look'  => 'Tarutaru',
    ],
    [
        'name'  => 'Mithra',
        'dat'   => 'ROM/51/89.DAT',
        'look'  => 'Mithra',
    ],
    [
        'name'  => 'Galkae',
        'dat'   => 'ROM/56/59.DAT',
        'look'  => 'Galka',
    ],
    
    // There is nothign for a bit here

    // 29 = Mithra Child
    // 30 = Hume/Elvaan Child F
    // 31 = Hume/Elvaan child M

    // 32,33,34,35,36 = Chocobo
];

/**
 * This struct is based on the character length for
 * each bit of data, with gear being mostly 4 bytes.
 * 
 * As it loops through the struct, it will increment
 * a pointer, starting from 0
 */
$struct = [
    'init'      => 4, // init 0x01 (this will check for 0x)
    'stylelock' => 2, // stylelock
    'face'      => 2, // face
    'race'      => 2, // race

    // gear struct
    'head'      => 4, // head
    'body'      => 4, // body
    'hands'     => 4, // hands
    'legs'      => 4, // legs
    'feet'      => 4, // feet
    'main'      => 4, // main
    'sub'       => 4, // sub
    'ranged'    => 4, // ranged
];

/**
 * Basically matches my "struct" to the json 
 * filename in "gear_dat_json", i could have named them
 * the same but this is more convoluted :D
 */
$gearslots = [
    'head'   => 'Heads',
    'body'   => 'Body',
    'hands'  => 'Hands',
    'legs'   => 'Legs',
    'feet'   => 'Feet',
    'main'   => 'Main',
    'sub'    => 'Sub',
    'ranged' => 'Ranged'
];

$total = count($npcList);
console("Total Npcs: {$total}");
console("Starting...");

foreach ($npcList as $i => $npcRow) {
    $remaining = $total - $i;

    $pointer = 0;
    $npc = [];

    [$id, $name, $rotation, $x, $y, $z, $look, $content, $zone] = str_getcsv($npcRow);

    foreach ($struct as $type => $length) {
        if ($type == 'init') {
            // if there is 0x, increment by normal length otherwise by 2
            $pointer += substr($look, 0, 2) == '0x' ? $length : 2;
            continue;
        }

        if ($type == 'stylelock') {
            // we don't care about this for NPCs
            $pointer += $length;
            continue;
        }

        // grab the data
        $lookdata = substr($look, $pointer, $length);

        // debugging
        // print_r([$type, $look, $lookdata, hexdec($lookdata)]);

        // if this is face or race, just hexdec it and continue
        if (in_array($type, ['face', 'race'])) {
            $npc[$type] = hexdec($lookdata);
            $pointer += $length;
            continue;
        }

        // onto gear
        $pointer += $length;

        // grab the model_id
        $npc[$type] = getModelIdFromLookData($lookdata);
    }

    // if no race or face data, skip!
    // will also skip body for now, pain in the ass.. eg: "Conflux Surveyor" not in the Body.json
    $race = $races[$npc['race']] ?? null;
    $face = $races[$npc['face']] ?? null;
    $body = $races[$npc['body']] ?? null;

    if (!$race || !$face || !$body) {
        writeFailedEntry($npc);
        continue;
    }

    $npc['race'] = $race;

    // grab the "look" name which matches the look table
    $lookrace = $npc['race']['look'];

    // find your face!
    $npc['face'] = findModelData($lookrace, 'Faces', $npc['face']);

    foreach ($gearslots as $slot => $lookslot) {
        $npc[$slot] = findModelData($lookrace, $lookslot, $npc[$slot]);
    }

    $npc['data'] = [
        'id' => $id,
        'name' => $name,
        'zone' => $zone,
        'look' => $look,
        'content' => $content,
        'pos' => [
            'rotation' => $rotation,
            'x' => $x,
            'y' => $y,
            'z' => $z
        ],
    ];

    // Write noesis file
    writeNoesisFile($npc);
    writeJsonFile($npc);

    console("- {$remaining} togo ... - {$npc['data']['name']} = {$npc['race']['name']}");
}


