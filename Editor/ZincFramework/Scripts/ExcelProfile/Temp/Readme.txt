第一行为此数据文件字段的名字
第二行是其数据文件字段的类型
第三行的key决定了数据结构容器类使用的字典的键是哪一个
第四行的serializableCode决定了其序列化时的唯一Code

脚本名字填在第四行第四列，在其他同样的数据文件中填过可以不填，但要注意字段类型必须全部相同，且不能存在同名但是字段类型不相同的类
命名空间填在第四行第六列，规则同脚本的名字

第五行是注释，想写什么就写什么

生成的数据文件名字是根据下面的表名来生成的，表名是什么就会生成什么名字的数据文件

支持的类型有int，float，string，double，bool，枚举，以及数组

写入枚举时，枚举需要填入枚举值对应的数字
写入数组时，请注意数组的所有元素需要用斜杠隔开

The first line is the name of this data file field
The second line is the type of its datafile field
The key in the third line determines which key of the dictionary the data structure container class uses.
The serializableCode in the fourth line determines its unique code for serialization.

The name of the script is in the fourth column of the fourth line, and can be left out if it has been filled in other data files of the same type, but note that the field types must all be the same, and there can't be a class with the same name but different field types.
The name of the namespace is in the fourth column of the fourth line.Its rule is same as script name.

The fifth line is the comment, you can write whatever you want.

The name of the generated data file is based on the following table name to generate, what is the table name will generate what the name of the data file

The supported types are int, float, string, double, bool, enumeration, and array.

When writing to an enumeration, the enumeration needs to be filled with the number corresponding to the value of the enumeration.
When writing arrays, please note that all elements of the array need to be separated by slashes.