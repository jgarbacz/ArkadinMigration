-- exec mtsp_snapshot_metrics
-- drop procedure mtsp_snapshot_metrics
CREATE PROCEDURE MTSP_SNAPSHOT_METRICS
AS
BEGIN
if object_id(N'[MT_AGGREGATE_METRICS]') is not null
			drop table [MT_AGGREGATE_METRICS]
create table MT_AGGREGATE_METRICS (metric_type VARCHAR(1000), metric_name VARCHAR(1000), metric_id INT, quantity INT, units VARCHAR(1000));

insert into MT_AGGREGATE_METRICS select 'usage by interval','total t_acc_usage rows for interval ' + CONVERT(varchar,id_usage_interval), id_usage_interval, COUNT(*), 'usage rows' from t_acc_usage
group by id_usage_interval;
        
declare @query VARCHAR(4000)  
declare intervalCursor cursor LOCAL FORWARD_ONLY for
     select d.id_interval from (select b.id_usage_cycle, MAX(a.quantity) max_quantity from MT_AGGREGATE_METRICS a
inner join t_usage_interval b on a.metric_id = b.id_interval
where metric_name like 'total t_acc_usage rows for interval %'
group by b.id_usage_cycle) c
inner join t_usage_interval d on c.id_usage_cycle = d.id_usage_cycle
inner join MT_AGGREGATE_METRICS e on d.id_interval = e.metric_id and c.max_quantity = e.quantity 

 open intervalCursor
 declare @intervalid INT 
 fetch next from intervalCursor
    into @intervalid

