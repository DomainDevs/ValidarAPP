<!--Js Local-->
$('.client__row').click((evt) => {
  const agentid = evt.currentTarget.dataset.agentid;
  $helpers.setVariable('glb_AgentIdId', agentid);
  $helpers.open(1);
});