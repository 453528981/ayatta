/*
drop database if exists Base;
create database Base;
*/
use base;
/*
drop table if exists ProdCatg;
create table ProdCatg(
Id int auto_increment not null,
Name nvarchar(100) not null,
ParentId int default 0 not null,
IsParent bool default 0 not null,
Priority int default 0 not null,
Status tinyint default 0 not null,
CreatedOn datetime default current_timestamp  not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='产品分类';


drop table if exists ProdProp;
create table ProdProp(
Id int not null,
CatgId int not null,
ParentPid int not null,
ParentVid int not null,
Name nvarchar(100) not null,
Must bool default 0 not null,
Multi bool default 0 not null,
IsKeyProp bool default 0 not null,
IsSaleProp bool default 0 not null,
IsEnumProp bool default 0 not null,
IsItemProp bool default 0 not null,
IsColorProp bool default 0 not null,
IsInputProp bool default 0 not null,
ChildTemplate nvarchar(50) not null,
AllowAlias bool default 0 not null,
FeatureStr nvarchar(200),
Priority int default 0 not null,
Status tinyint not null default 0 comment '0为可用',
CreatedOn datetime default current_timestamp  not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key (Id,CatgId)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='产品属性名';


drop table if exists ProdPropValue;
create table ProdPropValue(
Id int not null,
CatgId int not null,
PropId int not null,
Name nvarchar(100) not null,
Alias nvarchar(100),
PropName nvarchar(100) not null,
FeatureStr nvarchar(200),
Priority int default 0 not null,
Status tinyint not null default 0 comment '0为可用',
CreatedOn datetime default current_timestamp  not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key (Id,CatgId,PropId)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='产品属性值';

*/

