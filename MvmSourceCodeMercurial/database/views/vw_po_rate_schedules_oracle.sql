create or replace view vw_po_rate_schedules as
select NULL id_acc, NULL c_area, NULL c_areakey, '3 - PO rate' rate_type, '3' rate_type_id, NULL id_group, NULL id_sub, PLM.id_po, NULL tx_name, NULL gsub_start, NULL gsub_end,
NULL sub_start, NULL sub_end, ED.dt_start po_start, ED.dt_end po_end, PLM.id_pi_template, PLM.id_pricelist, PLM.id_paramtable, 
rp.id_sched, ed1.dt_start rsched_start, ed1.dt_end rsched_end,
ed1.n_begintype, ed1.n_beginoffset, ed1.n_endtype, ed1.n_endoffset 
FROM T_PO PO
inner join T_EFFECTIVEDATE ED on PO.id_eff_date = ED.id_eff_date
inner join T_PL_MAP PLM ON PLM.id_po = PO.id_po and NVL2(PLM.id_sub,NULL,0) = 0
INNER JOIN t_rsched rp on rp.id_pricelist = PLM.id_pricelist and rp.id_pi_template = plm.id_pi_template and rp.id_pt = plm.id_paramtable
INNER JOIN t_effectivedate ed1 on rp.id_eff_date = ed1.id_eff_date
;
