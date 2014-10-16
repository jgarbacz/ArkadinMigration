create view vw_gsub_rate_schedules as 
select MAIN.id_acc, '1 - override GSUB' rate_type, '2' rate_type_id, SUB.id_group id_group, SUB.id_sub, SUB.id_po, NULL tx_name, NULL gsub_start, NULL gsub_end,
SUB.vt_start sub_start, SUB.vt_end sub_end, PLM.id_pi_template, PLM.id_pricelist, PLM.id_paramtable, 
rp.id_sched, ed1.dt_start rsched_start, case substring(convert(varchar,ed1.dt_end, 120),12,12) when '23:59:59' then dateadd(second,1,ed1.dt_end) else ed1.dt_end end rsched_end,
ed1.n_begintype, ed1.n_beginoffset, ed1.n_endtype, ed1.n_endoffset 
FROM T_SUB SUB 
INNER JOIN T_PL_MAP PLM ON PLM.id_sub = SUB.id_sub
left outer join t_rsched rp on rp.id_pricelist = PLM.id_pricelist and rp.id_pi_template = plm.id_pi_template and rp.id_pt = plm.id_paramtable
left outer join t_effectivedate ed1 on rp.id_eff_date = ed1.id_eff_date
where SUB.id_group IS NOT NULL
union all
select MAIN.id_acc, '3 - PO rate' rate_type, '3' rate_type_id, SUB.id_group id_group, SUB.id_sub, SUB.id_po, NULL tx_name, NULL gsub_start, NULL gsub_end,
SUB.vt_start sub_start, SUB.vt_end sub_end, PLM.id_pi_template, PLM.id_pricelist, PLM.id_paramtable, 
rp.id_sched, ed1.dt_start rsched_start, case substring(convert(varchar,ed1.dt_end, 120),12,12) when '23:59:59' then dateadd(second,1,ed1.dt_end) else ed1.dt_end end rsched_end,
ed1.n_begintype, ed1.n_beginoffset, ed1.n_endtype, ed1.n_endoffset 
FROM T_SUB SUB 
INNER JOIN T_PL_MAP PLM ON PLM.id_po = SUB.id_po and PLM.id_sub IS NULL
INNER JOIN t_rsched rp on rp.id_pricelist = PLM.id_pricelist and rp.id_pi_template = plm.id_pi_template and rp.id_pt = plm.id_paramtable
INNER JOIN t_effectivedate ed1 on rp.id_eff_date = ed1.id_eff_date
where SUB.id_group IS NOT NULL

