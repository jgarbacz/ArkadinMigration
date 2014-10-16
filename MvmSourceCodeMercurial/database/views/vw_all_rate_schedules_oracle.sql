create view vw_all_rate_schedules as
select MAIN.id_acc, MAIN.c_area, MAIN.c_areakey, '1 - override GS' rate_type, '1' rate_type_id, GSB.id_group, SUB.id_sub, SUB.id_po, GSB.tx_name, MBR.vt_start gsub_start, MBR.vt_end gsub_end,
SUB.vt_start sub_start, SUB.vt_end sub_end, PLM.id_pi_template, PLM.id_pricelist, PLM.id_paramtable, 
rp.id_sched, ed1.dt_start rsched_start, ed1.dt_end rsched_end,
ed1.n_begintype, ed1.n_beginoffset, ed1.n_endtype, ed1.n_endoffset 
FROM T_AV_AREAKEY MAIN 
INNER JOIN T_GSUBMEMBER MBR on MAIN.id_acc = MBR.id_acc
INNER JOIN T_GROUP_SUB GSB ON GSB.id_group = MBR.id_group
INNER JOIN T_SUB SUB ON MBR.id_group = SUB.id_group
INNER JOIN T_PL_MAP PLM ON PLM.id_sub = SUB.id_sub
left outer join t_rsched rp on rp.id_pricelist = PLM.id_pricelist and rp.id_pi_template = plm.id_pi_template and rp.id_pt = plm.id_paramtable
left outer join t_effectivedate ed1 on rp.id_eff_date = ed1.id_eff_date
union all
select MAIN.id_acc, MAIN.c_area, MAIN.c_areakey, '2 - override SUB' rate_type, '2' rate_type_id, NULL id_group, SUB.id_sub, SUB.id_po, NULL tx_name, NULL gsub_start, NULL gsub_end,
SUB.vt_start sub_start, SUB.vt_end sub_end, PLM.id_pi_template, PLM.id_pricelist, PLM.id_paramtable, 
rp.id_sched, ed1.dt_start rsched_start, ed1.dt_end rsched_end,
ed1.n_begintype, ed1.n_beginoffset, ed1.n_endtype, ed1.n_endoffset 
FROM T_AV_AREAKEY MAIN 
INNER JOIN T_SUB SUB ON MAIN.id_acc = SUB.id_acc
INNER JOIN T_PL_MAP PLM ON PLM.id_sub = SUB.id_sub
left outer join t_rsched rp on rp.id_pricelist = PLM.id_pricelist and rp.id_pi_template = plm.id_pi_template and rp.id_pt = plm.id_paramtable
left outer join t_effectivedate ed1 on rp.id_eff_date = ed1.id_eff_date
union all
select MAIN.id_acc, MAIN.c_area, MAIN.c_areakey, '3 - PO rate' rate_type, '3' rate_type_id, NULL id_group, SUB.id_sub, SUB.id_po, NULL tx_name, NULL gsub_start, NULL gsub_end,
SUB.vt_start sub_start, SUB.vt_end sub_end, PLM.id_pi_template, PLM.id_pricelist, PLM.id_paramtable, 
rp.id_sched, ed1.dt_start rsched_start, ed1.dt_end rsched_end,
ed1.n_begintype, ed1.n_beginoffset, ed1.n_endtype, ed1.n_endoffset 
FROM T_AV_AREAKEY MAIN 
INNER JOIN T_SUB SUB ON MAIN.id_acc = SUB.id_acc
INNER JOIN T_PL_MAP PLM ON PLM.id_po = SUB.id_po and NVL2(PLM.id_sub,NULL,0) = 0
INNER JOIN t_rsched rp on rp.id_pricelist = PLM.id_pricelist and rp.id_pi_template = plm.id_pi_template and rp.id_pt = plm.id_paramtable
INNER JOIN t_effectivedate ed1 on rp.id_eff_date = ed1.id_eff_date
union all
select MAIN.id_acc, MAIN.c_area, MAIN.c_areakey, '3 - PO rate' rate_type, '3' rate_type_id, GSB.id_group, SUB.id_sub, SUB.id_po, GSB.tx_name, MBR.vt_start gsub_start, MBR.vt_end gsub_end,
SUB.vt_start sub_start, SUB.vt_end sub_end, PLM.id_pi_template, PLM.id_pricelist, PLM.id_paramtable, 
rp.id_sched, ed1.dt_start rsched_start, ed1.dt_end rsched_end,
ed1.n_begintype, ed1.n_beginoffset, ed1.n_endtype, ed1.n_endoffset 
FROM T_AV_AREAKEY MAIN 
INNER JOIN T_GSUBMEMBER MBR on MAIN.id_acc = MBR.id_acc
INNER JOIN T_GROUP_SUB GSB ON GSB.id_group = MBR.id_group
INNER JOIN T_SUB SUB ON MBR.id_group = SUB.id_group
INNER JOIN T_PL_MAP PLM ON PLM.id_po = SUB.id_po and NVL2(PLM.id_sub,NULL,0) = 0
INNER JOIN t_rsched rp on rp.id_pricelist = PLM.id_pricelist and rp.id_pi_template = plm.id_pi_template and rp.id_pt = plm.id_paramtable
INNER JOIN t_effectivedate ed1 on rp.id_eff_date = ed1.id_eff_date
union all
select MAIN.id_acc, MAIN.c_area, MAIN.c_areakey, '4 - default rate' rate_type, '4' rate_type_id, NULL id_group, NULL id_sub, NULL id_po, NULL tx_name, NULL gsub_start, NULL gsub_end,
NULL sub_start, NULL sub_end, rp.id_pi_template, rp.id_pricelist, rp.id_pt, 
rp.id_sched, ed1.dt_start rsched_start, ed1.dt_end rsched_end,
ed1.n_begintype, ed1.n_beginoffset, ed1.n_endtype, ed1.n_endoffset 
FROM T_AV_AREAKEY MAIN 
INNER JOIN T_AV_INTERNAL AI on MAIN.id_acc = AI.id_acc
INNER JOIN t_rsched rp on rp.id_pricelist = AI.c_pricelist
INNER JOIN t_effectivedate ed1 on rp.id_eff_date = ed1.id_eff_date;