drop table if exists Help;
create table Help(
Id int auto_increment not null comment 'Id',
ParentId int not null default 0 comment '父Id',
Title nvarchar(200) not null default '' comment '标题',
NavUrl nvarchar(300) not null default ''comment '导航URL',
Summary nvarchar(500) not null default '' comment '摘要',
Content nvarchar(8000) not null default '' comment '内容',
Priority int not null default 0 comment '排序优先级 从小到大',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 true可用 false不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='帮助';


drop table if exists Country ;
create table Country( 
Id varchar(3) not null default '' comment 'Id 三位数字代码',
Code varchar(3) not null default '' comment '三位字母代码',
Name nvarchar(50) not null default '' comment '中文名称',
Flag varchar(300) not null default '' comment '国旗',
EnName nvarchar(100) not null default '' comment '英文名称',

Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 ture可用 false不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id),
unique key(Code)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='国家';

drop table if exists Region ;
create table Region( 
Id varchar(12) not null default '' comment 'Id',
Name nvarchar(50) not null default '' comment '名称',
Depth tinyint not null default 0 comment '深度',
ParentId varchar(12) not null default '' comment '上级Id 最上级为国家三位字母代码',
PostalCode varchar(12) not null default '' comment '邮政编码',
GroupId int not null default 0 comment '分组Id 可能会把某些省归入华北 华南这种地理大区',
Priority int not null default 0 comment '排序优先级 从小到大',
Badge nvarchar(200) not null default '' comment '徽章 标记',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 ture可用 false不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id,ParentId)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='行政区';


drop table if exists OAuthProvider;
create table OAuthProvider(
Id nvarchar(30) not null comment 'Id (qq sina等)',
Name nvarchar(30) not null default '' comment '名称',
ClientId varchar(256) not null default '' comment 'ClientId',
ClientSecret varchar(300) not null default '' comment 'ClientSecret',
Scope varchar(300) not null default '' comment 'Scope',
CallbackEndpoint varchar(300) not null default '' comment 'CallbackEndpoint',
BaseUrl varchar(300) not null default '' comment 'BaseUrl',
AuthorizationEndpoint varchar(300) not null default '' comment 'AuthorizationEndpoint',
TokenEndpoint varchar(300) not null default '' comment 'TokenEndpoint',
UserEndpoint varchar(300) not null default '' comment 'UserEndpoint',
Priority int not null default 0 comment '排序优先级 从小到大',
Badge nvarchar(200) not null default '' comment '徽章 标记',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 true可用 false不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='OAuthProvider';

drop table if exists LogisticsCompany ;
create table LogisticsCompany( 
Id int auto_increment not null comment 'Id',
Code varchar(6) not null default '' comment '编码',
Name nvarchar(50) not null default '' comment '名称',
Regex varchar(200) not null default '' comment '编码正则表达式',
GroupId int not null default 0 comment '分组Id',
Priority int not null default 0 comment '排序优先级 从小到大',
Badge nvarchar(200) not null default '' comment '徽章 标记',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 true可用 false不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='物流公司';

drop table if exists SlideItem;
drop table if exists Slide;
create table Slide( 
Id nvarchar(15) not null comment 'Id',
Name nvarchar(50) not null default '' comment '名称',
Title nvarchar(50) not null default '' comment '标题',
Width int not null default 0 comment '宽',
Height int not null default 0 comment '高',
Thumb bool not null default 0 comment '是否有缩略图',
ThumbW int not null default 0 comment '缩略图高',
ThumbH int not null default 0 comment '缩略图宽',
Description nvarchar(300) not null default '' comment '描述',
Priority int not null default 0 comment '排序优先级 从小到大',
Badge nvarchar(200) not null default '' comment '徽章 标记',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 true可用 false不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='幻灯片';


create table SlideItem( 
Id int auto_increment not null comment 'Id',
SlideId nvarchar(15) not null comment 'SlideId',
GroupId int not null default 0 comment '分组id',
Name nvarchar(50) not null default '' comment '名称',
Title nvarchar(50) not null default '' comment '标题',
NavUrl nvarchar(300) not null default '' comment '导航URL',
ImageSrc nvarchar(300) not null default '' comment '图片Src',
ThumbSrc nvarchar(300) not null default '' comment '缩略图Src',
Description nvarchar(300) not null default '' comment '描述',
StartedOn datetime not null default now() comment '开始时间',
StoppedOn datetime not null default now() comment '结束时间',
Priority int not null default 0 comment '排序优先级 从小到大',
Badge nvarchar(200) not null default '' comment '徽章 标记',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 true可用 false不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id),
foreign key(SlideId) references Slide(Id) on delete cascade
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='幻灯片条目';


drop table if exists PaymentBank;
drop table if exists PaymentPlatform;
create table PaymentPlatform( 
Id int auto_increment not null comment 'Id',
Name nvarchar(200) not null default '' comment '支付平台名称',
IconSrc varchar(200) not null default '' comment '支付平台图标',
MerchantId varchar(100) not null default '' comment '支付平台分配的商户编号',
PrivateKey varchar(200) not null default '' comment '密/私钥',
PublicKey varchar(500) not null default '' comment '公钥',
GatewayUrl varchar(500) not null default '' comment '支付平台网关',
CallbackUrl varchar(500) not null default '' comment '支付回调URL',
NotifyUrl varchar(500) not null default '' comment '支付通知URL',
QueryUrl varchar(500) not null default '' comment '查询URL',
RefundUrl varchar(500) not null default '' comment '退款URL',
Description nvarchar(4000) not null default '' comment '描述',
Priority int not null default 0 comment '排序优先级 从小到大',
Badge nvarchar(200) not null default '' comment '徽章 标记 用于活动时提醒',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 1可用 0不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='支付平台';

drop table if exists Bank;
create table Bank( 
Id int auto_increment not null comment 'Id',
Name nvarchar(200) not null default '' comment '银行名称',
IconSrc varchar(200) not null default '' comment '银行图标',
Description nvarchar(4000) not null default '' comment '描述',
Priority int not null default 0 comment '排序优先级 从小到大',
Badge nvarchar(200) not null default '' comment '徽章 标记 用于活动时提醒',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 1可用 0不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='银行';


create table PaymentBank( 
Id int auto_increment not null comment 'Id',
BankId int not null default 0 comment '银行Id',
PlatformId int not null default 0 comment '支付平台Id',
Code varchar(100) not null default '' comment '银行在支付平台的Code',
Description nvarchar(4000) not null default '' comment '描述',
Priority int not null default 0 comment '排序优先级 从小到大',
Badge nvarchar(200) not null default '' comment '徽章 标记 用于活动时提醒',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 1可用 0不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id),
foreign key(BankId) references Bank(Id) on delete cascade,
foreign key(PlatformId) references PaymentPlatform(Id) on delete cascade
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='支付平台银行';



drop table if exists AdModule;
create table AdModule( 
Id int auto_increment not null comment 'Id',
ParentId int not null default 0 comment '父Id',
Name nvarchar(50) not null default '' comment '名称',
Title nvarchar(50) not null default '' comment '标题',
Path varchar(50) not null default '' comment '全路径',
Depth int not null default 0 comment '深度',
Icon nvarchar(300) not null default '' comment '图标',
Picture nvarchar(300) not null default '' comment '图片',
Summary nvarchar(300) not null default '' comment '描述',
Priority int not null default 0 comment '排序优先级',
Badge nvarchar(200) not null default '' comment '徽章 标记',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 true可用 false不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='广告模块';

drop table if exists AdItem;
create table AdItem( 
Id int auto_increment not null comment 'Id',
ModuleId int not null default 0 comment '模块Id',
GroupId int not null default 0 comment '分组Id',
Name nvarchar(50) not null default '' comment '名称',
Title nvarchar(50) not null default '' comment '标题',
Data nvarchar(4000) not null default '' comment '数据',
Link nvarchar(300) not null default '' comment '链接',
Icon nvarchar(300) not null default '' comment '图标',
Picture nvarchar(300) not null default '' comment '图片',
Summary nvarchar(300) not null default '' comment '描述',
StartedOn datetime not null default now() comment '开始时间',
StoppedOn datetime not null default now() comment '结束时间',
Priority int not null default 0 comment '排序优先级 从小到大',
Badge nvarchar(200) not null default '' comment '徽章 标记',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 true可用 false不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='广告条目';