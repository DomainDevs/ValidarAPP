<!--Js Local-->
$('.risk__row').click((evt) => {
  const riskid = evt.currentTarget.dataset.riskid;
  $helpers.setVariable('glb_riskId', riskid);
  $helpers.open(1);
});

$('.client__row').click((evt) => {
  const agentid = evt.currentTarget.dataset.agentid;
  $helpers.setVariable('glb_AgentIdId', agentid);
  $helpers.open(3);
});