while @@fetch_status = 0
        begin
		set @query = 'insert into MT_AGGREGATE_METRICS select ''top usage by interval'', ''total t_acc_usage rows for interval ' + CONVERT(varchar,@intervalid)+ ''', ' + CONVERT(varchar,@intervalid)+ ', count(*), ''usage rows''
         from t_acc_usage where id_usage_interval = ' + CONVERT(varchar,@intervalid) 
		EXECUTE(@query)
		set @query = 'insert into MT_AGGREGATE_METRICS select top 10 ''usage by account by interval'', ''total t_acc_usage rows for account '' + CONVERT(varchar,id_acc) 
			+ '' interval ' + CONVERT(varchar,@intervalid)+ ''', id_acc, count(*), ''usage rows''
         from t_acc_usage where id_usage_interval = ' + CONVERT(varchar,@intervalid) 
         + ' group by id_acc order by count(*) desc' 
		EXECUTE(@query)
		set @query = 'insert into MT_AGGREGATE_METRICS select top 10 ''usage by payee by interval'', ''total t_acc_usage rows for account '' + CONVERT(varchar,id_payee) 
			+ '' interval ' + CONVERT(varchar,@intervalid)+ ''', id_payee, count(*), ''usage rows''
         from t_acc_usage where id_usage_interval = ' + CONVERT(varchar,@intervalid) 
         + ' group by id_payee order by count(*) desc' 
		EXECUTE(@query)
		set @query = 'insert into MT_AGGREGATE_METRICS select top 10 ''usage by ancestor by interval'', ''total t_acc_usage rows for ancestor '' + CONVERT(varchar,id_ancestor) 
			+ '' interval ' + CONVERT(varchar,@intervalid)+ ''', id_ancestor, count(*), ''usage rows''
         from t_acc_usage a
         inner join t_account_ancestor b on a.id_acc = b.id_descendent and b.num_generations > 0
         where id_usage_interval = ' + CONVERT(varchar,@intervalid) 
         + ' group by id_ancestor order by count(*) desc' 
		EXECUTE(@query)
	set @query = 'insert into MT_AGGREGATE_METRICS select ''accounts by interval'', ''total account rows for interval ' + CONVERT(varchar,@intervalid)+ ''', NULL, count(*), ''accounts''
         from t_acc_usage_interval where id_usage_interval = ' + CONVERT(varchar,@intervalid) 
		EXECUTE(@query)	
	set @query = 'insert into MT_AGGREGATE_METRICS select ''credit card accounts by interval'', ''total credit card accounts for interval ' + CONVERT(varchar,@intervalid)+ ''', NULL, count(*), ''credit card accounts''
         from t_acc_usage_interval a
		inner join t_av_internal b on a.id_acc = b.id_acc
       		inner join t_enum_data c on b.c_PaymentMethod = c.id_enum_data and c.nm_enum_data like ''%/Credit%''
	where a.id_usage_interval = ' + CONVERT(varchar,@intervalid) 
		EXECUTE(@query)	

	set @query = 'insert into MT_AGGREGATE_METRICS select ''adapters by interval'', ''total seconds spent in interval ' 
		+ CONVERT(varchar,@intervalid)+ ' on adapter '' + a.tx_name + '' run '' + CONVERT(varchar,c.id_instance) + '' ('' + c.tx_status +'')'',
	       	b.id_arg_interval, DATEDIFF(second,c.dt_start, c.dt_end), ''seconds''
         	from t_recevent a
		inner join t_recevent_inst b on a.id_event = b.id_event
		inner join t_recevent_run c on b.id_instance = c.id_instance 
		where id_arg_interval = ' + CONVERT(varchar,@intervalid) 
		EXECUTE(@query)		

	set @query = 'insert into MT_AGGREGATE_METRICS select ''EOP time by interval'', ''total seconds spent in interval ' 
		+ CONVERT(varchar,@intervalid) + ''',
	       	min(b.id_arg_interval), DATEDIFF(second,min(c.dt_start), max(c.dt_end)), ''seconds''
         	from t_recevent a
		inner join t_recevent_inst b on a.id_event = b.id_event
		inner join t_recevent_run c on b.id_instance = c.id_instance 
		where b.id_arg_interval = ' + CONVERT(varchar,@intervalid) 
		EXECUTE(@query)		
		
		fetch next from intervalCursor
			into @intervalid
        end

close intervalCursor
deallocate intervalCursor

if object_id(N'[MT_SPACE_USED]') is not null
			drop table [MT_SPACE_USED]
			
CREATE TABLE MT_SPACE_USED
(name varchar(100), 
rows   int,
reserved varchar(100),
reserved_int int default(0),
data varchar(100),
data_int int default(0),
index_size varchar(18),
index_size_int int default(0),
unused varchar(18),
unused_int int default(0),
avg_row_width int default(0),
avg_index_bytes_per_row int default(0),
column_count int default(0)
)

if object_id(N'[MT_SU]') is not null
			drop table [MT_SU]
			
CREATE TABLE MT_SU
(name varchar(100), 
rows   int,
reserved varchar(100),
data varchar(100),
index_size varchar(18),
unused varchar(18)
)

if object_id(N'[MT_PARTITION_SPACE_USED]') is not null
			drop table [MT_PARTITION_SPACE_USED]

CREATE TABLE MT_PARTITION_SPACE_USED
(name varchar(100), 
rows   int,
reserved varchar(100),
reserved_int int default(0),
data varchar(100),
data_int int default(0),
index_size varchar(18),
index_size_int int default(0),
unused varchar(18),
unused_int int default(0),
avg_row_width int default(0),
avg_index_bytes_per_row int default(0),
column_count int default(0),
partition_name varchar(100) 
)

declare @curr_db_name VARCHAR(1000)

SELECT @curr_db_name = DB_NAME()

declare partitionCursor cursor LOCAL FORWARD_ONLY for
     select name from sys.sysdatabases where name like 'N_20%'

 open partitionCursor
 declare @partition_name varchar(1000) 
 fetch next from partitionCursor
    into @partition_name

while @@fetch_status = 0
        begin
        delete MT_SU
        EXECUTE('EXEC ' + @partition_name + '..sp_MSforeachtable @command1=
         "INSERT INTO ' + @curr_db_name + '..MT_SU
          EXEC sp_spaceused ''?''"')
          insert into MT_PARTITION_SPACE_USED ([name],[rows],[reserved],[data],[index_size],
				[unused],[partition_name])
				select *, @partition_name from MT_SU
		fetch next from partitionCursor
			into @partition_name
        end

close partitionCursor
deallocate partitionCursor

delete MT_SU

 UPDATE MT_PARTITION_SPACE_USED SET
reserved_int = CAST(SUBSTRING(reserved, 1,CHARINDEX(' ', reserved)) AS int),
data_int = CAST(SUBSTRING(data, 1,CHARINDEX(' ', data)) AS int),
index_size_int = CAST(SUBSTRING(index_size, 1,CHARINDEX(' ', index_size)) AS int),
unused_int = CAST(SUBSTRING(unused, 1,CHARINDEX(' ', unused)) AS int)

insert into MT_SPACE_USED ([name],[rows],[reserved],[data],[index_size],[unused])
	select name + '[partitioned]', sum(rows), convert(varchar,sum(reserved_int)) + ' KB', convert(varchar,sum(data_int)) + ' KB', convert(varchar,sum(index_size_int)) + ' KB', convert(varchar,sum(unused_int)) + ' KB' from MT_PARTITION_SPACE_USED
	group by name + '[partitioned]'

EXEC sp_MSforeachtable @command1=
         "INSERT INTO MT_SPACE_USED
           ([name],[rows],[reserved],[data],[index_size],[unused])
          EXEC sp_spaceused '?'"
 
 UPDATE MT_SPACE_USED SET
reserved_int = CAST(SUBSTRING(reserved, 1,CHARINDEX(' ', reserved)) AS int),
data_int = CAST(SUBSTRING(data, 1,CHARINDEX(' ', data)) AS int),
index_size_int = CAST(SUBSTRING(index_size, 1,CHARINDEX(' ', index_size)) AS int),
unused_int = CAST(SUBSTRING(unused, 1,CHARINDEX(' ', unused)) AS int)

 UPDATE MT_SPACE_USED SET
avg_row_width = (convert(bigint,data_int)*1024)/(rows + 1),
avg_index_bytes_per_row = (convert(bigint,index_size_int)*1024)/(rows + 1);


update MT_SPACE_USED set column_count = (select COUNT(*) from sysobjects a
inner join syscolumns b on a.id = b.id
where a.name = MT_SPACE_USED.name);

update MT_SPACE_USED set column_count = (select COUNT(*) from sysobjects a
inner join syscolumns b on a.id = b.id
where MT_SPACE_USED.name = a.name+'[partitioned]')
where MT_SPACE_USED.name like '%|[partitioned|]%' escape '|'

  

insert into MT_AGGREGATE_METRICS select top 500 'accounts by ancestor','total descendent rows for id_acc ' + CONVERT(varchar,id_ancestor), MAX(num_generations), COUNT(*), 'children' from t_account_ancestor
where num_generations != 0
group by id_ancestor order by count(*) desc;

insert into MT_AGGREGATE_METRICS select top 100 'first generation by ancestor','total first generation descendent rows for id_acc ' + CONVERT(varchar,id_ancestor), MAX(num_generations), COUNT(*), 'children' from t_account_ancestor
where num_generations = 1
group by id_ancestor order by count(*) desc;

insert into MT_AGGREGATE_METRICS select top 500 'accounts by payer','total billed accounts for id_acc ' + CONVERT(varchar,id_payer), id_payer, COUNT(*), 'billable children' from t_payment_redirection
group by id_payer order by count(*) desc;

insert into MT_AGGREGATE_METRICS select 'average accounts per payer','average accounts per payer', count(*), avg(total), 'billable children' from (
		select id_payer, COUNT(*) total 
		from t_payment_redirection
		group by id_payer
	) a;

insert into MT_AGGREGATE_METRICS select 'max hierarchy depth','max hierarchy depth', NULL, max(num_generations), 'num_generations' from t_account_ancestor;

insert into MT_AGGREGATE_METRICS select 'bill cycles','bill cycles per month', NULL, count(distinct id_usage_cycle), 'usage cycles' from t_usage_interval;

create table #temp_obj_lines (data varchar(MAX))

declare objCursor cursor LOCAL FORWARD_ONLY for
     select name, type from sysobjects where type in('P','V','FN') 

 open objCursor
 declare @objname VARCHAR(1000) 
 declare @objtype VARCHAR(1000)
 declare @proc_row_counter INTEGER
 declare @view_row_counter INTEGER
 declare @func_row_counter INTEGER
 fetch next from objCursor into @objname, @objtype

        set @proc_row_counter = 0
        set @view_row_counter = 0
        set @func_row_counter = 0

while @@fetch_status = 0
        begin
        print @objname
			set @query = ''
			truncate table #temp_obj_lines
			insert into #temp_obj_lines
			EXEC    mtsp_helptext @objname 
			declare objRowCursor cursor LOCAL FORWARD_ONLY for
				select data from #temp_obj_lines
			open objRowCursor
			declare @datarow VARCHAR(MAX)
			fetch next from objRowCursor into @datarow
			while @@fetch_status = 0
				begin
				if (@objtype = 'P') set @proc_row_counter = @proc_row_counter + 1
				if (@objtype = 'V') set @view_row_counter = @view_row_counter + 1
				if (@objtype = 'FN') set @func_row_counter = @func_row_counter + 1
				fetch next from objRowCursor into @datarow
				end
			close objRowCursor
			deallocate objRowCursor
			fetch next from objCursor into @objname, @objtype
		end
	close objCursor
	deallocate objCursor

drop table #temp_obj_lines 
insert into MT_AGGREGATE_METRICS values ('source code','lines of DB stored procedure code', NULL, @proc_row_counter, 'lines');
insert into MT_AGGREGATE_METRICS values ('source code','lines of DB view code', NULL, @view_row_counter, 'lines');
insert into MT_AGGREGATE_METRICS values ('source code','lines of DB function code', NULL, @func_row_counter, 'lines');
END

GO


