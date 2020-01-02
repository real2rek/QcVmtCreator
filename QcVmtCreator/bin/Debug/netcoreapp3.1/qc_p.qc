$modelname		"<name>/<prefix>/<filename>.mdl"
$body <filename>	"<filename>.smd"

$cdmaterials	"<name>/<prefix>/"
$scale			1
$sequence idle	"idle.smd" loop fps 1

$collisionmodel	"<physmodel>" { 
	$mass "150"
	$concave
}

$keyvalues
{
   "prop_data"
   {
       "base"  "Metal.Large"
   }
}

