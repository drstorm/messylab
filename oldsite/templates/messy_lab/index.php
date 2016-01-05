<?php

// no direct access
defined( '_JEXEC' ) or die( 'Restricted access' );
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="<?php echo $this->language; ?>" lang="<?php echo $this->language; ?>" >
<head>
<jdoc:include type="head" />
<link rel="stylesheet" href="<?php echo $this->baseurl ?>/templates/system/css/system.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $this->baseurl ?>/templates/system/css/general.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $this->baseurl ?>/templates/messy_lab/css/template.css" type="text/css" />
</head>
<body id="page_bg" class="color_red bg_black width_<?php echo $this->params->get('widthStyle'); ?>">
<a name="up" id="up"></a>
<div id="wrapper1">
<div id="wrapper2">
<div id="wrapper3">

      <?php if (JRequest::getVar('view') == 'frontpage') : ?>
      <div id="header_big">
        <div id="logo_big" onclick="javascript: window.location.href = '<?php echo JURI::base(); ?>'"></div>
      </div>
      <?php else: ?>
      <div id="header_small">
        <div id="logo_small" onclick="javascript: window.location.href = '<?php echo JURI::base(); ?>'"></div>
      </div>
      <?php endif; ?>

      <div id="tabarea">
        <div id="topmenu_w">
        <table cellpadding="0" cellspacing="0" class="topmenu_t">
          <tr>
            <td class="topmenu_t_m">
              <div id="topmenu">
                <jdoc:include type="modules" name="navigation" />
              </div>
            </td>
          </tr>
        </table>
        </div>
        <div id="pathway">
          <jdoc:include type="modules" name="breadcrumb" />
        </div>
      </div>
      
      <div class="clr"></div>
      
      <div id="whitebox">
        <div id="area">
          <jdoc:include type="message" />
          
          <div id="leftcolumn">
            <?php if($this->countModules('left')) : ?>
            <jdoc:include type="modules" name="left" style="rounded" />
            <?php endif; ?>
          </div>

          <?php if($this->countModules('left')) : ?>
          <?php echo "<div id=\"maincolumn\">\n"; ?>
          <?php else: ?>
          <div id="maincolumn_full">
          <?php endif; ?>
            <?php if($this->countModules('user1 or user2')) : ?>
            <table class="nopad user1user2">
              <tr valign="top">
                <?php if($this->countModules('user1')) : ?>
                <td><jdoc:include type="modules" name="user1" style="xhtml" /></td>
                <?php endif; ?>
                <?php if($this->countModules('user1 and user2')) : ?>
                <td class="greyline">&nbsp;</td>
                <?php endif; ?>
                <?php if($this->countModules('user2')) : ?>
                <td><jdoc:include type="modules" name="user2" style="xhtml" /></td>
                <?php endif; ?>
              </tr>
            </table>
            <div id="maindivider"></div>
            <?php endif; ?>
            
            <table class="nopad">
              <tr valign="top">
                <td><jdoc:include type="component" />
                  <jdoc:include type="modules" name="footer" style="xhtml"/></td>
                <?php if($this->countModules('right') and JRequest::getCmd('layout') != 'form') : ?>
                <td class="greyline">&nbsp;</td>
                <td width="170"><jdoc:include type="modules" name="right" style="xhtml"/></td>
                <?php endif; ?>
              </tr>
            </table>
          </div>
          <div class="clr"></div>
        </div>

        <div class="clr"></div>
      </div>
      
      <!--<div id="footerspacer"></div>-->
      <div id="footer">
        <div id="footer_l">
          <div id="footer_r">
            <p id="syndicate">
              <jdoc:include type="modules" name="syndicate" />
            </p>
            <p id="power_by">
			<?php
			echo JText::_('Copyright &copy; ');
			$copyYear = 2010; 
			$curYear = date('Y'); 
			echo $copyYear . (($copyYear != $curYear) ? '-' . $curYear : '');
			echo JText::_('. Miloš Anđelković. ');
			?><?php echo JText::_('Powered by') ?> <a href="http://www.joomla.org">Joomla!</a>. <?php echo JText::_('Valid') ?> <a href="http://validator.w3.org/check/referer">XHTML</a> <?php echo JText::_('and') ?> <a href="http://jigsaw.w3.org/css-validator/check/referer?profile=css3">CSS</a>.
            </p>
          </div>
        </div>
      </div>

</div>
</div>
</div>
<jdoc:include type="modules" name="debug" />
</body>
</html>
