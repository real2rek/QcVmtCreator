### What is QcVmtCreator?

QcVmtCreator is a small piece of software that will ease your workflow when doing post-modelling work for Source Engine. 

You start with modelling and naming your materials and smd's using prefixes and suffixes. 
They are later used to automatically generate qc's and all materials. 
This software was created to speed up the process of creating those text files. 
And also to allow developer to create much more models without worrying about dull "copy paste" work afterwards. 

### How to use it?

* Start by creating prefix (codename) of your project that will be used (ex. "tr" for trees)
* Name all meshes using this codename (ex. "tr_tree1")
* Name all materials using proper suffix (ex. "tr_tree1_glow", "tr_tree1_base"). 
You can edit base vmt's provided with the program or create your own. (see "mat_base" etc. in main directory).
Software will try to match material name he gathered with ones that are placed in main directory.
* Name all phys meshes using "_p" suffix. (ex. "tr_tree1_p")
* Launch program and provide it with your nickname and path to the smd files
* Program will generate all text files
