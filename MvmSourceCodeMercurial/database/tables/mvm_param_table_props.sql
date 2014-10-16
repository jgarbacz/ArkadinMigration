create table mvm_param_table_props (
paramtable_name VARCHAR(100),column_name VARCHAR(100),
display_name VARCHAR(100), is_condition VARCHAR(100),
column_operator VARCHAR(100), is_operator VARCHAR(100),
has_operator_name VARCHAR(1000),
is_action VARCHAR(100),
column_type VARCHAR(100),
 enum_namespace VARCHAR(1000), enum_type VARCHAR(1000),
 column_length INTEGER, is_required VARCHAR(100),
 default_value VARCHAR(1000));


insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BaseTransportRate','ServiceLevel','Service Level','Y','=','N','','N','enum','premconf.com/Conferencing','PremServiceLevel','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BaseTransportRate','CountryBridge','Bridge Country','Y','=','N','','N','enum','premconf.com/accountcreation','CountryCode','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BaseTransportRate','Transport','Transport','Y','=','N','','N','enum','premconf.com/Conferencing','Transport','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BaseTransportRate','CalendarCode','Calendar Code','Y','=','N','','N','enum','metratech.com/calendar','CalendarCode','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BaseTransportRate','TotalQualifiedMinutes_op','','N','','Y','','N','string','','','5','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BaseTransportRate','TotalQualifiedMinutes','Total Qualified Minutes','Y','','N','TotalQualifiedMinutes_op','N','decimal','','','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BaseTransportRate','BaseTransportRate','Base Transport Rate','N','','N','','Y','decimal','','','','Y','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BaseTransportRate','BaseTransportSetupCharge','Base Transport Setup Charge','N','','N','','Y','decimal','','','','Y','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BaseTransportRate','BaseTransportMinCharge','Base Transport Min Charge','N','','N','','Y','decimal','','','','Y','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BaseTransportRate','BaseTransportMinUOM','Transport Minimum Charge UOM','N','','N','','Y','enum','premconf.com/Conferencing','UOM','','Y','Minutes');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BaseTransportRate','UserRole','User Role','Y','=','N','','N','enum','premconf.com/Conferencing','PartTypeID','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BaseTransportRate','IsInternational','Is International?','Y','=','N','','N','boolean','','','','N','F');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','ServiceLevel','Service Level','Y','=','N','','N','enum','premconf.com/Conferencing','PremServiceLevel','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','Transport','Transport','Y','=','N','','N','enum','premconf.com/Conferencing','Transport','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','ActionBand','Band','Y','=','N','','N','integer','','','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','CountryBridge','Bridge Country','Y','=','N','','N','enum','premconf.com/accountcreation','CountryCode','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','CountryOrigin','Origin Country','Y','=','N','','N','enum','premconf.com/accountcreation','CountryCode','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','CountryDestination','Destination Country','Y','=','N','','N','enum','premconf.com/accountcreation','CountryCode','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','UserRole','User Role','Y','=','N','','N','enum','premconf.com/Conferencing','PartTypeID','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','CalendarCode','Calendar Code','Y','=','N','','N','enum','metratech.com/calendar','CalendarCode','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','TotalQualifiedMinutes_op','','N','','Y','','N','string','','','5','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','TotalQualifiedMinutes','Total Qualified Minutes','Y','','N','TotalQualifiedMinutes_op','N','decimal','','','','N','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','BridgeRate','Bridge Rate','N','','N','','Y','decimal','','','','Y','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','BridgeSetupCharge','Bridge Setup Charge','N','','N','','Y','decimal','','','','Y','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','BridgeMinCharge','Bridge Minimum Charge','N','','N','','Y','decimal','','','','Y','');

insert into mvm_param_table_props (paramtable_name,column_name, display_name, is_condition, column_operator, is_operator, has_operator_name, is_action,column_type, enum_namespace, enum_type, column_length, is_required, default_value)
values ('BridgeRate','BridgeMinUOM','Transport Minimum Charge (Unit of Measure)','N','','N','','Y','enum','premconf.com/Conferencing','UOM','','Y','Minutes');

